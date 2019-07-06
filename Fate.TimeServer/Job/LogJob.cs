using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Fate.TimeServer.Job
{
    /// <summary>
    /// 日志的作业任务
    /// </summary>
    public class LogJob : IJob
    {
        private ILogger<LogJob> logger;

        public LogJob(ILogger<LogJob> _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// 需要执行的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                logger.LogInformation("1");
            });
        }
    }
}
