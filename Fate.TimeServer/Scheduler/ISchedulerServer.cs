using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fate.TimeServer.Scheduler
{
    /// <summary>
    /// 调度器的接口实现
    /// </summary>
    public interface ISchedulerServer
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        Task ExecAsync();
    }
}
