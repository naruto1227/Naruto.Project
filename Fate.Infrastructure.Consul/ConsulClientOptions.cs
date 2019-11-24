using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Consul
{
    /// <summary>
    /// consul的配置
    /// </summary>
    public class ConsulClientOptions
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; } = "localhost";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 8500;

        /// <summary>
        /// Token is used to provide an ACL token which overrides the agent's default token. This ACL token is used for every request by
        /// clients created using this configuration.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 接口实例 http 或者 https
        /// </summary>
        public SchemeEnum Scheme { get; set; } = SchemeEnum.Http;

    }

    public enum SchemeEnum
    {
        Http,
        Https
    }
}
