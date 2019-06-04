using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fate.Common.Extensions
{
    /// <summary>
    /// 通用的扩展类
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
