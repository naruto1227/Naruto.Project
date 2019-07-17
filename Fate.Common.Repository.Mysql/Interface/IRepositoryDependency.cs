using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Repository.Mysql.Interface
{
    /// <summary>
    /// 所有仓储需要继承的接口(不做任何实现)  实现依赖注入
    /// 接口的命名规则 是在其实现类的基础名称前加I
    /// 此接口不要轻易更改名字
    /// </summary>
    public interface IRepositoryDependency
    {
    }
}
