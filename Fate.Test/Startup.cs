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
using Fate.Common.Repository.UnitOfWork;
using Fate.Common.Infrastructure;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;
using Fate.Common.Middleware;
using Fate.Common.Repository;
using Fate.Domain.Model.Entities;
using Fate.Common.Redis;
using Fate.Common.BaseRibbitMQ;
using Fate.Common.Repository.Object;
using Fate.Common.Extensions;
using Fate.Common.Options;
using Fate.Common.Repository.Interceptor;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Fate.Domain.Model;
using Fate.Commom.Consul;
using Fate.Commom.Consul.ServiceRegister;
using Fate.Commom.Consul.ServiceDiscovery;
using Fate.Commom.Consul.KVRepository;
using System.Net;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

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

            //注入响应压缩的服务
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            //注入redis仓储服务
            services.AddRedisRepository(options =>
            {
                options.Connection = ConfigurationManage.GetValue("RedisConfig:Connection");
                options.Password = ConfigurationManage.GetValue("RedisConfig:Password");
            });
            //注入mysql仓储   //注入多个ef配置信息
            services.AddMysqlRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql(Configuration.GetConnectionString("MysqlConnection"));
                options.ReadOnlyConnectionString = Configuration.GetConnectionString("ReadMysqlConnection").Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                //
                options.UseEntityFramework<MysqlDbContent>(services);
                options.IsOpenMasterSlave = false;
            });

            //使用单号
            services.UseOrderNo<IUnitOfWork<MysqlDbContent>>();

            //注入一个mini版的mvc 不需要包含Razor
            services.AddMvcCore(option =>
            {
                option.Filters.Add(typeof(Fate.Common.Filters.TokenAuthorizationAttribute));
            }).AddAuthorization().AddJsonFormatters().AddConfigurationManagement().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //注入api授权服务
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
            {
                option.Authority = "http://localhost:54717";
                option.RequireHttpsMetadata = false;
                option.Audience = "api";
            });
            services.AddScoped(typeof(List<>));
            services.UseFileOptions();
            //邮箱服务
            services.AddEmailServer(Configuration.GetSection("AppSetting:EmailConfig"));

            services.AddSingleton<Domain.Event.Infrastructure.Redis.RedisStoreEventBus>();
            //替换自带的di 转换为autofac 注入程序集
            ApplicationContainer = Fate.Common.AutofacDependencyInjection.AutofacDI.ConvertToAutofac(services);
            return new AutofacServiceProvider(ApplicationContainer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="options1"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<List<EFOptions>> options1)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //注入一场处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseAuthentication();

            app.UseResponseCompression();

            app.UseFileUpload(new Microsoft.AspNetCore.Http.PathString("/file"));
            //配置NLog
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件

            app.UseMvc();


        }
    }
}
