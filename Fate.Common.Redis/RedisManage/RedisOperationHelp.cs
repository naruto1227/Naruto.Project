using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using Fate.Common.Redis.IRedisManage;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Fate.Common.Redis.RedisConfig;
using System.Linq.Expressions;

namespace Fate.Common.Redis.RedisManage
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis操作封装类
    /// </summary>
    public class RedisOperationHelp : IRedisOperationHelp
    {
        private IRedisBase redisBase;
        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisOperationHelp(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            StringSysCustomKey = options.Value.RedisPrefix?.StringPrefixKey;
            ListSysCustomKey = options.Value.RedisPrefix?.ListPrefixKey;
            SetSysCustomKey = options.Value.RedisPrefix?.SetPrefixKey;
            HashSysCustomKey = options.Value.RedisPrefix?.HashPrefixKey;
            SortedSetCustomKey = options.Value.RedisPrefix?.SortedSetKey;
        }
        /// <summary>
        /// string的缓存前缀
        /// </summary>
        private readonly string StringSysCustomKey;
        /// <summary>
        /// list的前缀
        /// </summary>
        private readonly string ListSysCustomKey;

        /// <summary>
        /// set的前缀
        /// </summary>
        private readonly string SetSysCustomKey;
        /// <summary>
        /// hash的前缀
        /// </summary>
        private readonly string HashSysCustomKey;
        /// <summary>
        /// SortedSet 有序集合的前缀
        /// </summary>
        private readonly string SortedSetCustomKey;

        /// <summary>
        /// Store的前缀
        /// </summary>
        private readonly string StoreSysCustomKey = "urn:";

        #region list
        #region 同步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListSet<T>(string key, List<T> value)
        {
            if (value != null && value.Count > 0)
            {
                foreach (var single in value)
                {
                    var result = redisBase.ConvertJson(single);
                    redisBase.DoSave(db => db.ListRightPush(ListSysCustomKey + key, result));
                }
            }
        }
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public List<T> ListGet<T>(string key)
        {
            var vList = redisBase.DoSave(db => db.ListRange(ListSysCustomKey + key));
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = redisBase.ConvertObj<T>(item); //反序列化
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public long ListRemove<T>(string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRemove(ListSysCustomKey + key, redisBase.ConvertJson(value)));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ListLeftPop(string key)
        {
            return redisBase.DoSave(db => db.ListLeftPop(ListSysCustomKey + key));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListRightPush(string key, string value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(ListSysCustomKey + key, value));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.ListLeftPop(ListSysCustomKey + key)));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(ListSysCustomKey + key, redisBase.ConvertJson(value)));
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush(string key, string[] value)
        {
            if (value == null || value.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(ListSysCustomKey + key, value.ToRedisValueArray()));
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, List<T> value)
        {
            if (value == null || value.Count <= 0)
                throw new ApplicationException("值不能为空");
            RedisValue[] redisValues = new RedisValue[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                redisValues[i] = redisBase.ConvertJson(value[i]);
            }
            return redisBase.DoSave(db => db.ListRightPush(ListSysCustomKey + key, redisValues));
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return redisBase.DoSave(db => db.ListLength(ListSysCustomKey + key));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task ListSetAsync<T>(string key, List<T> value)
        {
            if (value != null && value.Count > 0)
            {
                foreach (var single in value)
                {
                    var result = redisBase.ConvertJson(single);
                    await redisBase.DoSave(db => db.ListRightPushAsync(ListSysCustomKey + key, result));
                }
            }
        }
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<T>> ListGetAsync<T>(string key)
        {
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(ListSysCustomKey + key));
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = redisBase.ConvertObj<T>(item); //反序列化
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<string>> ListGetAsync(string key)
        {
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(ListSysCustomKey + key));
            return vList.ToStringArray().ToList();
        }

        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRemoveAsync(ListSysCustomKey + key, redisBase.ConvertJson(value)));
        }


        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            return await redisBase.DoSave(db => db.ListLengthAsync(ListSysCustomKey + key));
        }

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string key)
        {
            return await redisBase.DoSave(db => db.ListLeftPopAsync(ListSysCustomKey + key));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task ListRightPushAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ApplicationException("值不能为空");
            await redisBase.DoSave(db => db.ListRightPushAsync(ListSysCustomKey + key, value));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            return redisBase.ConvertObj<T>((await redisBase.DoSave(db => db.ListLeftPopAsync(ListSysCustomKey + key))));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRightPushAsync(ListSysCustomKey + key, redisBase.ConvertJson(value)));
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync<T>(string key, List<T> value)
        {
            if (value == null || value.Count <= 0)
                throw new ApplicationException("值不能为空");
            List<RedisValue> redisValues = new List<RedisValue>();
            value.ForEach(item =>
            {
                redisValues.Add(redisBase.ConvertJson(item));
            });
            return await redisBase.DoSave(db => db.ListRightPushAsync(ListSysCustomKey + key, redisValues.ToArray()));
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string key, string[] value)
        {
            if (value == null || value.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRightPushAsync(ListSysCustomKey + key, value.ToRedisValueArray()));
        }
        #endregion

        #endregion

        #region string
        #region 同步
        /// <summary>
        /// 保存字符串
        /// </summary>
        public void StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            redisBase.DoSave(db => db.StringSet(StringSysCustomKey + key, value, expiry));
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void StringSet<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = StringSysCustomKey + key;
            var res = redisBase.ConvertJson(value);
            redisBase.DoSave(db => db.StringSet(key, res, expiry));
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool StringSet<T>(string key, List<T> value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (value == null || value.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = StringSysCustomKey + key;
            List<T> li = new List<T>();
            foreach (var item in value)
            {
                li.Add(item);
            }
            var res = redisBase.ConvertJson(li);
            return redisBase.DoSave(db => db.StringSet(key, res, expiry));
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public string StringGet(string key)
        {
            return redisBase.DoSave(db => db.StringGet(StringSysCustomKey + key));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T StringGet<T>(string key)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.StringGet(StringSysCustomKey + key)));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 保存字符串
        /// </summary>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(StringSysCustomKey + key, value, expiry));
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.StringSetAsync(StringSysCustomKey + key, redisBase.ConvertJson(value), expiry));
        }

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<bool> StringSetAsync<T>(string key, List<T> value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (value == null || value.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
            key = StringSysCustomKey + key;
            List<T> li = new List<T>();
            foreach (var item in value)
            {
                li.Add(item);
            }
            return await redisBase.DoSave(db => db.StringSetAsync(key, redisBase.ConvertJson(li), expiry));
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public async Task<RedisValue> StringGetAsync(string key)
        {
            return await redisBase.DoSave(db => db.StringGetAsync(StringSysCustomKey + key));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<T> StringGetAsync<T>(string key)
        {
            key = StringSysCustomKey + key;
            var value = await redisBase.DoSave(db => db.StringGetAsync(key));
            if (value.ToString() == null)
            {
                return default(T);
            }
            return redisBase.ConvertObj<T>(value);
        }

        ///// <summary>
        ///// 获取多个key的值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public async Task<T> StringGetMultipleAsync<T>(string[] keys) where T : class, new()
        //{
        //    if (keys == null)
        //        throw new ApplicationException("参数不能为空");
        //    T li = new T();
        //    foreach (var item in keys)
        //    {
        //        var key = StringSysCustomKey + item;
        //        var value = await redisBase.DoSave(db => db.StringGetAsync(key));
        //        if (value.ToString() != null)
        //        {
        //            li(redisBase.ConvertObj<T>(value));
        //        }
        //    }
        //}
        #endregion
        #endregion

        #region key
        #region 同步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void KeyRemove(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = StringSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = ListSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = SetSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = HashSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = SortedSetCustomKey + key;
            }
            redisBase.DoSave(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool KeyExists(string key, KeyOperatorEnum keyOperatorEnum = default)
        {

            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = StringSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = ListSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = SetSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = HashSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = SortedSetCustomKey + key;
            }
            return redisBase.DoSave(db => db.KeyExists(key));
        }
        #endregion

        #region 异步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> KeyRemoveAsync(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = StringSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = ListSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = SetSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = HashSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = SortedSetCustomKey + key;
            }
            return await redisBase.DoSave(db => db.KeyDeleteAsync(key));
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> KeyExistsAsync(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = StringSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = ListSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = SetSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = HashSysCustomKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = SortedSetCustomKey + key;
            }
            return await redisBase.DoSave(db => db.KeyExistsAsync(key));
        }
        #endregion
        #endregion

        #region SortedSet
        #region 同步
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return redisBase.DoSave(db => db.SortedSetAdd(SortedSetCustomKey + key, result, score));
        }
        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SortedSetGet<T>(string key, double score)
        {
            var result = redisBase.DoSave(db => db.SortedSetRangeByScore(SortedSetCustomKey + key, score));
            return redisBase.ConvertObj<T>(result.ToString());
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            return redisBase.DoSave(db => db.SortedSetLength(SortedSetCustomKey + key));
        }
        /// <summary>
        /// 移除SortedSet
        /// </summary>
        public bool SortedSetRemove<T>(string key, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return redisBase.DoSave(db => db.SortedSetRemove(SortedSetCustomKey + key, result));
        }
        #endregion

        #region 异步
        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return await redisBase.DoSave(db => db.SortedSetAddAsync(SortedSetCustomKey + key, result, score));
        }
        /// <summary>
        /// 获取SortedSet的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> SortedSetGetAsync<T>(string key, double score)
        {
            var result = await redisBase.DoSave(db => db.SortedSetRangeByScoreAsync(SortedSetCustomKey + key, score));
            return redisBase.ConvertObj<T>(result.ToString());
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            return await redisBase.DoSave(db => db.SortedSetLengthAsync(SortedSetCustomKey + key));
        }
        /// <summary>
        /// 移除SortedSet
        /// </summary>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var result = redisBase.ConvertJson(value);
            return await redisBase.DoSave(db => db.SortedSetRemoveAsync(SortedSetCustomKey + key, result));
        }
        #endregion
        #endregion

        #region Set
        #region 同步
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SetAdd<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return redisBase.DoSave(db => db.SetAdd(key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SetRemove<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return redisBase.DoSave(db => db.SetRemove(key, value));
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public string[] SetGet<T>()
        {
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return redisBase.DoSave(db => db.SetMembers(key)).ToStringArray();
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public string[] SetGet(string key)
        {
            return redisBase.DoSave(db => db.SetMembers(SetSysCustomKey + key)).ToStringArray();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SetAdd(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return redisBase.DoSave(db => db.SetAdd(SetSysCustomKey + key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetRemove(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            redisBase.DoSave(db => db.SetRemove(SetSysCustomKey + key, value));
        }
        #endregion
        #region 异步
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SetAddAsync<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return await redisBase.DoSave(db => db.SetAddAsync(key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SetRemoveAsync<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return await redisBase.DoSave(db => db.SetRemoveAsync(key, value));
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<string[]> SetGetAsync<T>()
        {
            //反射实体的信息
            var type = typeof(T);
            string key = SetSysCustomKey + type.Name;
            return (await redisBase.DoSave(db => db.SetMembersAsync(key))).ToStringArray();
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<string[]> SetGetAsync(string key)
        {
            return (await redisBase.DoSave(db => db.SetMembersAsync(SetSysCustomKey + key))).ToStringArray();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SetAddAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.SetAddAsync(SetSysCustomKey + key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SetRemoveAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.SetRemoveAsync(SetSysCustomKey + key, value));
        }
        #endregion
        #endregion

        #region Store
        /// <summary>
        /// 保存一个集合 （事务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool StoreAll<T>(List<T> list)
        {
            if (list != null && list.Count >= 0)
            {
                //获取实体的信息
                var type = typeof(T);
                //获取类名
                var name = type.Name;
                string key = StoreSysCustomKey + name.ToLower() + ":";
                //获取id的属性
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Id");
                var tran = redisBase.DoSave(db => db.CreateTransaction());
                foreach (var item in list)
                {
                    //获取id的值
                    var id = propertyInfo.GetValue(item, null);
                    tran.SetAddAsync(SetSysCustomKey + type.Name, id.ToString());
                    tran.StringSetAsync(key + id, redisBase.ConvertJson(item));
                }
                return tran.Execute();
            }
            return false;
        }
        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool Store<T>(T info)
        {
            if (info == null)
            {
                return false;
            }
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            //获取id的属性
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Id");
            //获取id的值
            var id = propertyInfo.GetValue(info,null);
            //开启事务
            var tran = redisBase.DoSave(db => db.CreateTransaction());
            tran.SetAddAsync(SetSysCustomKey + type.Name, id.ToString());
            tran.StringSetAsync(key + id.ToString(), redisBase.ConvertJson(info));
            return tran.Execute();
        }
        ///// <summary>
        ///// 删除所有的
        ///// </summary>
        //public void DeleteAll<T>()
        //{
        //    //获取实体的信息
        //    var type = typeof(T);
        //    //获取类名
        //    var name = type.Name;
        //    string key = StoreSysCustomKey + name.ToLower() + ":";

        //    //获取需要删除的id
        //     var ids= SetGet<T>();
        //    if (redis.KeyDelete(SetSysCustomKey + type.Name))
        //    {
        //        foreach (var item in ids)
        //        {
        //            redis.KeyDelete(key+item.ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// 删除所有的
        /// </summary>
        public bool DeleteAll<T>()
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";

            var tran = redisBase.DoSave(db => db.CreateTransaction());
            //获取需要删除的id
            var ids = SetGet<T>();
            tran.KeyDeleteAsync(SetSysCustomKey + type.Name);
            foreach (var item in ids)
            {
                tran.KeyDeleteAsync(key + item.ToString());
            }
            return tran.Execute();
        }
        /// <summary>
        /// 移除 单个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteById<T>(string id)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            var tran = redisBase.DoSave(db => db.CreateTransaction());
            tran.SetRemoveAsync(SetSysCustomKey + type.Name, id);
            tran.KeyDeleteAsync(key + id.ToString());
            return tran.Execute();
        }

        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteByIds<T>(List<string> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                //获取实体的信息
                var type = typeof(T);
                //获取类名
                var name = type.Name;
                string key = StoreSysCustomKey + name.ToLower() + ":";
                var tran = redisBase.DoSave(db => db.CreateTransaction());
                foreach (var item in ids)
                {
                    tran.SetRemoveAsync(SetSysCustomKey + type.Name, item);
                    tran.KeyDeleteAsync(key + item.ToString());
                }
                return tran.Execute();
            }
            return false;

        }
        /// <summary>
        /// 获取所有的集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>()
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";

            List<T> li = new List<T>();
            //获取id的集合
            var ids = SetGet<T>();
            if (ids != null && ids.Length > 0)
            {
                foreach (var item in ids)
                {
                    var res = redisBase.DoSave(db => db.StringGet(key + item));
                    if (!res.IsNullOrEmpty)
                    {
                        li.Add(redisBase.ConvertObj<T>(res));
                    }
                }
            }
            return li;
        }

        /// <summary>
        /// 获取单个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int id)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            var res = redisBase.DoSave(db => db.StringGet(key + id.ToString()));
            if (!res.IsNullOrEmpty)
            {
                return redisBase.ConvertObj<T>(res);
            }
            return default(T);
        }

        /// <summary>
        /// 获取多个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetByIds<T>(List<int> ids)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            List<T> li = new List<T>();
            foreach (var item in ids)
            {
                var res = redisBase.DoSave(db => db.StringGet(key + item.ToString()));
                if (!res.IsNullOrEmpty)
                {
                    li.Add(redisBase.ConvertObj<T>(res));
                }
            }
            return li;
        }

        ///// <summary>
        ///// 获取字段的值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private object FuncPropertyId<T>(T value)
        //{
        //    return Expression.Lambda<Func<object>>(Expression.Convert(Expression.PropertyOrField(Expression.Constant(value), "Id"), typeof(object))).Compile()();
        //}
        #endregion

        #region hash

        #region 同步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public bool HashDelete(string key, string hashField)
        {
            return redisBase.DoSave(db => db.HashDelete(HashSysCustomKey + key, hashField));
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public long HashDelete(string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashDelete(HashSysCustomKey + key, hashFields.ToRedisValueArray()));
        }

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashExists(string key, string hashField) => redisBase.DoSave(db => db.HashExists(HashSysCustomKey + key, hashField));
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public string HashGet(string key, string hashField)
        {
            var res = redisBase.DoSave(db => db.HashGet(HashSysCustomKey + key, hashField));
            return !res.IsNull ? res.ToString() : default;
        }
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> HashGetAll(string key)
        {
            var res = redisBase.DoSave(db => db.HashGetAll(HashSysCustomKey + key));
            return res != null ? res.ToStringDictionary() : default;
        }
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public string[] HashGet(string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            var res = redisBase.DoSave(db => db.HashGet(HashSysCustomKey + key, hashFields.ToRedisValueArray()));
            return res != null ? res.ToStringArray() : default;
        }
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HashLength(string key) => redisBase.DoSave(db => db.HashLength(HashSysCustomKey + key));

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public void HashSet(string key, HashEntry[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            redisBase.DoSave(db => db.HashSet(HashSysCustomKey + key, hashFields));
        }

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool HashSet(string key, string hashField, string value)
        {
            return redisBase.DoSave(db => db.HashSet(HashSysCustomKey + key, hashField, value));
        }
        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HashValues(string key)
        {
            var res = redisBase.DoSave(db => db.HashValues(HashSysCustomKey + key));
            return res != null ? res.ToStringArray() : default;
        }
        #endregion

        #region 异步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public Task<bool> HashDeleteAsync(string key, string hashField)
            => redisBase.DoSave(db => db.HashDeleteAsync(HashSysCustomKey + key, hashField));

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public Task<long> HashDeleteAsync(string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashDeleteAsync(HashSysCustomKey + key, hashFields.ToRedisValueArray()));
        }

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public Task<bool> HashExistsAsync(string key, string hashField) => redisBase.DoSave(db => db.HashExistsAsync(HashSysCustomKey + key, hashField));
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<string> HashGetAsync(string key, string hashField)
        {
            var res = await redisBase.DoSave(db => db.HashGetAsync(HashSysCustomKey + key, hashField));
            return !res.IsNull ? res.ToString() : default;
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> HashGetAllAsync(string key)
        {
            var res = await redisBase.DoSave(db => db.HashGetAllAsync(HashSysCustomKey + key));
            return res != null ? res.ToStringDictionary() : default;
        }
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public async Task<string[]> HashGetAsync(string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            var res = await redisBase.DoSave(db => db.HashGetAsync(HashSysCustomKey + key, hashFields.ToRedisValueArray()));
            return res != null ? res.ToStringArray() : default;
        }
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<long> HashLengthAsync(string key) => redisBase.DoSave(db => db.HashLengthAsync(HashSysCustomKey + key));

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public Task HashSetAsync(string key, HashEntry[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashSetAsync(HashSysCustomKey + key, hashFields));
        }

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<bool> HashSetAsync(string key, string hashField, string value)
        {
            return redisBase.DoSave(db => db.HashSetAsync(HashSysCustomKey + key, hashField, value));
        }

        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> HashValuesAsync(string key)
        {
            var res = await redisBase.DoSave(db => db.HashValuesAsync(HashSysCustomKey + key));
            return res != null ? res.ToStringArray() : default;
        }
        #endregion

        #endregion

        #region 发布订阅
        #region 同步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public void Subscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.Subscribe(chanel, handler);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        public long Publish(RedisChannel channel, RedisValue message)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            return subscriber.Publish(channel, message);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public void Unsubscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.Unsubscribe(chanel, handler);
        }
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.UnsubscribeAll();
        }
        #endregion
        #region 异步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public async Task SubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.SubscribeAsync(chanel, handler);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        public async Task<long> PublishAsync(RedisChannel channel, RedisValue message)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            return await subscriber.PublishAsync(channel, message);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public async Task UnsubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.UnsubscribeAsync(chanel, handler);
        }
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        public async Task UnsubscribeAllAsync()
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.UnsubscribeAllAsync();
        }
        #endregion
        #endregion

    }
}
