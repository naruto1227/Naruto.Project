using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 面板的上下文
    /// </summary>
    public class DashboardContext
    {
        public DashboardContext()
        {

        }
        public DashboardContext(string resourcesName, string contentType, HttpContext httpContext)
        {
            ResourcesName = resourcesName;
            ContentType = contentType;
            HttpContext = httpContext;
        }
        /// <summary>
        /// 资源的名称
        /// </summary>
        public string ResourcesName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 请求的上下文
        /// </summary>
        public HttpContext HttpContext { get; set; }
    }
}
