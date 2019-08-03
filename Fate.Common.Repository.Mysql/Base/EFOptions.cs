using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Repository.Mysql.Base
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
        internal string WriteReadConnectionName { get; set; }

        /// <summary>
        /// 只读的连接字符串的key(必填)
        /// </summary>
        public string ReadOnlyConnectionName { get; set; }

    }
}
