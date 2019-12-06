using Fate.Infrastructure.Redis.IRedisManage;
using Fate.Infrastructure.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.RedisManage
{
    public class RedisKey : IRedisKey
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisKey(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void KeyRemove(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
                key = redisPrefixKey.StringPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.List)
                key = redisPrefixKey.ListPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
                key = redisPrefixKey.SetPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
                key = redisPrefixKey.HashPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
                key = redisPrefixKey.SortedSetKey + key;
            redisBase.DoSave(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void KeyRemove(List<string> key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (key == null || key.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            List<string> removeList = new List<string>();
            key.ForEach(item =>
            {
                item = GetKey(item, keyOperatorEnum);
                removeList.Add(item);
            });
            redisBase.DoSave(db => db.KeyDelete(redisBase.ConvertRedisKeys(removeList)));
        }
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool KeyExists(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return redisBase.DoSave(db => db.KeyExists(key));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> KeyRemoveAsync(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return await redisBase.DoSave(db => db.KeyDeleteAsync(key));
        }
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> KeyRemoveAsync(List<string> key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (key == null || key.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            List<string> removeList = new List<string>();

            key.ForEach(item =>
            {
                item = GetKey(item, keyOperatorEnum);
                removeList.Add(item);
            });
            return await redisBase.DoSave(db => db.KeyDeleteAsync(redisBase.ConvertRedisKeys(removeList)));
        }
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> KeyExistsAsync(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return await redisBase.DoSave(db => db.KeyExistsAsync(key));
        }
        #endregion

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyOperatorEnum"></param>
        /// <returns></returns>
        private string GetKey(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = redisPrefixKey.StringPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = redisPrefixKey.ListPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = redisPrefixKey.SetPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = redisPrefixKey.HashPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = redisPrefixKey.SortedSetKey + key;
            }
            return key;
        }

    }
}
