using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Fate.Commom.Consul.ServiceRegister;

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
            services.TryAddScoped(typeof(IServiceRegisterManage), typeof(DefaultServiceRegisterManage));
            services.Configure(option);
            return services;
        }
    }
}
