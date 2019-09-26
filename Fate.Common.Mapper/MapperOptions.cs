using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Mapper
{
    internal class MapperOptions
    {
        internal MapperOptions() { }
        internal MapperOptions(Type soureType, Type targetType)
        {
            SoureType = soureType;
            TargetType = targetType;
        }
        internal MapperOptions(Type soureType, Type targetType, object reverseMap)
        {
            SoureType = soureType;
            TargetType = targetType;

            if (reverseMap != null)
            {
                ReverseMap = bool.Parse(reverseMap.ToString());
            }
        }
        /// <summary>
        /// 源类型
        /// </summary>
        internal Type SoureType { get; set; }
        /// <summary>
        /// 目标类型
        /// </summary>
        internal Type TargetType { get; set; }

        /// <summary>
        /// 是否需要翻转 mapper 默认false
        /// </summary>
        internal bool ReverseMap { get; set; } = false;
        /// <summary>
        /// 需要忽略的字段的名字
        /// </summary>
        internal IReadOnlyList<string> IgnoreName { get; set; }
    }
}
