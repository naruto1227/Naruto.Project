using System;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Configuration.Repository;

using Fate.Common.OcelotStore.EFCore;
using Fate.Common.Repository.UnitOfWork;
using Fate.Common.Repository;
using Fate.Common.Repository.Object;
using Fate.Common.Redis;
using Fate.Common.Redis.IRedisManage;
using Ocelot.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 更改ocelot的存储方式为使用EF
        /// </summary>
        /// <param name="ocelotBuilder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddEFCache(this IOcelotBuilder ocelotBuilder, Action<CacheOptions> options)
        {
            //获取数据
            CacheOptions cacheOptions = new CacheOptions();
            options?.Invoke(cacheOptions);

            EFOptions eFOptions = new EFOptions();
            cacheOptions?.EFOptions.Invoke(eFOptions);
            //验证仓储服务是否注册
            if (ocelotBuilder.Services.BuildServiceProvider().GetService<IUnitOfWork>() == null)
            {
                ocelotBuilder.Services.AddMysqlRepositoryServer();
            }
            //注入仓储
            ocelotBuilder.Services.AddRepositoryEFOptionServer(ocelot =>
            {
                ocelot.ConfigureDbContext = eFOptions.ConfigureDbContext;
                ocelot.ReadOnlyConnectionString = eFOptions.ReadOnlyConnectionString;
                //
                ocelot.UseEntityFramework<OcelotDbContent>(ocelotBuilder.Services);
                ocelot.IsOpenMasterSlave = eFOptions.IsOpenMasterSlave;
            });
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
            ocelotBuilder.Services.RemoveAll(typeof(IFileConfigurationRepository));
            ocelotBuilder.Services.RemoveAll(typeof(IInternalConfigurationRepository));

            //重写提取Ocelot配置信息
            ocelotBuilder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();
            ocelotBuilder.Services.AddSingleton(EFConfigurationProvider.Get);
            ocelotBuilder.Services.AddSingleton<IFileConfigurationRepository, EFConfigurationRepository>();


            return ocelotBuilder;
        }
    }
}
