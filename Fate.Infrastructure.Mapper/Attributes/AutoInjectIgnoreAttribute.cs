using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mapper.Attributes
{
    /// <summary>
    /// 忽略字段的标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoInjectIgnoreAttribute : Attribute
    {

    }
}
