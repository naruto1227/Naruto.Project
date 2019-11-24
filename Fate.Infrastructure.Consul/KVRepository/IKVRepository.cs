using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Consul.KVRepository
{
    /// <summary>
    /// 张海波
    /// 2019-08-23
    /// 访问consul的存储的仓储
    /// </summary>
    public interface IKVRepository
    {
        /// <summary>
        /// 推送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> PutAsync(string key, string value);

        /// <summary>
        /// 推送数据 根据参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<bool> PutAsync(string key, string value, WriteOptions option);
        /// <summary>
        /// 获取数据 根据参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key, QueryOptions option);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key);
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key, WriteOptions option);
    }
}
