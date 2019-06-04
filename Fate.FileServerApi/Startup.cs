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
using Fate.Common.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Fate.Common.Middleware;
using Fate.Common.FileOperation;

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
            services.AddMvcCore().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //定义一个文件夹的访问路径
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), StaticFieldConfig.FileUploadPathName)),
                RequestPath ="/"+ StaticFieldConfig.FileRequestPathName,
            });
            app.Map("/api/values", options =>
            {
                options.Run(async (content) =>
                {
                    await content.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("welcome to fileSystem"));
                });
            });
            //异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
