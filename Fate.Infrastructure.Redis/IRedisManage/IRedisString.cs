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
    /// redis的字符串操作
    /// </summary>
    public interface IRedisString : IRedisDependency
    {
        #region 同步

        /// <summary>
        /// 保存字符串
        /// </summary>
        void Set(string key, string value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, T value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool Set<T>(string key, List<T> value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 获取字符串
        /// </summary>
        string Get(string key, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Get<T>(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        long Increment(string key, long value = 1, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long Decrement(string key, long value = 1, CommandFlags flags = CommandFlags.None);
        #endregion


        #region 异步

        /// <summary>
        /// 保存字符串
        /// </summary>
        Task<bool> SetAsync(string key, string value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> SetAsync<T>(string key, List<T> value, TimeSpan? expiry = default, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 获取字符串
        /// </summary>
        Task<string> GetAsync(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<T> GetAsync<T>(string key, CommandFlags flags = CommandFlags.None);


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<long> IncrementAsync(string key, long value = 1, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> DecrementAsync(string key, long value = 1, CommandFlags flags = CommandFlags.None);
        #endregion
    }
}
