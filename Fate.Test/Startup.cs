using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Autofac;

using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Fate.Common.Repository.Mysql.UnitOfWork;
using Fate.Common.Infrastructure;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;
using Fate.Common.Middleware;
using Fate.Common.Repository.Mysql;
using Fate.Domain.Model.Entities;

namespace Fate.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //注入上下文
            services.AddDbContext<MysqlDbContent>(option => option.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton(typeof(Fate.Domain.Event.Infrastructure.IEventBus), typeof(Fate.Domain.Event.Infrastructure.EventBus));
            //注入一个mini版的mvc 不需要包含Razor
            services.AddMvcCore(option =>
            {
                option.Filters.Add(typeof(Fate.Common.Filters.TokenAuthorizationAttribute));
            }).AddAuthorization().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //注入api授权服务
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
            {
                option.Authority = "http://localhost:54717";
                option.RequireHttpsMetadata = false;
                option.Audience = "api";
            });
            //注入仓储
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

            services.AddScoped(typeof(List<>));

            services.AddSingleton<Domain.Event.Infrastructure.Redis.RedisStoreEventBus>();
            //替换自带的di 转换为autofac 注入程序集
            ApplicationContainer = Fate.Common.Ioc.Core.AutofacInit.Injection(services);
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //配置NLog
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件
            //注入一场处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
