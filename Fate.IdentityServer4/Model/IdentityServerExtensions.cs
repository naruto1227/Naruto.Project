using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Fate.IdentityServer4.Model
{
    public static class IdentityServerExtensions
    {
        /// <summary>
        /// 添加一个identityserver4 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIds4(this IServiceCollection services)
        {
            services.AddIdentityServer().AddDeveloperSigningCredential()
               .AddInMemoryIdentityResources(Config.GetIdentityResources())
               .AddInMemoryApiResources(Config.GetApis())
               .AddInMemoryClients(Config.GetClients())
               .AddTestUsers(Config.GetUsers());
            return services;
        }
        /// <summary>
        /// 注册中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>

        public static IApplicationBuilder UseIds4(this IApplicationBuilder app) {
            app.UseIdentityServer();
            return app;
        }
    }
}
