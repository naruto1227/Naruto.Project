using System;
using System.Collections.Generic;
using System.Text;
using Naruto.Domain.Event.Infrastructure;

namespace Naruto.Domain.Event.Events
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
                try
                {
                    var info = Newtonsoft.Json.JsonConvert.DeserializeObject<EventTest>(obj);
                    this.EventTime = info.EventTime;
                    this.Id = info.Id;
                    this.IsFail = info.IsFail;
                    this.Mesage = info.Mesage;
                }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}
