using Fate.Common.OcelotStore.RedisProvider;
using Fate.Common.Redis.IRedisManage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 张海波
    /// 2019-11-13
    /// 实用redis存储替换IInternalConfigurationRepository的服务
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 实用redis存储替换IInternalConfigurationRepository的服务
        /// </summary>
        /// <param name="ocelotBuilder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddRedisProvider(this IOcelotBuilder ocelotBuilder, Action<CacheOptions> options)
        {
            //获取数据
            CacheOptions cacheOptions = new CacheOptions();
            options?.Invoke(cacheOptions);

            //检验是否注入redis仓储
            if (ocelotBuilder.Services.BuildServiceProvider().GetService<IRedisOperationHelp>() == null)
            {
                if (cacheOptions.RedisOptions == null)
                {
                    throw new ArgumentNullException(nameof(cacheOptions.RedisOptions));
                }
                //注入redis仓储
                ocelotBuilder.Services.AddRedisRepository(cacheOptions.RedisOptions);
            }

            //更改扩展方式
            ocelotBuilder.Services.RemoveAll(typeof(IInternalConfigurationRepository));

            //重写ocelot每次请求获取配置的服务
            ocelotBuilder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();

            return ocelotBuilder;
        }
    }
}
