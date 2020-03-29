using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Domain.Event.Infrastructure
{
    /// <summary>
    /// 事件数据的公共类
    /// </summary>
    public abstract class EventData : IEventData
    {
        /// <summary>
        /// 事件的发生事件
        /// </summary>
        public DateTime EventTime { get; set; }
        /// <summary>
        /// 事件id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 是否执行失败 true 事件执行失败 false 事件执行成功
        /// </summary>
        public bool IsFail { get; set; }

        public EventData()
        {
            this.EventTime = DateTime.Now;
            this.Id = Guid.NewGuid();
            //默认成功
            IsFail = false;
        }
    }
}
