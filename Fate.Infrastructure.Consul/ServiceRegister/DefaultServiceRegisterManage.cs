using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Fate.Infrastructure.Consul.Object;
using System.Linq;

namespace Fate.Infrastructure.Consul.ServiceRegister
{
    /// <summary>
    /// 张海波
    /// 2019-08-22
    /// 服务注册的操作管理
    /// </summary>
    public class DefaultServiceRegisterManage : IServiceRegisterManage
    {
        private readonly IConsulClient consulClient;

        private readonly IHostingEnvironment env;
        public DefaultServiceRegisterManage(IConsulClientFactory _consulClient, IOptions<ConsulClientOptions> option, IHostingEnvironment _env)
        {
            consulClient = _consulClient.Get(option?.Value);
            env = _env;
        }
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="registerConfiguration">需要注册的参数</param>
        /// <returns></returns>
        public Task<WriteResult> ServiceRegister(RegisterConfiguration registerConfiguration)
        {
            if (registerConfiguration == null)
                throw new ArgumentNullException(nameof(registerConfiguration));

            //健康检查
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                TCP = registerConfiguration.TcpHealthCheck?.ToString(),
                Status = HealthStatus.Passing,
                Timeout = TimeSpan.FromSeconds(5)
            };

            //服务id
            var serverId = "";

            if (!string.IsNullOrWhiteSpace(registerConfiguration.ServerId))
                serverId = registerConfiguration.ServerId;
            else
                serverId = env.ApplicationName + "-" + Guid.NewGuid().ToString().Replace("-", "");

            var registration = new AgentServiceRegistration()
            {
                Check = httpCheck,
                ID = serverId,
                Name = registerConfiguration.ServerName,
                Address = registerConfiguration.Address?.Address.ToString(),
                Port = registerConfiguration.Address.Port,
                Tags = registerConfiguration.Tags?.ToArray()//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };
            return consulClient.Agent.ServiceRegister(registration);
        }

        /// <summary>
        /// 取消服务
        /// </summary>
        /// <param name="serverId">服务id</param>
        /// <returns></returns>
        public Task<WriteResult> ServiceDeregister(string serverId)
        {
            return consulClient.Agent.ServiceDeregister(serverId);
        }
    }
}
