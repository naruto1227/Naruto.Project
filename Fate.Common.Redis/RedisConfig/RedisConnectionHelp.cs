using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Fate.Common.Infrastructure;
using StackExchange.Redis;
namespace Fate.Common.Redis.RedisConfig
{
    /// <summary>
    /// redis 缓存链接
    /// </summary>
    public class RedisConnectionHelp
    {

        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;
        /// <summary>
        /// redis密码
        /// </summary>
        private static string RedisPassword = ConfigurationManage.GetAppSetting("RedisConfig:Password");
        /// <summary>
        /// 默认访问存储库
        /// </summary>
        private static int RedisDefaultDataBase
        {
            get
            {
                var dataBase = 0;
                var dataBase2 = ConfigurationManage.GetAppSetting("RedisConfig:DefaultDataBase");
                int.TryParse(dataBase2, out dataBase);
                return dataBase;
            }
        }
        /// <summary>
        /// redis连接字符串
        /// </summary>
        private static string RedisConnectionConfig = ConfigurationManage.GetAppSetting("RedisConfig:Connection");

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
                //EndPoints = { { ConfigurationManage.GetAppSetting("RedisConfig:connection"), Convert.ToInt32(ConfigurationManage.GetAppSetting("RedisConfig:port")) } }
                EndPoints = { { RedisConnectionConfig } }
            };
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
