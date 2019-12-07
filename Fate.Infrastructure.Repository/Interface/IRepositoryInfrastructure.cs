using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读写操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryWriteInfrastructure<TDbContext> : IRepositoryInfrastructure, IRepositoryDependency where TDbContext : DbContext
    {


    }

    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryReadInfrastructure<TDbContext>:IRepositoryInfrastructure , IRepositoryDependency where TDbContext : DbContext
    {


    }

    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的基础设施操作
    /// </summary>
    public interface IRepositoryInfrastructure
    {
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        TResult Exec<TResult>(Func<DbContext, TResult> action);
        /// <summary>
        /// 执行操作 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        void Exec(Action<DbContext> action);
    }
}
