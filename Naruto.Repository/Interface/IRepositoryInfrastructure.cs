using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读写操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryWriteInfrastructure<TDbContext> : IRepositoryInfrastructure, IRepositoryDependency where TDbContext : DbContext
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 开启事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚
        /// </summary>
        void RollBackTransaction();
    }

    /// <summary>
    /// 张海波
    /// 2019-12-7
    /// 仓储的读操作
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryReadInfrastructure<TDbContext> : IRepositoryInfrastructure, IRepositoryDependency where TDbContext : DbContext
    {
        /// <summary>
        /// 切换主库的上下文
        /// </summary>
        /// <returns></returns>
        Task SwitchMasterDbContextAsync();
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

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<TResult> ExecAsync<TResult>(Func<DbContext, TResult> action);
        /// <summary>
        /// 执行操作 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task ExecAsync(Action<DbContext> action);
    }


    /// <summary>
    /// 张海波
    /// 2019-12-29
    /// 仓储的基础设施的底层操作
    /// </summary>
    public interface IRepositoryInfrastructureBase<TDbContext> : IRepositoryDependency where TDbContext : DbContext
    {
        Task SwitchDataBaseAsync(DbContext dbContext);
        /// <summary>
        /// 切换上下文配置
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        Task SwitchAsync(DbContext dbContext);
        /// <summary>
        /// 切换从库
        /// </summary>
        /// <returns></returns>
        Task SwitchSlaveAsync(DbContext dbContext);
    }
}
