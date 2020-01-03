using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fate.Infrastructure.BaseRepository.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fate.Infrastructure.Repository.Interface;
using Microsoft.Extensions.Options;
using Fate.Infrastructure.Repository.Object;
using System.Collections.Generic;
using System.Diagnostics;
using Fate.Infrastructure.Repository.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

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
        /// 当前上下文
        /// </summary>
        public readonly Lazy<DbContext> dbContext;

        /// <summary>
        /// 工作单元参数
        /// </summary>
        private UnitOfWorkOptions unitOfWorkOptions;

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider service;

        /// <summary>
        /// 上下文事务
        /// </summary>
        private IDbContextTransaction dbContextTransaction;


        #endregion


        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="_service"></param>
        public UnitOfWork(IOptions<List<EFOptions>> _options, IServiceProvider _service, UnitOfWorkOptions _unitOfWorkOptions, IDbContextFactory _repositoryFactory)
        {
            unitOfWorkOptions = _unitOfWorkOptions;
            //获取上下文类型
            unitOfWorkOptions.DbContextType = typeof(TDbContext);
            //获取上下文
            var _dbContext = _service.GetService(unitOfWorkOptions.DbContextType) as DbContext;
            //获取当前的上下文
            dbContext = new Lazy<DbContext>(() => _dbContext);
            //获取主库的连接
            var dbInfo = _options.Value.Where(a => a.DbContextType == unitOfWorkOptions.DbContextType).FirstOrDefault();
            unitOfWorkOptions.WriteReadConnectionString = dbInfo?.WriteReadConnectionString;
            //是否开启读写分离操作
            unitOfWorkOptions.IsOpenMasterSlave = dbInfo.IsOpenMasterSlave;

            //设置上下文工厂
            _repositoryFactory.Set(unitOfWorkOptions?.DbContextType, dbContext?.Value);
            service = _service;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int CommandTimeout
        {
            set
            {
                dbContext.Value.Database.SetCommandTimeout(value);
                unitOfWorkOptions.CommandTimeout = value;
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            dbContextTransaction = infrastructureBase.BeginTransaction();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            dbContextTransaction = await infrastructureBase.BeginTransactionAsync();
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
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            var infrastructureBase = service.GetRequiredService<IRepositoryWriteInfrastructure<TDbContext>>();
            infrastructureBase.RollBackTransaction();
        }

        /// <summary>
        /// 强制更改为只读或者读写连接字符串
        /// </summary>
        /// <returns></returns>
        public async Task ChangeReadOrWriteConnection(ReadWriteEnum readWriteEnum = ReadWriteEnum.Read)
        {
            ////验证是否开启读写分离
            //if (unitOfWorkOptions.IsOpenMasterSlave == false)
            //    throw new ApplicationException("当前上下文未开启读写分离服务!");
            //if (unitOfWorkOptions.IsSumbitTran)
            //    throw new ApplicationException("无法在开启事务的时候执行读写库的更改!");

            ////获取连接信息
            //var connec = dbContext.Value.Database.GetDbConnection();
            ////关闭连接
            //await ChangeConnecState(connec, ConnectionState.Closed);
            ////读写
            //if (readWriteEnum == ReadWriteEnum.ReadWrite)
            //{
            //    //更改连接字符串
            //    connec.ConnectionString = unitOfWorkOptions.WriteReadConnectionString;
            //}
            ////只读
            //else if (readWriteEnum == ReadWriteEnum.Read)
            //{
            //    connec.ConnectionString = SlaveConnection(unitOfWorkOptions.DbContextType);
            //}
            ////打开连接
            //await ChangeConnecState(connec, ConnectionState.Open);
            ////连接更改重新验证是否更改了数据库名
            //await ChangeDataBase();
            //unitOfWorkOptions.IsMandatory = true;
        }

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync()
        {
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

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryQuery<T> Query<T>() where T : class, IEntity => service.GetService<IRepositoryQuery<T, TDbContext>>();

        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryCommand<T> Command<T>() where T : class, IEntity => service.GetService<IRepositoryCommand<T, TDbContext>>();
        /// <summary>
        /// 更改数据库的名字
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDataBase(string dataBase)
        {
            if (unitOfWorkOptions.IsSumbitTran)
                throw new ApplicationException("无法在事务中更改数据库!");
            var infrastructureBase = service.GetRequiredService<IRepositoryInfrastructureBase>();

            unitOfWorkOptions.ChangeDataBaseName = dataBase;

            await infrastructureBase.SwitchDataBase(dbContext.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行sql查询操作
        /// </summary>
        /// <returns></returns>
        public ISqlQuery SqlQuery() => service.GetService<ISqlQuery<TDbContext>>();

        /// <summary>
        /// 执行sql增删改操作
        /// </summary> 
        /// <returns></returns>
        public ISqlCommand SqlCommand() => service.GetService<ISqlCommand<TDbContext>>();
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            dbContext.Value?.Dispose();
            dbContextTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
