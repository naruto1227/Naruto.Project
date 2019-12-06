using Fate.Infrastructure.Redis.IRedisManage;
using Fate.Infrastructure.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.RedisManage
{
    public class DefaultRedisList: IRedisList
    {

        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisList(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
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
                    redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, result));
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
            var vList = redisBase.DoSave(db => db.ListRange(redisPrefixKey.ListPrefixKey  + key));
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
            return redisBase.DoSave(db => db.ListRemove(redisPrefixKey.ListPrefixKey  + key, redisBase.ConvertJson(value)));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ListLeftPop(string key)
        {
            return redisBase.DoSave(db => db.ListLeftPop(redisPrefixKey.ListPrefixKey  + key));
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
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey  + key, value));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.ListLeftPop(redisPrefixKey.ListPrefixKey  + key)));
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
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey  + key, redisBase.ConvertJson(value)));
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
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey  + key, value.ToRedisValueArray()));
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
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey  + key, redisValues));
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return redisBase.DoSave(db => db.ListLength(redisPrefixKey.ListPrefixKey  + key));
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
                    await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey  + key, result));
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
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(redisPrefixKey.ListPrefixKey  + key));
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
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(redisPrefixKey.ListPrefixKey  + key));
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
            return await redisBase.DoSave(db => db.ListRemoveAsync(redisPrefixKey.ListPrefixKey  + key, redisBase.ConvertJson(value)));
        }


        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            return await redisBase.DoSave(db => db.ListLengthAsync(redisPrefixKey.ListPrefixKey  + key));
        }

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string key)
        {
            return await redisBase.DoSave(db => db.ListLeftPopAsync(redisPrefixKey.ListPrefixKey  + key));
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
            await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey  + key, value));
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            return redisBase.ConvertObj<T>((await redisBase.DoSave(db => db.ListLeftPopAsync(redisPrefixKey.ListPrefixKey  + key))));
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
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey  + key, redisBase.ConvertJson(value)));
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
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey  + key, redisValues.ToArray()));
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
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey  + key, value.ToRedisValueArray()));
        }
        #endregion

    }
}
