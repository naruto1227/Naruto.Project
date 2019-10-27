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
        /// 请求地址
        /// </summary>
        public PathString RequestPath { get; set; }
    }
}
