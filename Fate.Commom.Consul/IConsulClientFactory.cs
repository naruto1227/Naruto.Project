using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Commom.Consul
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 获取客户端实例
    /// 注册为单例
    /// </summary>
    public interface IConsulClientFactory
    {
        /// <summary>
        /// 获取consul客户端
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IConsulClient Get(ConsulClientOptions configuration);
    }
}
