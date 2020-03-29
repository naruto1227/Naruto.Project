using System;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Configuration.Repository;

using Naruto.OcelotStore.EFCore;
using Naruto.Repository.UnitOfWork;
using Naruto.Repository;
using Naruto.Repository.Object;

using Ocelot.DependencyInjection;
using Naruto.OcelotStore.Entity;
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
        public static IOcelotBuilder AddOcelotEFCache(this IOcelotBuilder ocelotBuilder, Action<CacheOptions> options)
        {
            //获取数据
            CacheOptions cacheOptions = new CacheOptions();
            options?.Invoke(cacheOptions);

            OcelotEFOption eFOptions = new OcelotEFOption();
            cacheOptions?.EFOptions.Invoke(eFOptions);

            #region  注入仓储

            ocelotBuilder.Services.AddRepository();

            ocelotBuilder.Services.AddEFOption(ocelot =>
            {
                ocelot.ConfigureDbContext = eFOptions.ConfigureDbContext;
                ocelot.ReadOnlyConnectionString = eFOptions.ReadOnlyConnectionString;
                //
                ocelot.UseEntityFramework<OcelotDbContent, SlaveOcelotDbContent>();
                ocelot.IsOpenMasterSlave = eFOptions.IsOpenMasterSlave;
            });

            #endregion

            //更改扩展方式
            ocelotBuilder.Services.RemoveAll(typeof(IFileConfigurationRepository));
            //重写提取Ocelot配置信息
            ocelotBuilder.Services.AddSingleton(EFConfigurationProvider.Get);
            ocelotBuilder.Services.AddSingleton<IFileConfigurationRepository, EFConfigurationRepository>();
            return ocelotBuilder;
        }
    }
}
