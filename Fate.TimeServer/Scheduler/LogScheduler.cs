using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Fate.TimeServer.Job;
using Quartz.Impl;

namespace Fate.TimeServer.Scheduler
{
    /// <summary>
    /// 日志的调度服务
    /// </summary>
    public class LogScheduler : ISchedulerServer
    {
        public IScheduler scheduler = default;
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public async Task ExecAsync()
        {
            //1. 创建Schedule
             scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            //2. 创建Job
            var job1 = JobBuilder.Create<LogJob>().Build();
            //3. 创建Trigger
            //5s执行一次，永远执行
            var trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();
            //2s执行一次，执行10次
            //var trigger = TriggerBuilder.Create()
            //                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).WithRepeatCount(10))
            //                            .Build();
            //注意这种用法：WithScheduler，表示1s执行一次，执行了5次
            //var trigger = TriggerBuilder.Create()
            //                            .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForTotalCount(5, 1))
            //                            .Build();
            //4. 开始调度
            await scheduler.ScheduleJob(job1, trigger);
            await scheduler.Start();
        }
    }
}
