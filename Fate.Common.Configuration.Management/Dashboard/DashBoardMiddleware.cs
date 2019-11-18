
using Fate.Common.Configuration.Management.Dashboard.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task InvokeAsync(HttpContext httpContext, IOptions<ConfigurationOptions> configurationOptions)
        {
            //匹配路由
            if (!httpContext.Request.Path.StartsWithSegments(configurationOptions.Value.DashBoardOptions.RequestPath, out PathString matched, out PathString remaining))
            {
                await next(httpContext);
                return;
            }
            //获取面板路由服务
            var dashboardRoute = httpContext.RequestServices.GetService<IDashboardRoute>();
            //接收请求地址
            var requestPath = httpContext.Request.Path;
            //验证访问的是否为首页的资源
            if (string.IsNullOrWhiteSpace(remaining))
            {
                requestPath = dashboardRoute.MainPageName;
            }
            //路由集合服务
            var routeCollections = httpContext.RequestServices.GetService<IDashboardRouteCollections>();
            //验证当前文件是否存在
            var resourceInfo = routeCollections.Get(requestPath);
            if (resourceInfo == null)
            {
                await next(httpContext);
                return;
            }
            //设置上下文
            var dashbordContext = new DashboardContext(dashboardRoute.GetContentResourceName(resourceInfo.Item1, dashboardRoute.GetFileName(requestPath, resourceInfo.Item1)), resourceInfo.Item2, httpContext);
            //授权过滤器
            var authorizationList = configurationOptions.Value.DashBoardOptions?.Authorization;
            if (authorizationList != null && authorizationList.Count() > 0)
            {
                //授权处理
                foreach (var item in authorizationList)
                {
                    if ((await item.AuthorizationAsync(dashbordContext)))
                        continue;
                    else
                    {
                        httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                        return;
                    }
                }
            }
            //面板渲染服务
            var dashboardRender = httpContext.RequestServices.GetService<IDashboardRender>();
            //处理响应
            await dashboardRender.LoadAsync(dashbordContext);
        }
    }
}
