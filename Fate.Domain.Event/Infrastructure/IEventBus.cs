using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Domain.Event.Infrastructure
{
    /// <summary>
    /// 事件总线继承的接口 实现依赖注入
    /// </summary>
    public interface IEventBus : IEventDependency
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandler"></param>
        Task Register<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : class, IEventData;

        /// <summary>
        /// 从程序集注册所有的事件
        /// </summary>
        Task RegisterAllFromAssembly();
        /// <summary>
        /// 清除所有的事件
        /// </summary>
        /// <returns></returns>
        Task ClearAllEvent();

        /// <summary>
        /// 处理失败的事件
        /// </summary>
        /// <returns></returns>
        Task HandleFailEvent();

        /// <summary>
        /// 取消注册
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handlerType"></param>

        Task UnRegister<TEvent>() where TEvent : class, IEventData;
        /// <summary>
        /// 触发
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlerType"></param>
        /// <param name="eventData"></param>

        Task Trigger<TEvent>(TEvent eventData) where TEvent : class, IEventData;

    }
}
