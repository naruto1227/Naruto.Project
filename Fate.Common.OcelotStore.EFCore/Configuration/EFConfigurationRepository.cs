using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Fate.Common.OcelotStore.Redis
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

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="cache"></param>
        public EFConfigurationRepository(IOcelotCache<FileConfiguration> cache, IOptions<CacheOptions> _options)
        {
            _cache = cache;
            options = _options;
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
                fileConfiguration = _cache.Get(options.Value.CacheKey, default);
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
                _cache.AddAndDelete(options.Value.CacheKey, fileConfiguration, TimeSpan.FromHours(6), "");
            }
            return Task.FromResult((Response)new OkResponse());
        }
    }
}
