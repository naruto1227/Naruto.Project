using Fate.Infrastructure.Redis.IRedisManage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Redis.RedisConfig
{
    /// <summary>
    /// redis的连接配置
    /// </summary>
    public interface IRedisConnectionHelp : IRedisDependency
    {
        /// <summary>
        /// redis 密码
        /// </summary>
        string RedisPassword { get; }
        /// <summary>
        /// redis的默认存储库
        /// </summary>

        int RedisDefaultDataBase { get; }
        /// <summary>
        /// redis 连接
        /// </summary>

        string RedisConnectionConfig { get; }
        /// <summary>
        /// 默认超时时间
        /// </summary>
        int DefaultConnectTimeout { get; }
        /// <summary>
        /// 默认异步超时时间
        /// </summary>
        int DefaultAsyncTimeout { get; }
        /// <summary>
        /// 连接实例
        /// </summary>
        ConnectionMultiplexer RedisConnection { get; }
    }
}
