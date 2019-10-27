using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management
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

        }
    }
}
