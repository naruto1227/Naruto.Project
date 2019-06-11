
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
namespace Fate.FileServerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //设置文件 上传的 大小
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
            });
            //注入 返回的实体
            services.AddTransient<MyJsonResult>();
            //注入文件操作类
            services.AddSingleton<FileHelper>();
            services.AddSingleton<UploadFile>();
            // services.AddDirectoryBrowser();
            services.AddMvcCore().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //注入读取配置文件服务
            ConfigurationManage.SetAppSetting(Configuration.GetSection("AppSetting"));
            //定义一个文件夹的访问路径
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(StaticFieldConfig.UploadFilePath),
                RequestPath = "/" + StaticFieldConfig.FileRequestPathName,
            });
            app.Map("/api/values", options =>
            {
                options.Run(async (content) =>
                {
                    await content.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("welcome to fileSystem"));
                });
            });
            // app.UseDirectoryBrowser("/" + StaticFieldConfig.FileRequestPathName);
            //异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
