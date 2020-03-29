
using Naruto.VirtualFile;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 注入虚拟文件系统服务
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVirtualFileServices(this IServiceCollection services, Action<VirtualFileOptions> options)
        {
            services.Configure(options);
            services.AddSingleton<IVirtualFileRender, DefaultVirtualFileRender>();
            services.AddSingleton<IVirtualFileRouteCollections, DefaultVirtualFileRouteCollections>();
            services.AddSingleton<IVirtualFileResource, DefaultVirtualFileResource>();
            //注入中间件
            services.AddScoped(typeof(IStartupFilter), typeof(VirtualFileStartupFilter));
            return services;
        }
    }
}
