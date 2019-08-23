using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Fate.Commom.Consul
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 获取客户端实例
    /// 注册为单例
    /// </summary>
    public class DefaultConsulClientFactory : IConsulClientFactory
    {
        //private readonly ObjectPool<ConsulClient> objectPool;

        public DefaultConsulClientFactory()
        {
            //objectPool = new DefaultObjectPool<ConsulClient>();
            //objectPool.Return(null);
        }
        /// <summary>
        /// 获取consul客户端
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IConsulClient Get(ConsulClientOptions configuration)
        {
            //if (objectPool.Get() != null)
            //{
            //    return objectPool.Get();
            //}
            var consulClient = new ConsulClient(option =>
             {
                 option.Address = new Uri(configuration.Scheme.ToString() + $"://{configuration.Host}:{configuration.Port}");
                 if (!string.IsNullOrWhiteSpace(configuration.Token))
                 {
                     option.Token = configuration.Token;
                 }
             });
            //objectPool.Return(consulClient);
            return consulClient;
        }
    }
}
