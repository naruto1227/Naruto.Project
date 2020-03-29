using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace Naruto.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-16
    /// 对外提供配置数据的中间件
    /// </summary>
    public class ConfigurationDataMiddleware
    {
        private readonly RequestDelegate next;

        public ConfigurationDataMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext">上下文</param>
        /// <param name="requestOptions">配置的请求的参数</param>
        /// <param name="dataServices">数据服务</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, IOptions<ConfigurationOptions> configurationOptions)
        {
            //验证请求地址
            if (!httpContext.Request.Path.StartsWithSegments(configurationOptions.Value.RequestOptions.RequestPath))
            {
                await next(httpContext);
                return;
            }
            //匹配请求方式 是否满足
            if (!httpContext.Request.Method.ToLower().Equals(configurationOptions.Value.RequestOptions.HttpMethod.ToLower()))
            {
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.MethodNotAllowed);
                return;
            }
            RequestContext requestContext = new RequestContext()
            {
                HttpContext = httpContext,
                ServiceProvider = httpContext.RequestServices,
            };
            //授权过滤器
            var authorizationList = configurationOptions.Value.RequestOptions.Authorization;
            if (authorizationList != null && authorizationList.Count() > 0)
            {
                //授权处理
                foreach (var item in authorizationList)
                {
                    if ((await item.AuthorizationAsync(requestContext)))
                        continue;
                    else
                    {
                        httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                        return;
                    }
                }
            }
            var dataServices = httpContext.RequestServices.GetService<IConfigurationDataServices>();
            //处理数据
            await dataServices.QueryDataAsync(requestContext);
        }
    }
}
