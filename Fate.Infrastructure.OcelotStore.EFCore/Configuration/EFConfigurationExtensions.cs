using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fate.Infrastructure.OcelotStore.EFCore
{
    public static class EFConfigurationExtensions
    {
        /// <summary>
        /// 重新设置
        /// </summary>
        /// <param name="app"></param>
        public static void ResetEFConfiguration(this IApplicationBuilder app, PathString pathString)
        {
            app.MapWhen(context => context.Request.Path.StartsWithSegments(pathString), build =>
            {
                build.Run(async context =>
                {
                    //获取配置信息
                    var option = build.ApplicationServices.GetService<IOptions<CacheOptions>>();
                    //清理缓存
                    var ocelotCache = build.ApplicationServices.GetService<IOcelotCache<FileConfiguration>>();
                    ocelotCache.ClearRegion(option.Value.CacheKey);
                    //重新设置配置
                    await EFConfigurationProvider.Get(build);
                    Console.WriteLine("重新设置完成");
                });
            });
        }
    }
}
