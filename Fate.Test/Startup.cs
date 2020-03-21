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
using Fate.Infrastructure.Repository.UnitOfWork;
using Fate.Infrastructure.Infrastructure;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;
using Fate.Infrastructure.Middleware;
using Fate.Infrastructure.Repository;
using Fate.Domain.Model.Entities;
using Fate.Infrastructure.Redis;
using Fate.Infrastructure.BaseRibbitMQ;
using Fate.Infrastructure.Repository.Object;
using Fate.Infrastructure.Extensions;
using Fate.Infrastructure.Options;
using Fate.Infrastructure.Repository.Interceptor;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Fate.Domain.Model;
using Fate.Infrastructure.Consul;
using Fate.Infrastructure.Consul.ServiceRegister;
using Fate.Infrastructure.Consul.ServiceDiscovery;
using Fate.Infrastructure.Consul.KVRepository;
using System.Net;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;
using Fate.Infrastructure.Configuration.Management.Dashboard;
using Fate.Infrastructure.Configuration.Management.DB;
using Fate.Infrastructure.Configuration.Management;
using Fate.Infrastructure.Redis.IRedisManage;
using Microsoft.Extensions.Hosting;
using Fate.Infrastructure.AutofacDependencyInjection;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Fate.Test
{
    public class TestOption
    {
        public string test1 { get; set; }
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        private static IConfiguration configuration2;
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //        services.Replace(ServiceDescriptor
            //.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            //注入响应压缩的服务
            //services.AddResponseCompression();
            //services.Configure<GzipCompressionProviderOptions>(options =>
            //{
            //    options.Level = CompressionLevel.Fastest;
            //});
            //services.AddDistributedMemoryCache();
            //注入redis仓储服务
            services.AddRedisRepository(Configuration.GetSection("AppSetting:RedisConfig"));
            //注入mysql仓储   //注入多个ef配置信息
            services.AddRepository();

            services.AddEFOption(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql(Configuration.GetConnectionString("MysqlConnection"));
                options.ReadOnlyConnectionString = new string[] { "Database=test;DataSource=127.0.0.1;Port=33;UserId=hairead;Password=hai123;Charset=utf8;" };
                //
                options.UseEntityFramework<MysqlDbContent, SlaveMysqlDbContent>();
                options.IsOpenMasterSlave = true;
            }
            );
            services.AddEFOption(configureOptions =>
            {
                configureOptions.ConfigureDbContext = context => context.UseMySql("Database=ConfigurationDB;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;");
                configureOptions.UseEntityFramework<ConfigurationDbContent>();
            }
            );
            //var res = Configuration.GetSection("MongonConfigs").Get<List<MongoContext>>();
            ////mongo服务
            //services.AddMongoServices(Configuration.GetSection("MongonConfigs"));
            ////使用单号
            ////services.UseOrderNo<IUnitOfWork<MysqlDbContent>>();
            //services.AddPublishConfiguration();
            services.AddControllers();
            //services.AddControllers(option =>
            //{
            //    option.Filters.Add(typeof(Fate.Infrastructure.Filters.TokenAuthorizationAttribute));
            //})
            //               .AddConfigurationManagement(options =>
            //               {
            //                   options.EnableDashBoard = true;
            //                   options.RequestOptions = new RequestOptions
            //                   {
            //                       HttpMethod = "get",
            //                       RequestPath = "/api/data",

            //                   };
            //                   options.EnableDataRoute = true;
            //               });
            ////注入api授权服务
            //services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
            //{
            //    option.Authority = "http://localhost:54717";
            //    option.RequireHttpsMetadata = false;
            //    option.Audience = "api";
            //});
            //services.AddScoped(typeof(List<>));
            //services.UseFileOptions();

            // services.AddFateConfiguration();
            ////邮箱服务
            //services.AddEmailServer(Configuration.GetSection("AppSetting:EmailConfig"));
            //services.BuildServiceProvider()
            //services.AddSingleton<Domain.Event.Infrastructure.Redis.RedisStoreEventBus>();

            services.Configure<TestOption>(Configuration.GetSection("test"));
            //替换自带的di 转换为autofac 注入程序集
        }

        ///// <summary>
        ///// 使用autofac（在ConfigureServices之后执行）
        ///// </summary>
        ///// <param name="builder"></param>
        //public void ConfigureContainer(ContainerBuilder builder)

        //{ //获取所有控制器类型并使用属性注入
        //    var controllerBaseType = typeof(ControllerBase);
        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        //        //启用属性注入
        //        .PropertiesAutowired();
        //    builder.RegisterModule(new AutofacModule());
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="options1"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // app.UseFateConfiguration();
            configuration2 = Configuration;
            // var redis = app.ApplicationServices.GetRequiredService<IRedisOperationHelp>();
            //redis.Subscribe("changeConfiguration", (channel, redisvalue) =>
            //{

            //    Configuration["test"] = "2";
            //    var res = Configuration.AsEnumerable();
            //    using (var scope = app.ApplicationServices.CreateScope())
            //    {
            //        var test = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TestOption>>();
            //    }
            //    Console.WriteLine("1");
            //});
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //注入一场处理中间件
            // app.UseMiddleware<ExceptionHandlerMiddleware>();
            // app.UseMiddleware<DashBoardMiddleware>();
            //app.UseAuthentication();

            //app.UseResponseCompression();

            //app.UseFileUpload(new Microsoft.AspNetCore.Http.PathString("/file"));
            ////配置NLog
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            //loggerFactory.AddNLog();//添加NLog
            //env.ConfigureNLog("nlog.config");//读取Nlog配置文件

            app.UseRouting();
            app.UseEndpoints(builds =>
            {
                builds.MapControllers();
                builds.MapGet("/hello", async content =>
                 {
                     using (var scope = app.ApplicationServices.CreateScope())
                     {
                         var test = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TestOption>>();

                         var str = Encoding.UTF8.GetBytes(configuration2.GetValue<string>("1") + ":" + test.Value.test1);
                         await content.Response.Body.WriteAsync(str, 0, str.Length);
                     }
                 });
            });
        }
    }
}
