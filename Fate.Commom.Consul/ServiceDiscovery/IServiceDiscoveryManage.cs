using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Commom.Consul.Object;

namespace Fate.Commom.Consul.ServiceDiscovery
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 服务发现的接口
    /// </summary>
    public interface IServiceDiscoveryManage
    {
        /// <summary>
        /// 服务发现 
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        Task<IEnumerable<Service>> ServerDiscovery(string serverName);

        /// <summary>
        /// 服务发现 
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <param name="tag">标签</param>
        /// <returns></returns>
        Task<IEnumerable<Service>> ServerDiscovery(string serverName, string tag);
    }
}
