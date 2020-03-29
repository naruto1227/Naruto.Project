//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using System.Linq;
//using System.Xml.Linq;
//using System.Collections.Concurrent;
//using Naruto.AutofacDependencyInjection;
//using System.Reflection;

//namespace Naruto.Domain.Event.Infrastructure
//{
//    /// <summary>
//    /// 事件总线
//    /// 发布与订阅处理逻辑 (最简单的事件总线 将事件存储到字典中)
//    /// 核心功能代码
//    /// </summary>
//    public class EventBus : IEventBus
//    {
//        private static readonly object sync = new object();
//        /// <summary>
//        /// 存放注册的事件
//        /// </summary>
//        public static ConcurrentDictionary<Type, List<object>> concurrentDictionary;
//        /// <summary>
//        ///  定义一个静态的实例 实现一个唯一的入口访问事件总线 来操作
//        /// </summary>
//        public static EventBus Default { get; set; }
//        /// <summary>
//        /// 静态构造初始一个实例
//        /// </summary>
//        static EventBus()
//        {
//            Default = new EventBus();
//        }
//        public EventBus()
//        {
//            concurrentDictionary = new ConcurrentDictionary<Type, List<object>>();
//        }
//        /// <summary>
//        /// 事件的触发
//        /// </summary>
//        /// <typeparam name="TEvent"></typeparam>
//        /// <param name="eventHandlerType"></param>
//        /// <param name="eventData"></param>
//        /// <returns></returns>
//        public Task Trigger<TEvent>(TEvent eventData) where TEvent : class, IEventData
//        {
//            //获取事件源需要触发的事件行为
//            List<object> handlerTypes = concurrentDictionary.Count() > 0 ? concurrentDictionary[typeof(TEvent)] : new List<object>();
//            if (handlerTypes.Count() > 0)
//            {
//                ExecAction(() =>
//                {
//                    ConcurrentDictionary<Type, List<object>> success = new ConcurrentDictionary<Type, List<object>>();
//                    foreach (var item in handlerTypes)
//                    {
//                        var handler = item as IEventHandler<TEvent>;
//                        if (handler != null)
//                        {
//                            //异步执行
//                            handler.Handle(eventData);
//                            //判断事件是否执行成功
//                            if (eventData.IsFail==false)
//                            {
//                                lock (sync)
//                                {
//                                    List<object> handlers = success.Count() > 0 ? success[typeof(TEvent)] : new List<object>();
//                                    handlers.Add(handler);
//                                    success[typeof(TEvent)] = handlers;
//                                }
//                            }
//                        }
//                    }
//                    //取消订阅
//                    if (success.Count() > 0)
//                    {
//                        foreach (var item in success)
//                        {
//                            foreach (var item2 in item.Value)
//                            {
//                                UnRegister(item2 as IEventHandler<TEvent>);
//                            }
//                        }
//                    }
//                });
//            }
//            return Task.FromResult(0);
//        }
//        /// <summary>
//        /// 取消注册事件
//        /// </summary>
//        /// <typeparam name="TEvent">事件实体</typeparam>
//        /// <param name="handlerType">事件的动作</param>
//        /// <returns></returns>
//        public Task UnRegister<TEvent>(IEventHandler<TEvent> handlerType) where TEvent : class, IEventData
//        {
//            lock (sync)
//            {
//                List<object> handlerTypes = concurrentDictionary.Count() > 0 ? concurrentDictionary[typeof(TEvent)] : new List<object>();
//                if (handlerTypes.Contains(handlerType))
//                {
//                    handlerTypes.Remove(handlerType);
//                    concurrentDictionary[typeof(TEvent)] = handlerTypes;
//                }
//            }
//            return Task.FromResult(0);
//        }
//        /// <summary>
//        /// 注册
//        /// </summary>
//        /// <typeparam name="TEvent">实体对象</typeparam>
//        /// <param name="eventHandler">需要做的事情</param>
//        /// <returns></returns>
//        public Task Register<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : class, IEventData
//        {
//            lock (sync)
//            {
//                List<object> handlerTypes = concurrentDictionary.Count() > 0 ? concurrentDictionary[typeof(TEvent)] : new List<object>();
//                if (!handlerTypes.Contains(eventHandler))
//                {
//                    handlerTypes.Add(eventHandler);
//                    concurrentDictionary[typeof(TEvent)] = handlerTypes;
//                }
//            }
//            return Task.FromResult(0);
//        }

//        /// <summary>
//        /// 执行委托事件
//        /// </summary>
//        /// <param name="action"></param>
//        private void ExecAction(Action action)
//        {
//            action();
//        }
//        /// <summary>
//        /// 从程序集注册所有事件 (可放在系统启动的时候 运行)
//        /// </summary>
//        /// <returns></returns>
//        public Task RegisterAllFromAssembly()
//        {
//            //获取事件总线所在的程序集
//            var assembly = Assembly.Load("Naruto.Domain.Event");
//            //获取当前程序集的所有的类型
//            if (assembly != null)
//            {
//                Type[] types = assembly.GetTypes();
//                if (types != null && types.Count() > 0)
//                {
//                    //获取当前继承的接口 并且当前为class
//                    foreach (var item in types.Where(a => a.GetInterface("IEventHandler") != null && a.IsClass && a.IsAbstract == false))
//                    {
//                        //获取方法
//                        var method = item.GetMethod("Handle");
//                        //获取方法的参数
//                        var dataType = method.GetParameters()[0].ParameterType;
//                        //获取执行的方法
//                        var eventHandler = item.GetConstructor(Type.EmptyTypes).Invoke(null);
//                        //将注册的事件写入线程集合
//                        lock (sync)
//                        {
//                            List<object> handlerTypes = concurrentDictionary.Count() > 0 ? concurrentDictionary[dataType] : new List<object>();
//                            if (!handlerTypes.Contains(eventHandler))
//                            {
//                                handlerTypes.Add(eventHandler);
//                                concurrentDictionary[dataType] = handlerTypes;
//                            }
//                        }
//                    }
//                }
//            }
//            return Task.FromResult(0);
//        }
//        /// <summary>
//        /// 清除所有的事件
//        /// </summary>
//        /// <returns></returns>
//        public Task ClearAllEvent()
//        {
//            concurrentDictionary = new ConcurrentDictionary<Type, List<object>> ();
//            return Task.FromResult(0);
//        }


//        /// <summary>
//        /// 处理失败的事件
//        /// </summary>
//        /// <returns></returns>
//        public Task HandleFailEvent() {
//            return Task.FromResult(0);
//        }
//    }
//}
