using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Fate.TimeServer.Scheduler;

namespace Fate.TimeServer
{
    /// <summary>
    /// 启动调度器的服务
    /// </summary>
    public class StartSchedulerHostServer : IHostedService
    {
        /// <summary>
        /// 应用程序的生命周期
        /// </summary>
        private IApplicationLifetime applicationLifetime;
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<StartSchedulerHostServer> logger;
        private LogScheduler logScheduler;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="_applicationLifetime"></param>
        public StartSchedulerHostServer(IApplicationLifetime _applicationLifetime, ILogger<StartSchedulerHostServer> _logger, LogScheduler _logScheduler)
        {
            applicationLifetime = _applicationLifetime;
            logger = _logger;
            logScheduler = _logScheduler;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            applicationLifetime.ApplicationStarted.Register(Start);
            applicationLifetime.ApplicationStopped.Register(Stopped);
            applicationLifetime.ApplicationStopping.Register(Stopping);
            logger.LogInformation("start  HostServer");
            await logScheduler.ExecAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void Start() { }
        private void Stopped() { }
        private void Stopping() { }
    }
}
