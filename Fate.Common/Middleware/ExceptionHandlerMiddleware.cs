using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Fate.Common.Infrastructure;
using Fate.Common.NLog;
using Newtonsoft.Json;
using Fate.Common.Exceptions;
using Fate.Common.Enum;

namespace Fate.Common.Middleware
{
    /// <summary>
    /// 异常处理的中间件
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        /// <summary>
        /// 定义一个请求委托
        /// </summary>
        private readonly RequestDelegate next;

        private readonly MyJsonResult myJsonResult;
        private NLogHelper nLog;
        public ExceptionHandlerMiddleware(RequestDelegate request, MyJsonResult _myJsonResult, NLogHelper _nLog)
        {
            next = request;
            myJsonResult = _myJsonResult;
            nLog = _nLog;
        }
        /// <summary>
        /// 中间件的处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ex = ex.GetBaseException();
                if (ex is MyExceptions || ex is ApplicationException)
                {
                    //获取状态码
                    myJsonResult.code = (int)MyJsonResultEnum.checkCode;
                    myJsonResult.msg = ex.Message;
                }
                else if (ex is Exception)
                {
                    //获取状态码
                    myJsonResult.code = (int)MyJsonResultEnum.serverCode;
                    myJsonResult.failMsg = ex.Message;
                    myJsonResult.msg = "请求异常";
                    //转换成错误的集合
                    await nLog.Error("\n错误消息：" + ex.ToString().Trim() + "\n");//记录日志
                }

                await HandleExceptionAsync(context, myJsonResult);
            }
        }
        /// <summary>
        /// 事件的处理与数据的返回
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonResult"></param>
        /// <returns></returns>

        private static Task HandleExceptionAsync(HttpContext context, MyJsonResult jsonResult)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(jsonResult));
        }

    }
}
