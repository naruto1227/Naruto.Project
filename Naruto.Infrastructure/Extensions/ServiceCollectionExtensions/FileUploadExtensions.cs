using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Naruto.Infrastructure.Options;
using System.IO;
using Naruto.Infrastructure.FileOperation;
using Naruto.Infrastructure.Interface;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class FileUploadExtensions
    {
        /// <summary>
        /// 配置文件上传的参数
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseFileOptions(this IServiceCollection services)
        {
            return services.UseFileOptions(options => { });
        }

        /// <summary>
        /// 配置文件上传的参数
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseFileOptions(this IServiceCollection services, Action<FileUploadOptions> options)
        {
            FileUploadOptions fileUploadOptions = new FileUploadOptions();
            options?.Invoke(fileUploadOptions);
            if (string.IsNullOrWhiteSpace(fileUploadOptions.UploadFilePath))
                throw new ArgumentNullException("参数不能为空:" + nameof(fileUploadOptions.UploadFilePath));
            if (fileUploadOptions != null && !string.IsNullOrWhiteSpace(fileUploadOptions.UploadFilePath))
            {
                if (!Directory.Exists(fileUploadOptions.UploadFilePath))
                {
                    Directory.CreateDirectory(fileUploadOptions.UploadFilePath);
                }
            }
            //注入文件操作类
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IUploadFile, UploadFile>();
            services.AddTransient<Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider>();
            return services.Configure(options);
        }
    }
}
