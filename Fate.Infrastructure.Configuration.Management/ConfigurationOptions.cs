using Fate.Infrastructure.Configuration.Management.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 配置中心的参数
    /// </summary>
    public class ConfigurationOptions
    {
        /// <summary>
        /// 是否启用接口路由 默认不启用
        /// </summary>
        public bool EnableDataRoute { get; set; } = false;
        /// <summary>
        /// 接口的配置
        /// </summary>
        public RequestOptions RequestOptions { get; set; } = new RequestOptions();
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
