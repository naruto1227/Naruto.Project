using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-08-30
    /// 获取或者设置上下文
    /// </summary>
    public interface IRepositoryFactory : IRepositoryDependency
    {
        void Set(DbContext dbContext);

        DbContext Get();

    }
}
