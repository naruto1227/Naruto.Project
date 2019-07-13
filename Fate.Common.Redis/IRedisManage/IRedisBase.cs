using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Redis.IRedisManage
{
    /// <summary>
    /// 缓存的基类 
    /// </summary>
    public interface IRedisBase : IRedisDependency
    {
        /// <summary>
        /// 访问存储库
        /// </summary>
        IDatabase redisDataBase { get; }
        /// <summary>
        /// 实例连接
        /// </summary>
        ConnectionMultiplexer RedisConnection { get; }
        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        TResult DoSave<TResult>(Func<IDatabase, TResult> action);
        /// <summary>
        /// 保存 无返回值的
        /// </summary>
        /// <param name="action"></param>
        void DoSave(Action<IDatabase> action);
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        string ConvertJson<T>(T val);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        T ConvertObj<T>(RedisValue val);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        List<T> ConvertList<T>(RedisValue[] val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        RedisKey[] ConvertRedisKeys(List<string> val);
    }
}
