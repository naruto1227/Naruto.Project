using Fate.Infrastructure.Redis.IRedisManage;
using Fate.Infrastructure.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.RedisManage
{
    public class DefaultRedisString : IRedisString
    {

        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisString(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }

        #region 同步
        /// <summary>
        /// 保存字符串
        /// </summary>
        public void Add(string key, string value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            redisBase.DoSave(db => db.StringSet(redisPrefixKey.StringPrefixKey + key, value, expiry, when, flags));
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Add<T>(string key, T value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = redisPrefixKey.StringPrefixKey + key;
            var res = redisBase.ConvertJson(value);
            redisBase.DoSave(db => db.StringSet(key, res, expiry, when, flags));
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool Add<T>(string key, List<T> value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (value == null || value.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = redisPrefixKey.StringPrefixKey + key;
            List<T> li = new List<T>();
            foreach (var item in value)
            {
                li.Add(item);
            }
            var res = redisBase.ConvertJson(li);
            return redisBase.DoSave(db => db.StringSet(key, res, expiry, when,flags));
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public string Get(string key, CommandFlags flags = CommandFlags.None)
        {
            return redisBase.DoSave(db => db.StringGet(redisPrefixKey.StringPrefixKey + key, flags));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.StringGet(redisPrefixKey.StringPrefixKey + key, flags)));
        }


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long Increment(string key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringIncrement(key, value, flags));
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Decrement(string key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringDecrement(key, value, flags));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 保存字符串
        /// </summary>
        public async Task<bool> AddAsync(string key, string value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(redisPrefixKey.StringPrefixKey + key, value, expiry, when, flags));
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(redisPrefixKey.StringPrefixKey + key, redisBase.ConvertJson(value), expiry, when, flags));
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> AddAsync<T>(string key, List<T> value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (value == null || value.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = redisPrefixKey.StringPrefixKey + key;
            List<T> li = new List<T>();
            foreach (var item in value)
            {
                li.Add(item);
            }
            return await redisBase.DoSave(db => db.StringSetAsync(key, redisBase.ConvertJson(li), expiry, when, flags));
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public async Task<string> GetAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            return await redisBase.DoSave(db => db.StringGetAsync(redisPrefixKey.StringPrefixKey + key, flags));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<T> GetAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            var value = await redisBase.DoSave(db => db.StringGetAsync(key, flags));
            if (value.ToString() == null)
            {
                return default(T);
            }
            return redisBase.ConvertObj<T>(value);
        }

        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<long> IncrementAsync(string key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringIncrementAsync(key, value, flags));
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> DecrementAsync(string key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringDecrementAsync(key, value, flags));
        }
        #endregion
    }
}
