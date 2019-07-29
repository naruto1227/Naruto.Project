using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fate.Common.Options;
using System.IO;

namespace Fate.Common.Extensions
{
    public static class FileUploadExtensions
    {
        /// <summary>
        /// 配置文件上传的参数
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseFileOptions(this IServiceCollection services, Action<FileUploadOptions> options)
        {
            FileUploadOptions fileUploadOptions = new FileUploadOptions();
            options?.Invoke(fileUploadOptions);

            if (fileUploadOptions != null && !string.IsNullOrWhiteSpace(fileUploadOptions.UploadFilePath))
            {
                if (!Directory.Exists(fileUploadOptions.UploadFilePath))
                {
                    Directory.CreateDirectory(fileUploadOptions.UploadFilePath);
                }
            }
            return services.Configure(options);
        }
    }
}
