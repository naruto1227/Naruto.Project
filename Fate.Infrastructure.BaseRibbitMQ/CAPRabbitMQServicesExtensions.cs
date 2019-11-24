using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetCore.CAP;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// CAP.RabbitMQ 的配置服务
    /// </summary>
    public static class CAPRabbitMQServicesExtensions
    {
        /// <summary>
        ///注入CAPRabbitMQ
        /// </summary>
        /// <param name="capOptions">配置cap的参数</param>
        /// <returns></returns>
        public static IServiceCollection AddCAPRibbitMQServices(this IServiceCollection services, Action<CapOptions> capOptions)
        {
            //检查订阅服务所在的程序集
            Assembly assembly = Assembly.Load("Fate.Infrastructure.CAP.Subscribe");
            if (assembly == null)
                throw new ApplicationException("程序集不存在");
            var types = assembly.GetTypes().Where(a => a.GetInterface("ISubscribe") != null).ToList();
            //编辑类
            types.ForEach(item =>
            {
                services.AddScoped(item);
            });
            //配置CAP
            services.AddCap(capOptions);
            return services;
        }
    }
}
