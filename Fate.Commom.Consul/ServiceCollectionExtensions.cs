using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Fate.Commom.Consul.ServiceRegister;
using Fate.Commom.Consul.ServiceDiscovery;
using Fate.Commom.Consul.KVRepository;
using Fate.Commom.Consul.Object;
using Fate.Commom.Consul;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注入consul
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, Action<ConsulClientOptions> option)
        {
            services.TryAddSingleton(typeof(IConsulClientFactory), typeof(DefaultConsulClientFactory));
            services.TryAddSingleton(typeof(IServiceRegisterManage), typeof(DefaultServiceRegisterManage));

            services.TryAddSingleton(typeof(IServiceDiscoveryManage), typeof(DefaultServiceDiscoveryManage));
            services.TryAddSingleton(typeof(IKVRepository), typeof(DefaultKVRepository));
            services.Configure(option);
            return services;
        }
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="consul"></param>
        public static void UseServiceRegister(this IServiceCollection services, RegisterConfiguration registerConfiguration)
        {
            var serviceRegisterManage = services.BuildServiceProvider().GetRequiredService<IServiceRegisterManage>();
            serviceRegisterManage.ServiceRegister(registerConfiguration);
        }
    }
}
