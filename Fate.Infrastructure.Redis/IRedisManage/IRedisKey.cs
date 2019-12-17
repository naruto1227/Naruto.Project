using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.IRedisManage
{
    /// <summary>
    /// key的操作
    /// </summary>
    public interface IRedisKey : IRedisDependency
    {
        #region 同步

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key, KeyOperatorEnum keyOperatorEnum = default);
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        void Remove(List<string> key, KeyOperatorEnum keyOperatorEnum = default);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        bool Exists(string key, KeyOperatorEnum keyOperatorEnum = default);

        #endregion

        #region 异步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        Task<bool> RemoveAsync(string key, KeyOperatorEnum keyOperatorEnum = default);
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        Task<long> RemoveAsync(List<string> key, KeyOperatorEnum keyOperatorEnum = default);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        Task<bool> ExistsAsync(string key, KeyOperatorEnum keyOperatorEnum = default);
        #endregion
    }
}
