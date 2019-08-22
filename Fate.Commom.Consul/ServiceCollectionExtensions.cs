using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Fate.Commom.Consul.ServiceRegister;
using Fate.Commom.Consul.ServiceDiscovery;

namespace Fate.Commom.Consul
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

            services.Configure(option);
            return services;
        }
    }
}
