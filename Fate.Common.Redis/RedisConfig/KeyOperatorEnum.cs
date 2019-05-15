using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Redis
{
    /// <summary>
    /// 存储操作redis key的类型 
    /// </summary>
    public enum KeyOperatorEnum
    {
        /// <summary>
        /// 字符串类型
        /// </summary>
        STRING,
        /// <summary>
        /// list类型
        /// </summary>
        LIST,
        /// <summary>
        /// set 类型
        /// </summary>
        SET,
    }
}
