using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Fate.Commom.Consul.Object
{
    /// <summary>
    /// 服务注册的参数配置
    /// </summary>
    public class RegisterConfiguration
    {
        /// <summary>
        /// 服务名称(该名字用于服务发现时候的值)
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// tcp的健康检查地址
        /// </summary>
        public string TcpHealthCheck { get; set; }

        /// <summary>
        /// 服务发现时用于访问的地址
        /// </summary>
        public IPEndPoint Address { get; set; }

        public IEnumerable<string> Tags { get; set; }
        /// <summary>
        /// 服务id
        /// </summary>
        public string ServerId { get; set; }
    }
}
