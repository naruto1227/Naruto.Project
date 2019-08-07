
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fate.Common.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Fate.Common.Middleware;
using Fate.Common.FileOperation;
using Fate.Common.Config;
using Fate.Common.Extensions;
using Fate.Common.Ioc.Core;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fate.Common.Options;

namespace Fate.FileServerApi
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
            services.AddDirectoryBrowser();

            //注入 返回的实体
            services.AddTransient<MyJsonResult>();
            //注入文件操作类
            services.UseFileOptions(options =>
            {
                options.UploadFilePath = ConfigurationManage.GetValue("UploadFilePath");
            });
            // services.AddDirectoryBrowser();
            services.AddMvcCore().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ApplicationContainer = AutofacInit.Injection(services, 0);
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<FileUploadOptions> fileOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //定义一个文件夹的访问路径
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(fileOptions.Value.UploadFilePath),
                RequestPath = fileOptions.Value.RequestPathName,
                DefaultContentType = "application/x-msdownload",
                ServeUnknownFileTypes = true
            });
            //启用目录浏览
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(fileOptions.Value.UploadFilePath),
                RequestPath = "/" + fileOptions.Value.RequestPathName,
            });
            app.Map("/api/values", options =>
            {
                options.Run(async (content) =>
                {
                    await content.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("WelCome To FileSystem"));
                });
            });
            // app.UseDirectoryBrowser("/" + StaticFieldConfig.FileRequestPathName);
            //异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
