using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
namespace Fate.Common.Redis.RedisManage
{
    /// <summary>
    /// redis 访问的基类
    /// </summary>
    public class RedisBase : IRedisManage.IRedisBase
    {

        public IDatabase redisDataBase
        {
            get
            {
               return RedisConfig.RedisConnectionHelp.RedisConnection.GetDatabase();
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
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ConvertJson<T>(T val)
        {
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
            return JsonConvert.DeserializeObject<T>(val);
        }

        public List<T> ConvertList<T>(RedisValue[] val)
        {
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
    }
}
