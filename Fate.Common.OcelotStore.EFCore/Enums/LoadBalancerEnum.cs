using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.OcelotStore.EFCore.Enums
{
    /// <summary>
    /// 负载均衡的加载方式
    /// </summary>
    public enum LoadBalancerEnum
    {
        /// <summary>
        /// 将请求发往最空闲的那个服务器
        /// </summary>
        LeastConnection,
        /// <summary>
        /// 轮训
        /// </summary>
        RoundRobin,
        /// <summary>
        /// 总是发往第一个请求或者是服务发现
        /// </summary>
        NoLoadBalancer
    }
}
