using Fate.Common.Configuration.Management;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Fate.Common.Configuration.Management.Dashboard;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 注入配置界面
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注入配置界面
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddConfigurationManagement(this IMvcBuilder mvcBuilder)
        {
            //注入mvc扩展
            mvcBuilder.ConfigureApplicationPartManager(a =>
            {
                a.ApplicationParts.Add(new AssemblyPart(typeof(ServiceCollectionExtensions).Assembly));
            });
            return mvcBuilder;
        }
        /// <summary>
        /// 注入配置界面
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddConfigurationManagement(this IMvcCoreBuilder mvcBuilder)
        {
            mvcBuilder.Services.AddServices();
            //注入mvc扩展
            mvcBuilder.ConfigureApplicationPartManager(a =>
            {
                a.ApplicationParts.Add(new AssemblyPart(typeof(ServiceCollectionExtensions).Assembly));
            });
            return mvcBuilder;
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStartupFilter), typeof(ConfigurationStartupFilter));
            return services;
        }
    }
}
