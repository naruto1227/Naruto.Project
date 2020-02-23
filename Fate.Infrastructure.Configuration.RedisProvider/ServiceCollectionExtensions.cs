using Fate.Infrastructure.Configuration;
using Fate.Infrastructure.Configuration.RedisProvider;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注入配置更新服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFateConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<ISubscribeReloadData, RedisSubscribeReloadData>();
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
    }
}
