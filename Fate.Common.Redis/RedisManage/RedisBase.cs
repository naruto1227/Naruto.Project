using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using Fate.Common.Redis.RedisConfig;

namespace Fate.Common.Redis.RedisManage
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis 访问的基类
    /// </summary>
    public class RedisBase : IRedisManage.IRedisBase
    {
        private IRedisConnectionHelp redisConnectionHelp;
        public RedisBase(IRedisConnectionHelp _redisConnectionHelp)
        {
            redisConnectionHelp = _redisConnectionHelp;
        }
        public IDatabase redisDataBase
        {
            get
            {
                return redisConnectionHelp.RedisConnection.GetDatabase();
            }
        }
        public ConnectionMultiplexer RedisConnection
        {
            get
            {
                return redisConnectionHelp.RedisConnection;
            }
        }

        /// <summary>
        /// 执行缓存库保存
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult DoSave<TResult>(Func<IDatabase, TResult> action)
        {
            return action(redisDataBase);
        }
        /// <summary>
        /// 执行缓存库保存 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void DoSave(Action<IDatabase> action)
        {
            action(redisDataBase);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ConvertJson<T>(T val)
        {
            if (val == null)
                return default;
            return val is string ? val.ToString() : JsonConvert.SerializeObject(val);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public T ConvertObj<T>(RedisValue val)
        {
            if (val.ToString() == null)
                return default;
            return JsonConvert.DeserializeObject<T>(val);
        }

        public List<T> ConvertList<T>(RedisValue[] val)
        {
            if (val == null)
                return default;
            List<T> result = new List<T>();
            foreach (var item in val)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        public RedisKey[] ConvertRedisKeys(List<string> val)
        {
            return val.Select(k => (RedisKey)k).ToArray();
        }

        /// <summary>
        /// 生成EndPoint
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static EndPoint ParseEndPoints(string host, int port)
        {
            IPAddress ip;
            if (IPAddress.TryParse(host, out ip)) return new IPEndPoint(ip, port);
            return new DnsEndPoint(host, port);
        }

        public static EndPoint ParseEndPoints(string hostAndPort)
        {
            if (hostAndPort.IndexOf(":") != -1)
            {
                var obj = hostAndPort.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                var host = obj[0];
                var port = Convert.ToInt32(obj[1]);
                return ParseEndPoints(host, port);
            }
            else
            {
                throw new ApplicationException("hostAndPort error");
            }
        }
    }
}
