using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Redis.RedisConfig
{
    /// <summary>
    /// redis的key的前缀
    /// </summary>
    public class RedisPrefixKey
    {
        /// <summary>
        /// string类型的前缀 (默认string:)
        /// </summary>
        public string StringPrefixKey { get; set; } = "string:";
        /// <summary>
        /// List 类型的前缀  (默认list:)
        /// </summary>
        public string ListPrefixKey { get; set; } = "list:";
        /// <summary>
        /// Set类型的前缀  (默认ids:)
        /// </summary>
        public string SetPrefixKey { get; set; } = "ids:";
        /// <summary>
        /// Hash 类型的前缀  (默认hash:)
        /// </summary>
        public string HashPrefixKey { get; set; } = "hash:";
        /// <summary>
        /// 有序集合的前缀  (默认sortedset:)
        /// </summary>
        public string SortedSetKey { get; set; } = "sortedset:";
    }
}
