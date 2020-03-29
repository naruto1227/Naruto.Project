using Consul;
using Naruto.Consul.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Consul.ServiceRegister
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 服务注册的操作管理
    /// </summary>
    public interface IServiceRegisterManage
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <returns></returns>
        Task<WriteResult> ServiceRegister(RegisterConfiguration registerConfiguration);

        /// <summary>
        /// 取消服务
        /// </summary>
        /// <param name="serverId">服务id</param>
        /// <returns></returns>
        Task<WriteResult> ServiceDeregister(string serverId);
    }
}
