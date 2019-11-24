using System;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Configuration.Repository;

using Fate.Infrastructure.OcelotStore.EFCore;
using Fate.Infrastructure.Repository.UnitOfWork;
using Fate.Infrastructure.Repository;
using Fate.Infrastructure.Repository.Object;
using Fate.Infrastructure.Redis;
using Fate.Infrastructure.Redis.IRedisManage;
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
                ocelotBuilder.Services.AddRepositoryServer();
            }
            //注入仓储
            ocelotBuilder.Services.AddRepositoryEFOptionServer(ocelot =>
            {
                ocelot.ConfigureDbContext = eFOptions.ConfigureDbContext;
                ocelot.ReadOnlyConnectionString = eFOptions.ReadOnlyConnectionString;
                //
                ocelot.UseEntityFramework<OcelotDbContent>();
                ocelot.IsOpenMasterSlave = eFOptions.IsOpenMasterSlave;
            });


            //更改扩展方式
            ocelotBuilder.Services.RemoveAll(typeof(IFileConfigurationRepository));
            //重写提取Ocelot配置信息
            ocelotBuilder.Services.AddSingleton(EFConfigurationProvider.Get);
            ocelotBuilder.Services.AddSingleton<IFileConfigurationRepository, EFConfigurationRepository>();
            return ocelotBuilder;
        }
    }
}
