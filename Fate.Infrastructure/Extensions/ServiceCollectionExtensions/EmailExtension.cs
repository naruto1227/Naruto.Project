using Fate.Infrastructure.Email;
using Fate.Infrastructure.Interface;
using Fate.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 邮件的服务注册
    /// </summary>
    public static class EmailExtension
    {
        /// <summary>
        /// 注入邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailServer(this IServiceCollection services, Action<EmailOptions> options)
        {
            services.AddServer();
            services.Configure(options);
            return services;
        }

        /// <summary>
        /// 注入邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailServer(this IServiceCollection services, IConfiguration config)
        {
            services.AddServer();
            services.Configure<EmailOptions>(config);
            return services;
        }
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServer(this IServiceCollection services)
        {
            return services.AddSingleton<IEmail, EmailKit>();
        }
    }
}
