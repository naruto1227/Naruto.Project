using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Redis.RedisConfig
{
    public class RedisOptions
    {
        /// <summary>
        /// redis的key的前缀配置
        /// </summary>
        public RedisPrefixKey RedisPrefix { get; set; } = new RedisPrefixKey();
        /// <summary>
        /// redis的连接地址
        /// </summary>
        public string[] Connection { get; set; }

        /// <summary>
        /// Redis的默认存储库
        /// </summary>
        public int DefaultDataBase { get; set; } = 0;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否开启哨兵
        /// </summary>
        public int IsOpenSentinel { get; set; } = 0;
        /// <summary>
        /// 哨兵的地址 
        /// </summary>
        public string[] RedisSentinelIp { get; set; }

        /// <summary>
        /// 连接超时时间 单位毫秒 默认 300ms
        /// </summary>
        public int ConnectTimeout { get; set; } = 300;

        /// <summary>
        /// 异步超时时间 单位毫秒 默认5s(指定系统允许异步操作的时间(默认为SyncTimeout)，以毫秒为单位)
        /// 超时时提示Timeout awaiting response
        /// </summary>

        public int AsyncTimeout { get; set; } = 5000;

    }
}
