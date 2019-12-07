using Fate.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Repository.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读写操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class RepositoryWriteInfrastructure<TDbContext> : IRepositoryWriteInfrastructure<TDbContext> where TDbContext : DbContext
    {
        //获取仓储上下文
        private DbContext dbContext;

        public RepositoryWriteInfrastructure(IDbContextFactory _factory)
        {
            dbContext = _factory.Get<TDbContext>();
        }
        /// <summary>
        /// 带返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult Exec<TResult>(Func<DbContext, TResult> action)
        {
            return action(dbContext);
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        public void Exec(Action<DbContext> action)
        {
            action(dbContext);
        }
    }

    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class RepositoryReadInfrastructure<TDbContext> : IRepositoryReadInfrastructure<TDbContext> where TDbContext : DbContext
    {
        //获取仓储上下文
        private DbContext dbContext;

        public RepositoryReadInfrastructure(IDbContextFactory _factory)
        {
            dbContext = _factory.Get<TDbContext>();
        }
        /// <summary>
        /// 带返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult Exec<TResult>(Func<DbContext, TResult> action)
        {
            return action(dbContext);
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        public void Exec(Action<DbContext> action)
        {
            action(dbContext);
        }
    }
}
