using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fate.Common.Repository.Mysql.Base;

namespace Fate.Common.Repository.Mysql.HostServer
{
    internal class MasterSlaveHostServer : IHostedService
    {
        /// <summary>
        /// 应用程序的生命周期
        /// </summary>
        private IApplicationLifetime applicationLifetime;

        private ILogger<MasterSlaveHostServer> logger;

        private Timer timer;

        public MasterSlaveHostServer(IApplicationLifetime _applicationLifetime, ILogger<MasterSlaveHostServer> _logger)
        {
            applicationLifetime = _applicationLifetime;
            logger = _logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            applicationLifetime.ApplicationStarted.Register(Start);
            applicationLifetime.ApplicationStopped.Register(Stopped);
            applicationLifetime.ApplicationStopping.Register(Stopping);
            logger.LogInformation("启动服务");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("服务停止");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 启动
        /// </summary>
        private void Start()
        {
            //2分钟执行一次
            timer = new Timer(Handler, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
        }
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="state"></param>
        private void Handler(object state)
        {
            logger.LogInformation("执行定时器验证从库服务器的状态");
            SlavePools.TimerHeartBeatCheck();
        }
        private void Stopped()
        {
            if (timer != null)
            {
                logger.LogInformation("停止定时器");
                timer?.Change(Timeout.Infinite, 0);
                timer.Dispose();
            }
        }
        private void Stopping()
        {
        }
    }
}
