using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Middleware
{
    /// <summary>
    /// 接口日志中间件
    /// </summary>
    public class InterfaceLogMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger logger;


        public InterfaceLogMiddleware(RequestDelegate _next, ILogger<InterfaceLogMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //不记录根请求 减少日志量
            if (httpContext.Request.Path.Value.Equals("/"))
            {
                //执行下一个中间件
                await next(httpContext);
                return;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"【接口地址：{httpContext.Request.Path.Value} ,Method:{httpContext.Request.Method}】");
            //heard
            stringBuilder.Append($"\n【Heared:{httpContext.Request.Headers.ToJson()}】");
            //参数
            var paramter = "";
            //定义一个流接收
            StreamReader streamReader = default;
            if (httpContext.Request.Method == HttpMethods.Get)
            {
                var query = httpContext.Request.Query;
                paramter = query.ToJson();
            }
            else
            {
                //使用默认为30kb大小的内存流备份FileBufferingReadStream ,创建一个Request.Body的缓冲区 当body大小超过30kb 将触发异常
                httpContext.Request.EnableBuffering();
                streamReader = new StreamReader(httpContext.Request.Body);
                paramter = await streamReader.ReadToEndAsync();
                //重新设置流的位置以便下一个中间件读取
                httpContext.Request.Body.Position = 0;
            }
            stringBuilder.Append($"\n【Pamater:{paramter}】");
            //记录日志
            logger.LogTrace(stringBuilder.ToString());
            //执行下一个中间件
            await next(httpContext);
            //释放资源
            streamReader?.Close();
            streamReader?.Dispose();
        }
    }
}
