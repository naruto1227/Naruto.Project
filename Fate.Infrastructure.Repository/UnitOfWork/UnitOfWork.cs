using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fate.Infrastructure.BaseRepository.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Fate.Infrastructure.Repository.Interface;
using Microsoft.Extensions.Options;
using Fate.Infrastructure.Repository.Object;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;

namespace Fate.Infrastructure.Repository.UnitOfWork
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 工作单元的统一入口
    /// </summary>
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        #region  paramater

        /// <summary>
        /// 工作单元参数
        /// </summary>
        private UnitOfWorkOptions<TDbContext> unitOfWorkOptions;


        /// <summary>
        /// 上下文事务
        /// </summary>
        private IDbContextTransaction dbContextTransaction;

        /// <summary>
        /// 仓储的中介者对象
        /// </summary>
        private readonly IRepositoryMediator<TDbContext> repositoryMediator;

        #endregion


        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="_service"></param>
        public UnitOfWork(IOptions<List<EFOptions>> _options, IServiceProvider _service, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions, IDbContextFactory _repositoryFactory, IRepositoryMediator<TDbContext> _repositoryMediator)
        {
            unitOfWorkOptions = _unitOfWorkOptions;
            //获取上下文类型
            unitOfWorkOptions.DbContextType = typeof(TDbContext);
            //获取主库的连接
            var dbInfo = _options.Value.Where(a => a.DbContextType == unitOfWorkOptions.DbContextType).FirstOrDefault();

            unitOfWorkOptions.WriteReadConnectionString = dbInfo?.WriteReadConnectionString;
            //是否开启读写分离操作
            unitOfWorkOptions.IsOpenMasterSlave = dbInfo.IsOpenMasterSlave;
            //获取上下文
            var _dbContext = _service.GetService(unitOfWorkOptions.DbContextType) as DbContext;
            //设置上下文工厂
            _repositoryFactory.Set(unitOfWorkOptions?.DbContextType, _dbContext);

            repositoryMediator = _repositoryMediator;
        }

        #region 事务操作

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction() => dbContextTransaction = repositoryMediator.BeginTransaction();

        /// <summary>
        /// 开始事务
        /// </summary>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) => dbContextTransaction = await repositoryMediator.BeginTransactionAsync(cancellationToken);
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction() => repositoryMediator.CommitTransaction();
        /// <summary>
        /// 提交事务
        /// </summary>
        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            repositoryMediator.CommitTransactionAsync(cancellationToken);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction() => repositoryMediator.RollBackTransaction();
        /// <summary>
        /// 回滚事务
        /// </summary>
        public Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
        {
            repositoryMediator.RollBackTransactionAsync();
            return Task.CompletedTask;
        }

        #endregion

        #region EFCore 保存

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default) => await repositoryMediator.SaveChangeAsync(cancellationToken);
        /// <summary>
        /// 同步提交
        /// </summary>
        /// <returns></returns>
        public int SaveChanges() => repositoryMediator.SaveChanges();

        #endregion

        #region EFCore仓储 

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <param name="isMaster">是否访问主库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryQuery<T> Query<T>(bool isMaster = false) where T : class, IEntity => repositoryMediator.Query<T>(isMaster);
        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryCommand<T> Command<T>() where T : class, IEntity => repositoryMediator.Command<T>();

        #endregion

        #region  ado操作

        /// <summary>
        /// 执行sql查询操作
        /// </summary>
        /// <param name="isMaster">是否在主库上执行</param>
        /// <returns></returns>
        public ISqlQuery SqlQuery(bool isMaster = false) => repositoryMediator.SqlQuery(isMaster);

        /// <summary>
        /// 执行sql增删改操作
        /// </summary> 
        /// <returns></returns>
        public ISqlCommand SqlCommand() => repositoryMediator.SqlCommand();

        #endregion

        #region 切换数据库

        /// <summary>
        /// 更改数据库的名字
        /// </summary>
        /// <returns></returns>
        public Task ChangeDataBaseAsync(string dataBase)
        {
            if (unitOfWorkOptions.IsBeginTran)
                throw new ApplicationException("无法在事务中更改数据库!");
            unitOfWorkOptions.ChangeDataBaseName = dataBase;
            return Task.CompletedTask;
        }

        #endregion

        #region 超时时间设置

        /// <summary>
        /// 超时时间
        /// </summary>
        public int CommandTimeout
        {
            set
            {
                unitOfWorkOptions.CommandTimeout = value;
            }
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
           // dbContextTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
