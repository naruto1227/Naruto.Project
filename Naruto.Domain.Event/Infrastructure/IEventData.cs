using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Domain.Event.Infrastructure
{
    /// <summary>
    /// 事件实体接口
    /// </summary>
    public interface IEventData : IEventDependency
    {
        /// <summary>
        /// 事件的发生的时间
        /// </summary>
        DateTime EventTime { get; set; }
        /// <summary>
        /// 事件的id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// 是否执行失败 true 事件执行失败 false 事件执行成功
        /// </summary>
        bool IsFail { get; set; }
    }
}
