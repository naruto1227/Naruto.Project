using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 上下文
    /// </summary>
    public class VirtualFileContext
    {
        public VirtualFileContext(string resourcesName, string contentType, HttpContext httpContext)
        {
            ResourcesName = resourcesName;
            ContentType = contentType;
            HttpContext = httpContext;
            ServiceProvider = httpContext.RequestServices;
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
        public  HttpContext HttpContext { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public  IServiceProvider ServiceProvider { get; internal set; }
    }
}
