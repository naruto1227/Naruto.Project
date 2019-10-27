using Fate.Common.Configuration.Management.Dashboard;
using Microsoft.AspNetCore.Http;
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

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.StartsWithSegments(new PathString("/fate")))
            {
                await next(httpContext);
                return;
            }

            var i = DashboardRoute.Routes[httpContext.Request.Path];
            await i.LoadAsync(new DashboardContext() { HttpContext = httpContext, ResourcesName = "Fate.Common.Configuration.Management.Dashboard.Content.js.MD5.js" });
        }
    }
}
