using Fate.Common.Configuration.Management.Dashboard;
using Fate.Common.Configuration.Management.Dashboard.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 面板的中间件
    /// </summary>
    public class DashBoardMiddleware
    {
        private readonly RequestDelegate next;

        public DashBoardMiddleware(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IDashboardRouteCollections routeCollections, IDashboardRender dashboardRender, IOptions<DashBoardOptions> dashBoardOptions, IDashboardRoute dashboardRoute)
        {
            if (!httpContext.Request.Path.StartsWithSegments(dashBoardOptions.Value.RequestPath, out PathString matched, out PathString remaining))
            {
                await next(httpContext);
                return;
            }
            //接收请求地址
            var requestPath = httpContext.Request.Path;
            //验证访问的是否为首页的资源
            if (string.IsNullOrWhiteSpace(remaining))
            {
                requestPath = dashboardRoute.MainPageName;
            }
            //验证当前文件是否存在
            var resourceInfo = routeCollections.Get(requestPath);
            if (resourceInfo == null)
            {
                await next(httpContext);
                return;
            }

            await dashboardRender.LoadAsync(new DashboardContext(dashboardRoute.GetContentResourceName(resourceInfo.Item1, dashboardRoute.GetFileName(requestPath, resourceInfo.Item1)), resourceInfo.Item2, httpContext));
        }
    }
}
