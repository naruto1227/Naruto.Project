using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.IRedisManage
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// 锁
    /// </summary>
    public interface IRedisLock : IRedisDependency
    {
        /// <summary>
        /// 将key锁住(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        bool Lock(string key, string value, TimeSpan expiry = default, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 锁(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<bool> LockAsync(string key, string value, TimeSpan expiry = default, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        bool Release(string key, string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(string key, string value, CommandFlags flags = CommandFlags.None);

    }
}
