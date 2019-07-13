
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

        #region list
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
        long ListRemove<T>(string key, T value);

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ListLeftPop(string key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListRightPush(string key, string value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long ListRightPush(string key, string[] value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, List<T> value);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T ListLeftPop<T>(string key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, T value);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListLength(string key);

        #endregion

        #region string

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

        #endregion

        #region key

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

        #endregion

        #region SortedSet

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

        #endregion

        #region Set

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
        #endregion

        #region Store

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
        bool DeleteById<T>(string id);
        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteByIds<T>(List<string> ids);
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

        #region hash

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        bool HashDelete(string key, string hashField);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        long HashDelete(string key, string[] hashFields);
        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        bool HashExists(string key, string hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        string HashGet(string key, string hashField);

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        string[] HashGet(string key, string[] hashFields);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long HashLength(string key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        void HashSet(string key, HashEntry[] hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool HashSet(string key, string hashField, string value);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> HashGetAll(string key);

        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] HashValues(string key);

        #endregion

        #endregion

        #region 异步

        #region string

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

        #endregion

        ///// <summary>
        ///// 获取多个key的值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //Task<T> StringGetMultipleAsync<T>(string[] keys);
        #region list

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
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<List<string>> ListGetAsync(string key);


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
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> ListLeftPopAsync(string key);

        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task ListRightPushAsync(string key, string value);

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> ListLeftPopAsync<T>(string key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync<T>(string key, T value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string key, string[] value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync<T>(string key, List<T> value);
        #endregion

        #region key
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
        #endregion

        #region SortedSet

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

        #endregion

        #region set

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

        #region hash

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        Task<bool> HashDeleteAsync(string key, string hashField);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        Task<long> HashDeleteAsync(string key, string[] hashFields);
        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        Task<bool> HashExistsAsync(string key, string hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        Task<string> HashGetAsync(string key, string hashField);

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        Task<string[]> HashGetAsync(string key, string[] hashFields);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> HashGetAllAsync(string key);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> HashLengthAsync(string key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        Task HashSetAsync(string key, HashEntry[] hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> HashSetAsync(string key, string hashField, string value);
        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string[]> HashValuesAsync(string key);
        #endregion

        #endregion

        #region 发布订阅
        #region 同步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        void Subscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        long Publish(RedisChannel channel, RedisValue message = default);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        void Unsubscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null);
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        void UnsubscribeAll();
        #endregion
        #region 异步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        Task SubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        Task<long> PublishAsync(RedisChannel channel, RedisValue message = default);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        Task UnsubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null);

        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        Task UnsubscribeAllAsync();
        #endregion
        #endregion
    }
}
