using Naruto.BaseRepository.Model;
using Naruto.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Repository.Base
{
    /// <summary>
    /// 仓储的中介者实现
    /// </summary>
    public class RepositoryMediator<TDbContext> : IRepositoryMediator<TDbContext> where TDbContext : DbContext
    {
        private readonly IServiceProvider service;

        public RepositoryMediator(IServiceProvider _serviceProvider)
        {
            service = _serviceProvider;
        }

        #region 事务操作

        /// <summary>
        /// 开始事务
        /// </summary>
        public IDbContextTransaction BeginTransaction()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            return infrastructureBase.BeginTransaction();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            return infrastructureBase.BeginTransactionAsync();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            infrastructureBase.CommitTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            infrastructureBase.CommitTransaction();
            return Task.CompletedTask;
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            infrastructureBase.RollBackTransaction();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            infrastructureBase.RollBackTransaction();
            return Task.CompletedTask;
        }

        #endregion

        #region EFCore 保存

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            return await infrastructureBase.SaveChangesAsync();
        }
        /// <summary>
        /// 同步提交
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            return infrastructureBase.SaveChanges();
        }

        #endregion

        #region EFCore仓储 

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <param name="isMaster">是否访问主库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryQuery<T> Query<T>(bool isMaster = false) where T : class, IEntity
        {
            if (isMaster)
                return service.GetService<IRepositoryMasterQuery<T, TDbContext>>();
            else
                return service.GetService<IRepositoryQuery<T, TDbContext>>();
        }
        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryCommand<T> Command<T>() where T : class, IEntity => service.GetService<IRepositoryCommand<T, TDbContext>>();

        #endregion

        #region  ado操作

        /// <summary>
        /// 执行sql查询操作
        /// </summary>
        /// <param name="isMaster">是否在主库上执行</param>
        /// <returns></returns>
        public ISqlQuery SqlQuery(bool isMaster = false)
        {
            if (isMaster)
                return service.GetService<ISqlMasterQuery<TDbContext>>();
            else
                return service.GetService<ISqlQuery<TDbContext>>();
        }

        /// <summary>
        /// 执行sql增删改操作
        /// </summary> 
        /// <returns></returns>
        public ISqlCommand SqlCommand() => service.GetService<ISqlCommand<TDbContext>>();

        #endregion
    }
}
