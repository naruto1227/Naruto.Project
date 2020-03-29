using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.IRedisManage
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// SortedSet操作
    /// </summary>
    public interface IRedisSortedSet : IRedisDependency
    {

        #region 同步

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        bool Add<T>(string key, T value, double score);

        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key, double score, Order order = Order.Ascending, long skip = 0, long take = -1);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Length(string key);

        /// <summary>
        /// 移除SortedSet
        /// </summary>
        bool Remove<T>(string key, T value);

        #endregion

        #region 异步

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task<bool> AddAsync<T>(string key, T value, double score);

        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, double score, Order order = Order.Ascending, long skip = 0, long take = -1);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> LengthAsync(string key);

        /// <summary>
        /// 移除SortedSet
        /// </summary>
        Task<bool> RemoveAsync<T>(string key, T value);

        #endregion
    }
}
