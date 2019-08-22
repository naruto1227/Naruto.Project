using Consul;
using Fate.Commom.Consul.Object;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace Fate.Commom.Consul.ServiceDiscovery
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultServiceDiscoveryManage : IServiceDiscoveryManage
    {
        private readonly IConsulClient consulClient;

        public DefaultServiceDiscoveryManage(IConsulClientFactory _consulClient, IOptions<ConsulClientOptions> option)
        {
            consulClient = _consulClient.Get(option?.Value);
        }
        /// <summary>
        /// 服务发现 
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        public async Task<IEnumerable<Service>> ServerDiscovery(string serverName)
        {
            if (string.IsNullOrEmpty(serverName))
                throw new ArgumentNullException(nameof(serverName));
            //获取结果
            var result = await consulClient.Health.Service(serverName);
            if (result == null)
            {
                return default;
            }
            List<Service> services = new List<Service>();

            foreach (var item in result.Response)
            {
                Service service = new Service();
                service.Id = item.Service.ID;
                service.Name = item.Service.Service;
                service.Tags = item.Service.Tags;
                service.HostAndPort = new IPEndPoint(IPAddress.Parse(item.Service.Address), item.Service.Port);
                services.Add(service);
            }
            return services;
        }

        /// <summary>
        /// 服务发现 
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        public async Task<IEnumerable<Service>> ServerDiscovery(string serverName,string tag)
        {
            if (string.IsNullOrEmpty(serverName))
                throw new ArgumentNullException(nameof(serverName));
            //获取结果
            var result = await consulClient.Health.Service(serverName, tag);
            if (result==null)
            {
                return default;
            }
            List<Service> services = new List<Service>();

            foreach (var item in result.Response)
            {
                Service service = new Service();
                service.Id = item.Service.ID;
                service.Name = item.Service.Service;
                service.Tags = item.Service.Tags;
                service.HostAndPort = new IPEndPoint(IPAddress.Parse(item.Service.Address), item.Service.Port);
                services.Add(service);
            }
            return services;
        }
    }
}
