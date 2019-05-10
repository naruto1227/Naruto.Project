using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Fate.Common.Cache
{
    /// <summary>
    /// 缓存的处理
    /// </summary>
    public class CacheMemory : Interface.ICacheMemory
    {
        private IMemoryCache memoryCache;

        public CacheMemory(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            memoryCache?.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 新增一个缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task AddAsync<T>(string key, T value)
        {
            memoryCache.Set(key, value);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取指定key的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(memoryCache.Get<T>(key));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            memoryCache.Remove(key);
            return Task.FromResult(0);
        }
    }
}
