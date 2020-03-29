using Naruto.Redis.IRedisManage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.RedisManage
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultRedisLock : IRedisLock
    {
        /// <summary>
        /// 锁的前缀
        /// </summary>
        private readonly string LockPrefix = "lock:";

        private readonly IRedisBase redisBase;

        public DefaultRedisLock(IRedisBase _redisBase)
        {
            redisBase = _redisBase;
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(string key, string value, TimeSpan expiry = default)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTake(LockPrefix + key, value, expiry);
            });
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(string key, string value, TimeSpan expiry)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTakeAsync(LockPrefix + key, value, expiry);
            });
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Release(string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockRelease(LockPrefix + key, value);
            });
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockReleaseAsync(LockPrefix + key, value);
            });
        }
    }
}
