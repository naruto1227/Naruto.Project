using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Fate.Infrastructure.RabbitMQ
{
    public static class RabbitMQExtension
    {
        /// <summary>
        /// 注入RibbitMQ参数配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddRibbitMQServer(this IServiceCollection services, Action<RabbitMQOption> configure)
        {
            services.Configure(configure);
            return services;
        }
    }
}
