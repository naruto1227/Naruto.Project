using Naruto.Redis.RedisConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.OcelotStore.Redis
{
    public class CacheOptions
    {
        /// <summary>
        /// 存储的redis中的key （默认为 ocelot）
        /// </summary>
        public string CacheKey { get; set; } = "ocelot";

        /// <summary>
        /// redis的参数配置
        /// </summary>
        public Action<RedisOptions> RedisOptions { get; set; } = null;
    }
}
