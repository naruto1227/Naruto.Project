using Fate.Common.Configuration.Management.Dashboard.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 加载资源
    /// </summary>
    public class DefaultDashboardRender : IDashboardRender
    {
        /// <summary>
        /// 获取当前项目的程序集
        /// </summary>
        private readonly Assembly assembly;

        public DefaultDashboardRender()
        {
            assembly = typeof(DefaultDashboardRender).Assembly;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="dashboardContext"></param>
        /// <returns></returns>
        public async Task LoadAsync(DashboardContext dashboardContext)
        {
            if (dashboardContext == null)
                throw new ArgumentNullException(nameof(dashboardContext));
            if (string.IsNullOrWhiteSpace(dashboardContext.ResourcesName))
                throw new ArgumentNullException(nameof(dashboardContext.ResourcesName));
            //加载程序集 获取指定的资源
            using (var resourcesStream = assembly.GetManifestResourceStream(dashboardContext.ResourcesName))
            {
                if (resourcesStream == null)
                    return;
                //添加响应类型
                dashboardContext.HttpContext.Response.ContentType = dashboardContext.ContentType;
                //设置缓存时间
                dashboardContext.HttpContext.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromMinutes(30)
                };
                await resourcesStream.CopyToAsync(dashboardContext.HttpContext.Response.Body);
            }
        }
    }
}
