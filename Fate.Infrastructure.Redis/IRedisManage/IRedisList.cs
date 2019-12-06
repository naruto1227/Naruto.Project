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
    /// list的操作
    /// </summary>
    public interface IRedisList : IRedisDependency
    {
        #region 同步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListSet<T>(string key, List<T> value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        List<T> ListGet<T>(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        long ListRemove<T>(string key, T value, long count = 0, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ListLeftPop(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListRightPush(string key, string value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long ListRightPush(string key, string[] value, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, List<T> value, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T ListLeftPop<T>(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListLength(string key, CommandFlags flags = CommandFlags.None);

        #endregion
        #region 异步

        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task ListSetAsync<T>(string key, List<T> value, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<List<T>> ListGetAsync<T>(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<List<string>> ListGetAsync(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None);


        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        Task<long> ListRemoveAsync<T>(string key, T value, long count = 0, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> ListLeftPopAsync(string key, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task ListRightPushAsync(string key, string value, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> ListLeftPopAsync<T>(string key, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string key, string[] value, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync<T>(string key, List<T> value, CommandFlags flags = CommandFlags.None);
        #endregion
    }
}
