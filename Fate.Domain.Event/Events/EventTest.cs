using System;
using System.Collections.Generic;
using System.Text;
using Fate.Domain.Event.Infrastructure;

namespace Fate.Domain.Event.Events
{
    /// <summary>
    /// 一个测试的事件数据
    /// </summary>
    public class EventTest : EventData
    {
        public string Mesage { get; set; }
        public EventTest()
        {

        }
        /// <summary>
        /// 该构造方法是为了动态创建实例的时候 给属性赋值(目前仅用于处理失败的事件的时候)
        /// </summary>
        /// <param name="obj"></param>

        public EventTest(string obj)
        {
            if (!string.IsNullOrWhiteSpace(obj))
            {
                var info = Newtonsoft.Json.JsonConvert.DeserializeObject<EventTest>(obj);
                this.EventTime = info.EventTime;
                this.Id = info.Id;
                this.IsFail = info.IsFail;
                this.Mesage = info.Mesage;
            }
        }
    }
}
