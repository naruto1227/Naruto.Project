using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Repository.Base
{
    public class EFOptions
    {
        /// <summary>
        /// 上下文的配置 (必填)
        /// </summary>
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
        /// <summary>
        /// ef实体的类型
        /// </summary>
        internal Type DbContextType { get; set; }

        /// <summary>
        /// 读写连接字符串的key ()
        /// </summary>
        internal string WriteReadConnectionString { get; set; }

        /// <summary>
        /// 只读的连接字符串的key 当IsOpenMasterSlave为true时 必须设置
        /// </summary>
        public string[] ReadOnlyConnectionString { get; set; }

        /// <summary>
        /// 是否开启读写分离的操作 (默认不开启)
        /// </summary>
        public bool IsOpenMasterSlave { get; set; } = false;

    }
}
