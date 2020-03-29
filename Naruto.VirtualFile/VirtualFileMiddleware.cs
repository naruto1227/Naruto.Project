
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 资源处理的中间件
    /// </summary>
    public class VirtualFileMiddleware
    {
        private readonly RequestDelegate next;

        public VirtualFileMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="configurationOptions"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, IOptions<VirtualFileOptions> configurationOptions)
        {
            try
            {
                //匹配路由
                if (!httpContext.Request.Path.StartsWithSegments(configurationOptions.Value.RequestPath, out PathString matched, out PathString remaining))
                {
                    await next(httpContext);
                    return;
                }
                //获取面板路由服务
                var virtualFileResource = httpContext.RequestServices.GetService<IVirtualFileResource>();
                //接收请求地址
                var requestPath = httpContext.Request.Path;

                //路由集合服务
                var routeCollections = httpContext.RequestServices.GetService<IVirtualFileRouteCollections>();
                //验证当前文件是否存在
                var resourceInfo = routeCollections.Get(requestPath);
                if (resourceInfo == null)
                {
                    await next(httpContext);
                    return;
                }
                //设置上下文
                var virtualFileContext = new VirtualFileContext(virtualFileResource.GetContentResourceName(resourceInfo.Item1, virtualFileResource.GetFileName(requestPath, resourceInfo.Item1)), resourceInfo.Item2, httpContext);
                //授权过滤器
                var authorizationList = configurationOptions.Value?.Authorization;
                if (authorizationList != null && authorizationList.Count() > 0)
                {
                    //授权处理
                    foreach (var item in authorizationList)
                    {
                        if ((await item.AuthorizationAsync(virtualFileContext)))
                            continue;
                        else
                        {
                            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                            return;
                        }
                    }
                }
                //资源渲染服务
                var virtualFileRender = httpContext.RequestServices.GetService<IVirtualFileRender>();
                //处理响应
                await virtualFileRender.LoadAsync(virtualFileContext);
            }
            catch (Exception ex)
            {
                var errorMsg = Encoding.UTF8.GetBytes(ex.Message);
                await httpContext.Response.Body.WriteAsync(errorMsg, 0, errorMsg.Length);
            }
        }
    }
}
