using Fate.Common.Enum;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Redis.IRedisManage
{
    /// <summary>
    /// redis 操作类
    /// </summary>
    public interface IRedisOperationHelp : IRedisDependency
    {
        #region 同步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListSet<T>(string key, List<T> value);
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        List<T> ListGet<T>(string key);

        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        void ListRemove<T>(string key, T value);


        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListLength(string key);

        /// <summary>
        /// 保存字符串
        /// </summary>
        void StringSet(string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void StringSet<T>(string key, T value, TimeSpan? expiry = default);

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool StringSet<T>(string key, List<T> value, TimeSpan? expiry = default);

        /// <summary>
        /// 获取字符串
        /// </summary>
        string StringGet(string key);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T StringGet<T>(string key);

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        void KeyRemove(string key, KeyOperatorEnum keyOperatorEnum = default);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        bool KeyExists(string key, KeyOperatorEnum keyOperatorEnum = default);

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        bool SortedSetAdd<T>(string key, T value, double score);

        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T SortedSetGet<T>(string key, double score);

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long SortedSetLength(string key);

        /// <summary>
        /// 移除SortedSet
        /// </summary>
        bool SortedSetRemove<T>(string key, T value);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetAdd<T>(string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetRemove<T>(string value);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] SetGet<T>();

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        string[] SetGet(string key);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetAdd(string key, string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetRemove(string key, string value);

        /// <summary>
        /// 保存一个集合 （事务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        bool StoreAll<T>(List<T> list);

        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        bool Store<T>(T info);
        /// <summary>
        /// 删除所有的
        /// </summary>
        bool DeleteAll<T>();

        /// <summary>
        /// 移除 单个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteById<T>(int id);
        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteByIds<T>(List<int> ids);
        /// <summary>
        /// 获取所有的集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAll<T>();

        /// <summary>
        /// 获取单个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(int id);
        /// <summary>
        /// 获取多个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        List<T> GetByIds<T>(List<int> ids);
        #endregion

        #region 异步
        /// <summary>
        /// 保存字符串
        /// </summary>
        Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?));
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));
        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> StringSetAsync<T>(string key, List<T> value, TimeSpan? expiry = default(TimeSpan?));
        /// <summary>
        /// 获取字符串
        /// </summary>
        Task<RedisValue> StringGetAsync(string key);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<T> StringGetAsync<T>(string key);

        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task ListSetAsync<T>(string key, List<T> value);

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<List<T>> ListGetAsync<T>(string key);
        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        Task<long> ListRemoveAsync<T>(string key, T value);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string key);
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        Task<bool> KeyRemoveAsync(string key, KeyOperatorEnum keyOperatorEnum = default);

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        Task<bool> KeyExistsAsync(string key, KeyOperatorEnum keyOperatorEnum = default);

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task<bool> SortedSetAddAsync<T>(string key, T value, double score);

        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> SortedSetGetAsync<T>(string key, double score);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> SortedSetLengthAsync(string key);

        /// <summary>
        /// 移除SortedSet
        /// </summary>
        Task<bool> SortedSetRemoveAsync<T>(string key, T value);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetAddAsync<T>(string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetRemoveAsync<T>(string value);

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> SetGetAsync<T>();

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<string[]> SetGetAsync(string key);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetAddAsync(string key, string value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> SetRemoveAsync(string key, string value);
        #endregion
    }
}
