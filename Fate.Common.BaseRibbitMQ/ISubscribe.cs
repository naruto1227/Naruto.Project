using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.CAP;

namespace Fate.Common.BaseRibbitMQ
{

    /// <summary>
    /// 订阅服务需要继承此接口实现依赖注入 方法也需要实现CapSubscribe特性指定给定的路由key
    /// </summary>
    public interface ISubscribe : ICapSubscribe
    {

    }
}
