using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Fate.Domain.Event.Infrastructure
{
    public interface IEventHandler
    {

    }

    /// <summary>
    /// 事件的处理接口（动作行为）
    /// </summary>
    /// <typeparam name="TEvent">继承事件实体的对象</typeparam>
    public interface IEventHandler<TEvent>: IEventHandler where TEvent : class, IEventData
    {
        /// <summary>
        /// 领域事件的处理的方法
        /// </summary>
        /// <param name="event"></param>
        Task Handle(TEvent @event);
    }


}
