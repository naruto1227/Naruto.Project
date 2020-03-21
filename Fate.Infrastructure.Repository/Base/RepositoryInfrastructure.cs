using Fate.Infrastructure.Repository.Interface;
using Fate.Infrastructure.Repository.Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

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
        private readonly DbContext masterDbContext;

        private readonly IRepositoryInfrastructureBase<TDbContext> infrastructureBase;

        /// <summary>
        /// 工作单元参数
        /// </summary>
        private readonly UnitOfWorkOptions<TDbContext> unitOfWorkOptions;

        private readonly IServiceProvider serviceProvider;

        public RepositoryWriteInfrastructure(IDbContextFactory _factory, IRepositoryInfrastructureBase<TDbContext> _infrastructureBase, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions, IServiceProvider _serviceProvider)
        {
            masterDbContext = _factory.GetMaster<TDbContext>();
            infrastructureBase = _infrastructureBase;
            unitOfWorkOptions = _unitOfWorkOptions;
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// 带返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult Exec<TResult>(Func<DbContext, TResult> action)
        {
            infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            return action(masterDbContext);
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        public void Exec(Action<DbContext> action)
        {
            infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            action(masterDbContext);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false);
            return await Exec(dbContext => dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false));
        }

        public int SaveChanges()
        {
            infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            return Exec(dbContext => dbContext.SaveChanges());
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            //验证是否开启事务
            if (unitOfWorkOptions.IsBeginTran)
                return Exec(dbContext => dbContext.Database.CurrentTransaction);
            //更改事务的状态
            unitOfWorkOptions.IsBeginTran = true;
            //切换配置
            await infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false);
            //切换从库的连接上下文
            await serviceProvider.GetRequiredService<IRepositoryReadInfrastructure<TDbContext>>().SwitchMasterDbContextAsync().ConfigureAwait(false);
            return await Exec(async dbContext =>
            {
                return await dbContext.Database.BeginTransactionAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            //验证是否开启事务
            if (unitOfWorkOptions.IsBeginTran)
                return Exec(dbContext => dbContext.Database.CurrentTransaction);
            //更改事务的状态
            unitOfWorkOptions.IsBeginTran = true;
            //切换配置
            infrastructureBase.SwitchAsync(masterDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            //切换从库的连接上下文
            serviceProvider.GetRequiredService<IRepositoryReadInfrastructure<TDbContext>>().SwitchMasterDbContextAsync().ConfigureAwait(false);
            return Exec(dbContext =>
             {
                 return dbContext.Database.BeginTransaction();
             });
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        public void CommitTransaction()
        {
            if (unitOfWorkOptions.IsBeginTran == false)
                throw new InvalidOperationException("当前事务未开启!");
            Exec(dbContext =>
            {
                dbContext.Database.CommitTransaction();
                unitOfWorkOptions.IsBeginTran = false;
            });
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBackTransaction()
        {
            if (unitOfWorkOptions.IsBeginTran == false)
                throw new InvalidOperationException("当前事务未开启!");
            Exec(dbContext =>
            {
                dbContext.Database.RollbackTransaction();
                unitOfWorkOptions.IsBeginTran = false;
            });
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
        private DbContext slaveDbContext;
        //上下文工厂
        private readonly IDbContextFactory factory;
        //基础设施
        private readonly IRepositoryInfrastructureBase<TDbContext> infrastructureBase;
        //工作单元参数信息
        private readonly UnitOfWorkOptions<TDbContext> unitOfWorkOptions;
        /// <summary>
        /// 是否为主库的上下文
        /// </summary>
        private bool IsMaster = false;
        public RepositoryReadInfrastructure(IDbContextFactory _factory, IRepositoryInfrastructureBase<TDbContext> _infrastructureBase, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions)
        {
            factory = _factory;
            infrastructureBase = _infrastructureBase;
            unitOfWorkOptions = _unitOfWorkOptions;
            //验证是否开启了事务 如果开启了事务 就获取主库的连接
            //或者没有开启读写分离 直接使用 主库的数据
            if (_unitOfWorkOptions.IsBeginTran || !_unitOfWorkOptions.IsOpenMasterSlave)
            {
                slaveDbContext = _factory.GetMaster<TDbContext>();
                IsMaster = true;
            }
            else
            {
                slaveDbContext = _factory.GetSlave<TDbContext>();
                _infrastructureBase.SwitchSlaveAsync(slaveDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// 带返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TResult Exec<TResult>(Func<DbContext, TResult> action)
        {
            infrastructureBase.SwitchAsync(slaveDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            return action(slaveDbContext);
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        public void Exec(Action<DbContext> action)
        {
            infrastructureBase.SwitchAsync(slaveDbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            action(slaveDbContext);
        }

        /// <summary>
        /// 切换主库的上下文
        /// </summary>
        /// <returns></returns>
        public Task SwitchMasterDbContextAsync()
        {
            if (!IsMaster && unitOfWorkOptions.IsOpenMasterSlave)
                slaveDbContext = factory.GetMaster<TDbContext>();
            IsMaster = true;
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 基础设施的底层方法
    /// </summary>
    public class RepositoryInfrastructureBase<TDbContext> : IRepositoryInfrastructureBase<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 工作单元参数
        /// </summary>
        private readonly UnitOfWorkOptions<TDbContext> unitOfWorkOptions;

        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly EFOptions options;

        public RepositoryInfrastructureBase(UnitOfWorkOptions<TDbContext> _unitOfWorkOptions, IServiceProvider serviceProvider)
        {
            unitOfWorkOptions = _unitOfWorkOptions;
            options = serviceProvider.GetService(MergeNamedType.Get(_unitOfWorkOptions.DbContextType.Name)) as EFOptions;
        }

        public async Task SwitchAsync(DbContext dbContext)
        {
            await SwitchDataBaseAsync(dbContext).ConfigureAwait(false);
            await SwitchCommandTimeoutAsync(dbContext).ConfigureAwait(false);
        }
        public async Task SwitchSlaveAsync(DbContext dbContext)
        {
            //验证是否开启读写分离
            //如果当前没有开启事务 并且 当前为主库的话 则 更改连接字符串为 从库的 
            if (unitOfWorkOptions.IsOpenMasterSlave && unitOfWorkOptions.IsBeginTran == false)
            {
                var connec = dbContext.Database.GetDbConnection();
                //关闭连接
                await ChangeConnecState(connec, ConnectionState.Closed).ConfigureAwait(false);
                //更改连接
                connec.ConnectionString = SlaveConnection(unitOfWorkOptions.DbContextType);
                //开启连接
                await ChangeConnecState(connec, ConnectionState.Open).ConfigureAwait(false);
                //检查配置的更改
                await SwitchAsync(dbContext).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 切换库
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public async Task SwitchDataBaseAsync(DbContext dbContext)
        {
            //判断是否需要切库
            if (!string.IsNullOrWhiteSpace(unitOfWorkOptions.ChangeDataBaseName))
            {
                //验证当前的库是否相等
                if (!dbContext.Database.GetDbConnection().Database.ToLower().Equals(unitOfWorkOptions.ChangeDataBaseName))
                {
                    //验证连接是否打开
                    await ChangeConnecState(dbContext.Database.GetDbConnection(), ConnectionState.Open).ConfigureAwait(false);
                    await dbContext.Database.GetDbConnection().ChangeDatabaseAsync(unitOfWorkOptions.ChangeDataBaseName).ConfigureAwait(false);
                }
            }
        }

        private Task SwitchCommandTimeoutAsync(DbContext dbContext)
        {
            //判断是否需要更改超时时间
            if (unitOfWorkOptions.CommandTimeout != null)
            {
                if (dbContext.Database.GetCommandTimeout() != unitOfWorkOptions.CommandTimeout)
                {
                    dbContext.Database.SetCommandTimeout(unitOfWorkOptions.CommandTimeout);
                }
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 更改连接的状态
        /// </summary>
        private async Task ChangeConnecState(DbConnection connec, ConnectionState connectionState)
        {
            //验证是否关闭连接
            if (connectionState == ConnectionState.Closed && connec.State == ConnectionState.Open)
            {
                connec.Close();
                return;
            }
            //验证是否开启连接
            if (connectionState == ConnectionState.Open && connec.State == ConnectionState.Closed)
            {
                await connec.OpenAsync().ConfigureAwait(false);
                return;
            }
        }
        /// <summary>
        /// 获取从库的连接字符串( 读取规则，当所有的从库无法使用的时候读取返回主库的连接字符串)
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        private string SlaveConnection(Type dbContextType)
        {
            //获取从库的信息
            var slaveList = SlavePools.slaveConnec.Where(a => a.Key == dbContextType).Select(a => a.Value).FirstOrDefault();

            SlaveDbConnection slaveInfo = null;
            if (slaveList != null && slaveList.Count() > 0)
            {
                slaveInfo = slaveList.Where(a => a.IsAvailable).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            }

            if (slaveInfo == null)
            {
                return options.WriteReadConnectionString;
            }
            //进行心跳检查
            var isBeat = SlavePools.HeartBeatCheck(dbContextType, slaveInfo);
            if (isBeat == false)
            {
                return SlaveConnection(dbContextType);
            }
            return slaveInfo.ConnectionString;
        }
    }
}
