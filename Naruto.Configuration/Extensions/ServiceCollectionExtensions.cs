using Naruto.Configuration;
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
        /// 订阅服务 实现 热更新
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseFateConfiguration(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<ISubscribeReloadData>().ReloadAsync(null).ConfigureAwait(false).GetAwaiter().GetResult();
            return app;
        }
    }
}
