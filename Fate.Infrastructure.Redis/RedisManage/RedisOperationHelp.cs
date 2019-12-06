using System;

using Fate.Infrastructure.Redis.IRedisManage;
using Microsoft.Extensions.DependencyInjection;


namespace Fate.Infrastructure.Redis.RedisManage
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis操作封装类
    /// </summary>
    public class RedisOperationHelp : IRedisOperationHelp
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisOperationHelp(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// hash操作
        /// </summary>
        /// <returns></returns>
        public IRedisHash RedisHash()
        {
            return serviceProvider.GetRequiredService<IRedisHash>();
        }

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        public IRedisKey RedisKey()
        {
            return serviceProvider.GetRequiredService<IRedisKey>();
        }

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        public IRedisList RedisList()
        {
            return serviceProvider.GetRequiredService<IRedisList>();
        }

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        public IRedisSet RedisSet()
        {
            return serviceProvider.GetRequiredService<IRedisSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisSortedSet RedisSortedSet()
        {
            return serviceProvider.GetRequiredService<IRedisSortedSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisStore RedisStore()
        {
            return serviceProvider.GetRequiredService<IRedisStore>();
        }
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        public IRedisString RedisString()
        {
            return serviceProvider.GetRequiredService<IRedisString>();
        }
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        public IRedisSubscribe RedisSubscribe()
        {
            return serviceProvider.GetRequiredService<IRedisSubscribe>();
        }
        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        public IRedisLock RedisLock()
        {
            return serviceProvider.GetRequiredService<IRedisLock>();
        }
    }
}
