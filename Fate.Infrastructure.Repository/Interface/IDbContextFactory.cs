using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-08-30
    /// 获取或者设置上下文
    /// </summary>
    public interface IDbContextFactory : IRepositoryDependency
    {
        /// <summary>
        /// 设置上下文的类型
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <param name="dbContext"></param>
        void Set(Type DbContextType, DbContext dbContext);
        /// <summary>
        /// 获取指定的上下文 主库
        /// </summary> 
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        DbContext GetMaster<TDbContext>() where TDbContext : DbContext;

        /// <summary>
        /// 获取指定的上下文 从库
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        DbContext GetSlave<TDbContext>() where TDbContext : DbContext;

    }
}
