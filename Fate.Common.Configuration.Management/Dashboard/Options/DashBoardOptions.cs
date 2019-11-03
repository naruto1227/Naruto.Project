using Fate.Common.Configuration.Management.Dashboard.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 面板的参数
    /// </summary>
    public class DashBoardOptions
    {
        /// <summary>
        /// 首页地址 默认(/fate)
        /// </summary>
        public PathString RequestPath { get; set; } = new PathString("/fate");

        /// <summary>
        /// 面板的授权接口过滤器
        /// </summary>
        public IEnumerable<IDashboardAuthorizationFilters> Authorization { get; set; }
    }
}
