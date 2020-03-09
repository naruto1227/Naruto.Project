using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// 代表标记的对象不会生成集合
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NoCollectionAttribute : Attribute
    {
    }
}
