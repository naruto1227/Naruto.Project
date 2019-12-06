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
    /// set的操作
    /// </summary>
    public interface IRedisSet : IRedisDependency
    {
        #region 同步

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetAdd<T>(string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetRemove<T>(string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] SetGet<T>(CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] SetGet(string key, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetAdd(string key, string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetRemove(string key, string value, CommandFlags flags = CommandFlags.None);

        #endregion

        #region 异步

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetAddAsync<T>(string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetRemoveAsync<T>(string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> SetGetAsync<T>(CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> SetGetAsync(string key, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetAddAsync(string key, string value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetRemoveAsync(string key, string value, CommandFlags flags = CommandFlags.None);

        #endregion
    }

}
