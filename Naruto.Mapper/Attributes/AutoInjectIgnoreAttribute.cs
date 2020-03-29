using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Mapper.Attributes
{
    /// <summary>
    /// 忽略字段的标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AutoInjectIgnoreAttribute : Attribute
    {

    }
}
