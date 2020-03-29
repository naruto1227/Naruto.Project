using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.AutofacDependencyInjection
{
    /// <summary>
    /// autofac生命周期的三种形式
    /// </summary>
    public enum LiftTimeEnum
    {
        /// <summary>
        /// 同一个Lifetime生成的对象是同一个实例
        /// </summary>
        InstancePerLifetimeScope,
        /// <summary>
        /// 单例模式
        /// </summary>
        SingleInstance,
        /// <summary>
        /// 每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
        /// </summary>
        InstancePerDependency
    }
}
