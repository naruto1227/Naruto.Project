using System;
using System.Collections.Generic;

using System.Text;

namespace Naruto.Mapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    /// <summary>
    /// 自动化注册dto 属性标记
    /// </summary>
    public class AutoInjectDtoAttribute : Attribute
    {
        /// <summary>
        /// 源类型
        /// </summary>
        public Type SoureType { get; set; }
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; set; }

        public bool ReverseMap { get; set; } = false;

    }
}
