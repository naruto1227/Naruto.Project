using System;
using System.Collections.Generic;
using System.Text;
using Naruto.OcelotStore.Redis;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Naruto.Redis;
using Naruto.Redis.IRedisManage;
using Ocelot.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 更改ocelot的存储方式为使用redis 
        /// </summary>
        /// <param name="ocelotBuilder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddRedisCache(this IOcelotBuilder ocelotBuilder)
        {
            return ocelotBuilder.AddRedisCache((option) => { });
        }

        /// <summary>
        /// 更改ocelot的存储方式为使用redis 
        /// </summary>
        /// <param name="ocelotBuilder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddRedisCache(this IOcelotBuilder ocelotBuilder, Action<CacheOptions> options)
        {
            //获取数据
            CacheOptions cacheOptions = new CacheOptions();
            options?.Invoke(cacheOptions);
            //验证仓储服务是否注册
            if (ocelotBuilder.Services.BuildServiceProvider().GetService<IRedisOperationHelp>() == null)
            {
                if (cacheOptions != null && cacheOptions.RedisOptions != null)
                {
                    ocelotBuilder.Services.AddRedisRepository(cacheOptions.RedisOptions);
                }
                else
                    throw new ApplicationException("当前检查没有配置Redis仓储服务");
            }

            //更改扩展方式
            ocelotBuilder.Services.RemoveAll(typeof(IOcelotCache<FileConfiguration>));
            ocelotBuilder.Services.RemoveAll(typeof(IOcelotCache<CachedResponse>));

            ocelotBuilder.Services.TryAddSingleton<IOcelotCache<FileConfiguration>, OcelotRedisManagerCache<FileConfiguration>>();
            ocelotBuilder.Services.TryAddSingleton<IOcelotCache<CachedResponse>, OcelotRedisManagerCache<CachedResponse>>();
            ocelotBuilder.Services.RemoveAll(typeof(IFileConfigurationRepository));

            //重写提取Ocelot配置信息
            ocelotBuilder.Services.AddSingleton(RedisConfigurationProvider.Get);
            ocelotBuilder.Services.AddSingleton<IFileConfigurationRepository, RedisConfigurationRepository>();
            return ocelotBuilder;
        }
    }
}
