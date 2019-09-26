using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Fate.Common.Mapper
{
    /// <summary>
    /// 自动化检测 获取 需要 mapper 的实体
    /// 张海波
    /// 2019-09-26
    /// </summary>
    internal interface IAutoInjectFactory
    {
        /// <summary>
        /// 从程序集 获取指定的需要注入到mapper的实体  组装实体返回
        /// </summary>
        /// <param name="assemblies"></param>
        IReadOnlyList<MapperOptions> GetFromAssemblys(params Type[] assemblies);
    }
}
