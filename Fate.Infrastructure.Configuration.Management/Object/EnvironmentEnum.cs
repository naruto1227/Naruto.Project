using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Configuration.Management.Object
{
    /// <summary>
    /// 环境变量的枚举值
    /// </summary>
    public enum EnvironmentEnum
    {
        /// <summary>
        /// 测试环境
        /// </summary>
        Development = 0,
        /// <summary>
        /// 预发环境
        /// </summary>
        Staging = 1,
        /// <summary>
        /// 生产环境
        /// </summary>
        Production = 2
    }
}
