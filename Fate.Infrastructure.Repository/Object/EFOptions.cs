using Microsoft.EntityFrameworkCore;
using System;

namespace Fate.Infrastructure.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// EF上下文参数配置
    /// </summary>
    public class EFOptions
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
        /// ef实体的类型
        /// </summary>
        internal Type DbContextType { get; set; }
        /// <summary>
        /// 从库的上下文类型
        /// </summary>
        internal Type SlaveDbContextType { get; set; }

        /// <summary>
        /// 读写连接字符串的key ()
        /// </summary>
        internal string WriteReadConnectionString { get; set; }

        /// <summary>
        /// 只读的连接字符串的key 当IsOpenMasterSlave为true时 必须设置
        /// </summary>
        public string[] ReadOnlyConnectionString { get; set; }



    }
}
