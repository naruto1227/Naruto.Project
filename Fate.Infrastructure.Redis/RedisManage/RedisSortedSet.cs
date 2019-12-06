using Fate.Infrastructure.Redis.IRedisManage;
using Fate.Infrastructure.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.RedisManage
{
    public class RedisSortedSet : IRedisSortedSet
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisSortedSet(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return redisBase.DoSave(db => db.SortedSetAdd(redisPrefixKey.SortedSetKey + key, result, score));
        }
        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SortedSetGet<T>(string key, double score)
        {
            var result = redisBase.DoSave(db => db.SortedSetRangeByScore(redisPrefixKey.SortedSetKey + key, score));
            return redisBase.ConvertObj<T>(result.ToString());
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            return redisBase.DoSave(db => db.SortedSetLength(redisPrefixKey.SortedSetKey + key));
        }
        /// <summary>
        /// 移除SortedSet
        /// </summary>
        public bool SortedSetRemove<T>(string key, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return redisBase.DoSave(db => db.SortedSetRemove(redisPrefixKey.SortedSetKey + key, result));
        }
        #endregion

        #region 异步
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return await redisBase.DoSave(db => db.SortedSetAddAsync(redisPrefixKey.SortedSetKey + key, result, score));
        }
        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> SortedSetGetAsync<T>(string key, double score)
        {
            var result = await redisBase.DoSave(db => db.SortedSetRangeByScoreAsync(redisPrefixKey.SortedSetKey + key, score));
            return redisBase.ConvertObj<T>(result.ToString());
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            return await redisBase.DoSave(db => db.SortedSetLengthAsync(redisPrefixKey.SortedSetKey + key));
        }
        /// <summary>
        /// 移除SortedSet
        /// </summary>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return await redisBase.DoSave(db => db.SortedSetRemoveAsync(redisPrefixKey.SortedSetKey + key, result));
        }
        #endregion
    }
}
