using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Fate.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fate.Common.Repository.Mysql.UnitOfWork;
using Fate.Common.Repository.Mysql;
using Fate.Common.Interface;

namespace Fate.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// 容器
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //注入上下文
            services.AddDbContext<Fate.Common.Repository.Mysql.MysqlDbContent>(option => option.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //注入一个mini版的mvc 不需要包含Razor
            services.AddMvcCore(option =>
            {
                option.Filters.Add(typeof(Fate.Common.Filters.TokenAuthorizationAttribute));
            }).AddAuthorization().AddJsonFormatters().AddFormatterMappings().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //替换自带的di 转换为autofac 注入程序集
            ApplicationContainer = Common.Ioc.Core.AutofacInit.Injection(services);
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            ConfigurationManage.SetAppSetting(Configuration.GetSection("AppSetting"));
        }
    }
}
