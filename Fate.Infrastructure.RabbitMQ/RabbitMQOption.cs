using System;

namespace Fate.Infrastructure.RabbitMQ
{
    /// <summary>
    /// 配置RabbitMQ的配置信息
    /// </summary>
    public class RabbitMQOption
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; } = "127.0.0.1";

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// 用户名
        /// </summary>
        public string Password { get; set; } = "guest";


    }
}
