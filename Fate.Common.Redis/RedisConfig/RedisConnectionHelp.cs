using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Linq;
using Fate.Common.Redis.RedisManage;

namespace Fate.Common.Redis.RedisConfig
{
    /// <summary>
    /// redis 缓存链接
    /// </summary>
    public class RedisConnectionHelp
    {
        private static IConfigurationRoot ConfigurationManage = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;
        /// <summary>
        /// redis密码
        /// </summary>
        public static string RedisPassword = ConfigurationManage.GetSection("AppSetting:RedisConfig:Password").Value;
        /// <summary>
        /// 默认访问存储库
        /// </summary>
        public static int RedisDefaultDataBase
        {
            get
            {
                var dataBase = 0;
                var dataBase2 = ConfigurationManage.GetSection("AppSetting:RedisConfig:DefaultDataBase").Value;
                int.TryParse(dataBase2, out dataBase);
                return dataBase;
            }
        }
        /// <summary>
        /// redis连接字符串
        /// </summary>
        public static string RedisConnectionConfig = ConfigurationManage.GetSection("AppSetting:RedisConfig:Connection").Value;

        /// <summary>
        /// 是否开启哨兵模式 1 开启
        /// </summary>
        private static string IsOpenSentinel = ConfigurationManage.GetSection("AppSetting:RedisConfig:IsOpenSentinel").Value;

        private static ISubscriber sentinelsub;
        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> Concache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer RedisConnection
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetFromCache();
                            //判断是否开启集群哨兵模式
                            if (!string.IsNullOrWhiteSpace(IsOpenSentinel) && IsOpenSentinel.Equals("1"))
                            {
                                OpenSentinelManager();
                            }
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 先从缓存中获取 再从配置读取
        /// </summary>
        /// <returns></returns>
        private static ConnectionMultiplexer GetFromCache()
        {
            //判断缓存中是否存在
            if (!Concache.ContainsKey(RedisConnectionConfig))
                Concache[RedisConnectionConfig] = GetManager();
            return Concache[RedisConnectionConfig];
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>

        private static ConnectionMultiplexer GetManager()
        {
            ConfigurationOptions configurationOptions = new ConfigurationOptions()
            {
                AllowAdmin = true,
                Password = RedisPassword,
                DefaultDatabase = RedisDefaultDataBase,
                ConnectTimeout = 300,
            };
            if (string.IsNullOrWhiteSpace(RedisConnectionConfig))
            {
                throw new ArgumentNullException("redis链接字符串未null");
            }
            //获取连接的字符串
            var connections = RedisConnectionConfig.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            connections.ForEach(item =>
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    configurationOptions.EndPoints.Add(item);
                }
            });
            var connect = ConnectionMultiplexer.Connect(configurationOptions);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;

            return connect;
        }

        #region 哨兵集群
        public static ConfigurationOptions sentineloption = new ConfigurationOptions()
        {
            TieBreaker = "",
            CommandMap = CommandMap.Sentinel,
            ServiceName = "mymaster"
        };
        /// <summary>
        /// 订阅哨兵主从切换
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static void OpenSentinelManager(ConfigurationOptions sentineloptions = null)
        {
            //获取哨兵地址
            List<string> sentinelConfig = ConfigurationManage.GetSection("AppSetting:RedisConfig:RedisSentinelIp").Value?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            //哨兵节点
            sentinelConfig.ForEach(a =>
            {
                var endPoint = RedisBase.ParseEndPoints(a);
                if (!sentineloption.EndPoints.Contains(endPoint))
                {
                    sentineloption.EndPoints.Add(a);
                }
            });
            sentineloptions = sentineloptions ?? sentineloption;
            //我们可以成功的连接一个sentinel服务器，对这个连接的实际意义在于：当一个主从进行切换后，如果它外层有Twemproxy代理，我们可以在这个时机（+switch-master事件)通知你的Twemproxy代理服务器，并更新它的配置文件里的master服务器的地址，然后从起你的Twemproxy服务，这样你的主从切换才算真正完成。
            //一般没有代理服务器，直接更改从数据库配置文件，将其升级为主数据库。
            var connect = ConnectionMultiplexer.Connect(sentineloptions);
            sentinelsub = connect.GetSubscriber();

            sentinelsub.Subscribe("+switch-master", (ch, mg) =>
            {
                Console.WriteLine(mg);
            });
        }

        #endregion
        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine("InternalError:Message" + e.Exception.Message);
        }

        #endregion 事件
    }
}
