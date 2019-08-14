using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Fate.Common.Repository.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;

namespace Fate.Common.OcelotStore.EFCore
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 实现从Redis操作配置信息
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
                _cache.AddAndDelete(options.Value.CacheKey, fileConfiguration, TimeSpan.FromHours(6), options.Value.CacheKey);
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
                var info = unitOfWork.Respositiy<OcelotConfiguration>().Where(a => 1 == 1).OrderBy(a => a.Id).FirstOrDefault();

                return JsonConvert.DeserializeObject<FileConfiguration>(info.Config);
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
                var info = unitOfWork.Respositiy<OcelotConfiguration>().Where(a => 1 == 1).OrderBy(a => a.Id).FirstOrDefault();
                if (info != null)
                {
                    unitOfWork.Respositiy<OcelotConfiguration>().Update(a => a.Id == info.Id, (item) =>
                    {
                        item.Config = JsonConvert.SerializeObject(fileConfiguration);
                        return item;
                    });
                }
                else
                    unitOfWork.Respositiy<OcelotConfiguration>().Add(new OcelotConfiguration() { Config = JsonConvert.SerializeObject(fileConfiguration), Id = Guid.NewGuid().ToString().Replace("-", "") });

                unitOfWork.SaveChanges();
            }
        }
    }
}
