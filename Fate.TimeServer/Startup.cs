using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.TimeServer.Scheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace Fate.TimeServer
{
    public class Startup
    {

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LogScheduler>();
            //注入后台服务
            services.AddHostedService<StartSchedulerHostServer>();
        }

        // 请求中间件
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Welcome To TimeServer");
            });
        }
    }
}
