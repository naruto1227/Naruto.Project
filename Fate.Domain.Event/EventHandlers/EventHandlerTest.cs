using Fate.Domain.Event.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Domain.Event.Events;
namespace Fate.Domain.Event.EventHandlers
{
    /// <summary>
    /// 定义一个测试的事件实现动作
    /// </summary>
    public class EventHandlerTest : IEventHandler<EventTest>
    {
        public Task Handle(EventTest @event)
        {
            @event.IsFail = true;
            Console.WriteLine("时间:" + @event.EventTime + ",事件：" + @event.Mesage);
            return Task.FromResult(0);
        }
    }
}
