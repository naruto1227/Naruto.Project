using Fate.Common.Configuration.Management.Dashboard.Interface;
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
    public class DashboardRender : IDashboardRender
    {
        public async Task LoadAsync(DashboardContext dashboardContext)
        {
            if (dashboardContext == null)
                throw new ArgumentNullException(nameof(dashboardContext));
            if (string.IsNullOrWhiteSpace(dashboardContext.ResourcesName))
                throw new ArgumentNullException(nameof(dashboardContext.ResourcesName));
            //加载程序集 获取指定的资源
            using (var resourcesStream = typeof(DashboardRender).Assembly.GetManifestResourceStream(dashboardContext.ResourcesName))
            {
                dashboardContext.HttpContext.Response.ContentType = dashboardContext.ContentType;
                await resourcesStream.CopyToAsync(dashboardContext.HttpContext.Response.Body);
            }
        }
    }
}
