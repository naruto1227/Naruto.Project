using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace Fate.Infrastructure.Infrastructure
{
    /// <summary>
    /// 使用方法app.UseGlobalCore();
    /// </summary>
    public class GlobalCore
    {
        private static IApplicationBuilder _app;

        public static void Configure(IApplicationBuilder app)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRequiredService<T>()
        {
            using (var scope = _app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }


        public static HttpContext HttpContext => GetRequiredService<IHttpContextAccessor>().HttpContext;

        public static IConfiguration Configuration => GetRequiredService<IConfiguration>();

        public static IMemoryCache Cache => GetRequiredService<IMemoryCache>();

        public static string GetIp()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            ip = ip.Replace("::ffff:", "");
            return ip == "::1" ? "127.0.0.1" : ip;
        }

        public static string GetUrl()
        {
            var req = HttpContext.Request;
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        public static IEnumerable<Claim> GetClaim()
        {
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return HttpContext.User.Claims;
            }
            else
            {
                return null;
            }
        }

        public static string Browser() => HttpContext.Request.Headers["User-Agent"].ToString();

        public static string Header(string key) => HttpContext.Request.Headers[key].ToString();
    }
}