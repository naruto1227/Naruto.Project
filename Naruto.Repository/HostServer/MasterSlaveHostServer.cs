using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Naruto.Repository.Object;

namespace Naruto.Repository.HostServer
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 后台服务 检查从库的状态
    /// </summary>
    internal class MasterSlaveHostServer : IHostedService
    {

        private ILogger<MasterSlaveHostServer> logger;

        private Timer timer;

        public MasterSlaveHostServer( ILogger<MasterSlaveHostServer> _logger)
        {
            logger = _logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("启动服务");
            //2分钟执行一次
            timer = new Timer(Handler, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("服务停止");
            if (timer != null)
            {
                logger.LogInformation("停止定时器");
                timer?.Change(Timeout.Infinite, 0);
                timer.Dispose();
            }
            return Task.CompletedTask;
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
    }
}
