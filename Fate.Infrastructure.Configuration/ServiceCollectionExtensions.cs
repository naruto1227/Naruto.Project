using Fate.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 自定义配置扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 将自定义的配置加入
        /// </summary>
        /// <returns></returns>
        public static IConfigurationBuilder AddFateConfiguration(this IConfigurationBuilder @this)
        {
            return @this.Add(new FateConfigurationSource());
        }
        /// <summary>
        /// 注入配置更新服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFateConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IReloadData, DefaultReloadDataRedisProvider>();
            services.AddSingleton<IFateConfigurationLoadAbstract, DefaultFateConfigurationLoad>();
            return services;
        }
        /// <summary>
        /// 注入发布服务 （用于热更新）
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IServiceCollection AddPublishConfiguration(this IServiceCollection @this)
        {
            return @this.AddSingleton<IConfigurationPublish, DefaultConfigurationPublish>();
        }
        /// <summary>
        /// 订阅服务 实现 热更新
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseFateConfiguration(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IReloadData>().SubscribeReloadAsync(null).ConfigureAwait(false).GetAwaiter().GetResult();
            return app;
        }
    }
}
