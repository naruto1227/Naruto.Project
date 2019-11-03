using Fate.Common.Configuration.Management.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 配置中心的参数
    /// </summary>
    public class ConfigurationOptions
    {
        /// <summary>
        /// 面板的配置
        /// </summary>
        public DashBoardOptions DashBoardOptions { get; set; } = new DashBoardOptions();
        /// <summary>
        /// 接口的配置
        /// </summary>
        public RequestOptions RequestOptions { get; set; } = new RequestOptions();
    }
}
