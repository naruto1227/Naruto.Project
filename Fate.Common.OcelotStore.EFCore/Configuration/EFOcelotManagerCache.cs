using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Text;


namespace Fate.Common.OcelotStore.Redis
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// Ocelot 使用redis来操作缓存数据  z注册 singleton
    /// </summary>
    public class EFOcelotManagerCache<T> : IOcelotCache<T> where T : class
    {
        /// <summary>
        /// redis操作类
        /// </summary>
        private IRedisOperationHelp redis;

        /// <summary>
        /// 存储的缓存的前缀
        /// </summary>
        private readonly string prefix = "ocelotcache:";

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_redis"></param>
        public EFOcelotManagerCache(IRedisOperationHelp _redis)
        {
            redis = _redis;
        }

        /// <summary>
        /// 添加key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void Add(string key, T value, TimeSpan ttl, string region)
        {
            //验证时间是否正确
            if (ttl.TotalMilliseconds <= 0)
            {
                return;
            }
            //保存数据
            redis.StringSet<T>(prefix + key, value, ttl);
        }

        /// <summary>
        /// 更新key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
        {

            if (redis.KeyExists(prefix + key))
            {
                redis.KeyRemove(prefix + key);
            }

            Add(key, value, ttl, region);
        }

        public void ClearRegion(string region)
        {

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public T Get(string key, string region)
        {
            if (!redis.KeyExists(prefix + key))
            {
                return default;
            }
            return redis.StringGet<T>(prefix + key);
        }
    }
}
