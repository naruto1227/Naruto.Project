
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.IRedisManage
{
    /// <summary>
    /// redis 操作类
    /// </summary>
    public interface IRedisOperationHelp : IRedisDependency
    {
        /// <summary>
        /// hash操作
        /// </summary>
        /// <returns></returns>
        IRedisHash RedisHash();

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        IRedisKey RedisKey();

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        IRedisList RedisList();

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        IRedisSet RedisSet();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisSortedSet RedisSortedSet();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisStore RedisStore();
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        IRedisString RedisString();
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        IRedisSubscribe RedisSubscribe();

        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        IRedisLock RedisLock();
    }
}
