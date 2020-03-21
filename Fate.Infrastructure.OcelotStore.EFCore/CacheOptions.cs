
using Fate.Infrastructure.Repository.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore
{
    public class CacheOptions
    {
        /// <summary>
        /// 存储的缓存中的key （默认为 ocelotef）
        /// </summary>
        internal string CacheKey { get; set; } = "ocelotef";

        /// <summary>
        /// ef的参数配置
        /// </summary>
        public Action<OcelotEFOption> EFOptions { get; set; } = null;
    }

    public class OcelotEFOption
    {
        /// <summary>
        /// 上下文的配置 (必填)
        /// </summary>
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
        /// <summary>
        /// 是否开启读写分离的操作 (默认不开启)
        /// </summary>
        public bool IsOpenMasterSlave { get; set; } = false;
        /// <summary>
        /// 只读的连接字符串的key 当IsOpenMasterSlave为true时 必须设置
        /// </summary>
        public string[] ReadOnlyConnectionString { get; set; }
    }
}
