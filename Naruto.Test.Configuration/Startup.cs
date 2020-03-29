using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naruto.Configuration.Management.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Naruto.Test.Configuration
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddPublishConfiguration();
            //services.AddRedisRepository(option =>
            //{
            //    option.Connection = new string[] { "127.0.0.1:6379" };
            //});

            services.AddControllers().AddConfigurationManagement(new Naruto.Configuration.Management.OcelotEFOption
            {
                ConfigureDbContext = context => context.UseMySql("Database=ConfigurationDB;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;"),
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
