using Naruto.Redis.IRedisManage;
using Naruto.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.RedisManage
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
        public void Add(string key, string value, TimeSpan? expiry = default)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            redisBase.DoSave(db => db.StringSet(redisPrefixKey.StringPrefixKey + key, value, expiry));
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Add<T>(string key, T value, TimeSpan? expiry = default)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = redisPrefixKey.StringPrefixKey + key;
            var res = redisBase.ConvertJson(value);
            redisBase.DoSave(db => db.StringSet(key, res, expiry));
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool Add<T>(string key, List<T> value, TimeSpan? expiry = default)
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
            return redisBase.DoSave(db => db.StringSet(key, res, expiry));
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public string Get(string key)
        {
            return redisBase.DoSave(db => db.StringGet(redisPrefixKey.StringPrefixKey + key));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>(string key)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.StringGet(redisPrefixKey.StringPrefixKey + key)));
        }


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long Increment(string key, long value = 1)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringIncrement(key, value));
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Decrement(string key, long value = 1)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringDecrement(key, value));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 保存字符串
        /// </summary>
        public async Task<bool> AddAsync(string key, string value, TimeSpan? expiry = default)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(redisPrefixKey.StringPrefixKey + key, value, expiry)).ConfigureAwait(false);
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan? expiry = default)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(redisPrefixKey.StringPrefixKey + key, redisBase.ConvertJson(value), expiry)).ConfigureAwait(false);
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> AddAsync<T>(string key, List<T> value, TimeSpan? expiry = default)
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
            return await redisBase.DoSave(db => db.StringSetAsync(key, redisBase.ConvertJson(li), expiry)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public async Task<string> GetAsync(string key)
        {
            return await redisBase.DoSave(db => db.StringGetAsync(redisPrefixKey.StringPrefixKey + key)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<T> GetAsync<T>(string key)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            var value = await redisBase.DoSave(db => db.StringGetAsync(key)).ConfigureAwait(false);
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
        public Task<long> IncrementAsync(string key, long value = 1)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringIncrementAsync(key, value));
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> DecrementAsync(string key, long value = 1)
        {
            key = redisPrefixKey.StringPrefixKey + key;
            return redisBase.DoSave(db => db.StringDecrementAsync(key, value));
        }
        #endregion
    }
}
