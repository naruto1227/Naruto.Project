using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Naruto.Infrastructure.Interface;
using Naruto.Infrastructure.Extensions;
namespace Naruto.Infrastructure.Cache
{
    /// <summary>
    /// 缓存的处理
    /// </summary>
    public class CacheMemory : ICacheMemory
    {
        private IDistributedCache memoryCache;

        public CacheMemory(IDistributedCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }

        /// <summary>
        /// 新增一个缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task AddAsync<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            await memoryCache.SetStringAsync(key, value.ToJson(), options);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            await memoryCache.RemoveAsync(key);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var str = await memoryCache.GetStringAsync(key);
            if (str != null)
            {
                return str.ConvertTo<T>();
            }
            return default;
        }
    }
}
