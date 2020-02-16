using Fate.Infrastructure.BaseRepository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-01-15
    /// 仓储的中介接口
    /// </summary>
    public interface IRepositoryMediator<TDbContext> : IRepositoryDependency where TDbContext : DbContext
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 异步开始事务
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// 事务回滚
        /// </summary>
        Task RollBackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <param name="isMaster">是否访问主库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryQuery<T> Query<T>(bool isMaster = false) where T : class, IEntity;

        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryCommand<T> Command<T>() where T : class, IEntity;

        /// <summary>
        /// 返回sql查询的对象
        /// </summary>
        /// <param name="isMaster">是否在主库上执行</param>
        /// <returns></returns>
        ISqlQuery SqlQuery(bool isMaster = false);
        /// <summary>
        /// 返回sql增删改的对象
        /// </summary>
        /// <returns></returns>
        ISqlCommand SqlCommand();
    }
}
