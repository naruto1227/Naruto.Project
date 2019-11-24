using Fate.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Fate.Infrastructure.Options;

namespace Fate.Infrastructure.Extensions
{
    public static partial class FileUploadExtensions
    {
        /// <summary>
        /// 注册文件上传的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseFileUpload(this IApplicationBuilder app, PathString path)
        {
            //获取配置的参数
            var fileOptions = app.ApplicationServices.GetRequiredService<IOptions<FileUploadOptions>>();
            //定义一个文件夹来保存文件的上传地址
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(fileOptions.Value.UploadFilePath),
                RequestPath = fileOptions.Value.RequestPathName,
                DefaultContentType = "application/x-msdownload",
                ServeUnknownFileTypes = true
            });

            app.UseWhen(predicate => predicate.Request.Path.StartsWithSegments(path), appBuild =>
            {
                appBuild.UseMiddleware<FileUploadMiddleware>();
            });

            return app;
        }
    }
}
