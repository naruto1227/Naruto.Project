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
        bool Add<T>(string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool Remove<T>(string value);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] Get<T>();

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] Get(string key);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool Add(string key, string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Remove(string key, string value);

        #endregion

        #region 异步

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> AddAsync<T>(string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> RemoveAsync<T>(string value);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> GetAsync<T>();

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> GetAsync(string key);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> AddAsync(string key, string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> RemoveAsync(string key, string value);

        #endregion
    }

}
