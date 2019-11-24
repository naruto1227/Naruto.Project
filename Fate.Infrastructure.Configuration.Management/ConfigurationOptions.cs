using Fate.Infrastructure.Configuration.Management.Dashboard;
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
        /// 是否启用面板(默认不启动)
        /// </summary>
        public bool EnableDashBoard { get; set; } = false;

        /// <summary>
        /// 是否启用接口路由 默认不启用
        /// </summary>
        public bool EnableDataRoute { get; set; } = false;
        /// <summary>
        /// 接口的配置
        /// </summary>
        public RequestOptions RequestOptions { get; set; } = new RequestOptions();
    }
}
