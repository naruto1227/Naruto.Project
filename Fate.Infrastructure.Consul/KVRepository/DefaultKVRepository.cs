using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Fate.Infrastructure.Consul.Extensions;

namespace Fate.Infrastructure.Consul.KVRepository
{

    public class DefaultKVRepository : IKVRepository
    {
        private readonly IConsulClient consulClient;

        public DefaultKVRepository(IConsulClientFactory consulClientFactory, IOptions<ConsulClientOptions> options)
        {
            consulClient = consulClientFactory.Get(options.Value);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> PutAsync(string key, string value)
        {
            key.IsNull();
            value.IsNull();
            var res = await consulClient.KV.Put(new KVPair(key) { Key = key, Value = value.ToBytes() });
            return res.Response;
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> PutAsync(string key, string value, WriteOptions option)
        {
            key.IsNull();
            value.IsNull();
            var res = await consulClient.KV.Put(new KVPair(key) { Key = key, Value = value.ToBytes() }, option);
            return res.Response;
        }

        /// <summary>
        /// 获取数据 根据参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key, QueryOptions option)
        {
            key.IsNull();
            if (option == null)
                throw new ArgumentNullException(nameof(option));

            var res = await consulClient.KV.Get(key, option);
            return res?.Response?.Value.CoverToString();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            key.IsNull();
            var res = await consulClient.KV.Get(key);
            return res?.Response?.Value.CoverToString();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key)
        {
            key.IsNull();
            return (await consulClient.KV.Delete(key)).Response;
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key, WriteOptions option)
        {
            key.IsNull();
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            return (await consulClient.KV.Delete(key, option)).Response;
        }
    }
}
