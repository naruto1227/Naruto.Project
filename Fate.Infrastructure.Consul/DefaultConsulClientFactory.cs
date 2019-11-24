using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;
using System.Linq;

namespace Fate.Infrastructure.Consul
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 获取客户端实例
    /// 注册为单例
    /// </summary>
    public class DefaultConsulClientFactory : IConsulClientFactory
    {

        private readonly ConcurrentQueue<ConsulClient> consulClientsQueue;

        public DefaultConsulClientFactory()
        {
            consulClientsQueue = new ConcurrentQueue<ConsulClient>();
        }
        /// <summary>
        /// 获取consul客户端
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IConsulClient Get(ConsulClientOptions configuration)
        {
            if (consulClientsQueue.Count() > 0)
            {
                return consulClientsQueue.FirstOrDefault();
            }
            var consulClient = new ConsulClient(option =>
             {
                 option.Address = new Uri(configuration.Scheme.ToString() + $"://{configuration.Host}:{configuration.Port}");
                 if (!string.IsNullOrWhiteSpace(configuration.Token))
                 {
                     option.Token = configuration.Token;
                 }
             });
            consulClientsQueue.Enqueue(consulClient);
            return consulClient;
        }
    }
}
