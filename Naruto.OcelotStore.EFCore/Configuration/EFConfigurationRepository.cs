using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Naruto.Repository.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Naruto.OcelotStore.Entity;
namespace Naruto.OcelotStore.EFCore
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 实现从数据库操作配置信息
    /// </summary>
    public class EFConfigurationRepository : IFileConfigurationRepository
    {
        /// <summary>
        /// 获取配置的缓存服务
        /// </summary>
        private readonly IOcelotCache<FileConfiguration> _cache;

        private static readonly object _lock = new object();

        /// <summary>
        /// 获取配置的参数
        /// </summary>
        private IOptions<CacheOptions> options;

        private IServiceProvider serviceProvider;
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="cache"></param>
        public EFConfigurationRepository(IOcelotCache<FileConfiguration> cache, IOptions<CacheOptions> _options, IServiceProvider _serviceProvider)
        {
            _cache = cache;
            options = _options;
            serviceProvider = _serviceProvider;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public Task<Response<FileConfiguration>> Get()
        {
            FileConfiguration fileConfiguration;
            lock (_lock)
            {
                fileConfiguration = _cache.Get(options.Value.CacheKey, options.Value.CacheKey);
                if (fileConfiguration == null)
                {
                    fileConfiguration = GetFileConfiguration();
                }
            }
            return Task.FromResult<Response<FileConfiguration>>(new OkResponse<FileConfiguration>(fileConfiguration));
        }
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="fileConfiguration"></param>
        /// <returns></returns>
        public Task<Response> Set(FileConfiguration fileConfiguration)
        {
            lock (_lock)
            {
                SetFileConfiguration(fileConfiguration);
                _cache.AddAndDelete(options.Value.CacheKey, fileConfiguration, TimeSpan.FromSeconds(120), options.Value.CacheKey);
            }
            return Task.FromResult((Response)new OkResponse());
        }
        /// <summary>
        /// 从数据库读取配置信息
        /// </summary>
        /// <returns></returns>
        private FileConfiguration GetFileConfiguration()
        {
            using (var services = serviceProvider.CreateScope())
            {
                //获取工作单元
                var unitOfWork = services.ServiceProvider.GetRequiredService<IUnitOfWork<OcelotDbContent>>();
                //从数据库读取
                var info = unitOfWork.Query<OcelotConfiguration>().Where(a => 1 == 1).OrderBy(a => a.Id).AsNoTracking().FirstOrDefault();
                return JsonConvert.DeserializeObject<FileConfiguration>("{'ReRoutes':"+info.ReRoutes+ ",'DynamicReRoutes':"+info.DynamicReRoutes+ ",'Aggregates':"+info.Aggregates+ ",'GlobalConfiguration':"+info.GlobalConfiguration+"}");
            }
        }

        /// <summary>
        ///设置配置信息
        /// </summary>
        /// <returns></returns>
        private void SetFileConfiguration(FileConfiguration fileConfiguration)
        {
            using (var services = serviceProvider.CreateScope())
            {
                //获取工作单元
                var unitOfWork = services.ServiceProvider.GetRequiredService<IUnitOfWork<OcelotDbContent>>();
                //从数据库读取
                var info = unitOfWork.Query<OcelotConfiguration>().Where(a => 1 == 1).OrderBy(a => a.Id).AsNoTracking().FirstOrDefault();
                if (info != null)
                {
                    unitOfWork.Command<OcelotConfiguration>().Update(a => a.Id == info.Id, (item) =>
                    {
                        item.ReRoutes = JsonConvert.SerializeObject(fileConfiguration.ReRoutes);
                        item.DynamicReRoutes = JsonConvert.SerializeObject(fileConfiguration.DynamicReRoutes);
                        item.GlobalConfiguration = JsonConvert.SerializeObject(fileConfiguration.GlobalConfiguration);
                        item.Aggregates = JsonConvert.SerializeObject(fileConfiguration.Aggregates);
                        return item;
                    });
                }
                else
                    unitOfWork.Command<OcelotConfiguration>().Add(new OcelotConfiguration() { ReRoutes = JsonConvert.SerializeObject(fileConfiguration.ReRoutes), Aggregates = JsonConvert.SerializeObject(fileConfiguration.Aggregates), DynamicReRoutes = JsonConvert.SerializeObject(fileConfiguration.DynamicReRoutes), GlobalConfiguration = JsonConvert.SerializeObject(fileConfiguration.GlobalConfiguration), Id = Guid.NewGuid().ToString().Replace("-", "") });

                unitOfWork.SaveChanges();
            }
        }
    }
}
