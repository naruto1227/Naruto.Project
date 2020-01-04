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
        private readonly DbContext dbContext;

        private readonly IRepositoryInfrastructureBase infrastructureBase;

        /// <summary>
        /// 工作单元参数
        /// </summary>
        private readonly UnitOfWorkOptions unitOfWorkOptions;

        private readonly IServiceProvider serviceProvider;

        public RepositoryWriteInfrastructure(IDbContextFactory _factory, IRepositoryInfrastructureBase _infrastructureBase, UnitOfWorkOptions _unitOfWorkOptions, IServiceProvider _serviceProvider)
        {
            dbContext = _factory.GetMaster<TDbContext>();
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
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            await infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false);
            return await Exec(dbContext => dbContext.SaveChangesAsync().ConfigureAwait(false));
        }

        public int SaveChanges()
        {
            infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            return Exec(dbContext => dbContext.SaveChanges());
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            //更改事务的状态
            unitOfWorkOptions.IsBeginTran = true;
            //切换配置
            await infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false);
            //切换从库的连接上下文
            await serviceProvider.GetRequiredService<IRepositoryReadInfrastructure<TDbContext>>().SwitchMasterDbContextAsync().ConfigureAwait(false);
            return await Exec(async dbContext =>
            {
                return await dbContext.Database.BeginTransactionAsync();
            });
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            //更改事务的状态
            unitOfWorkOptions.IsBeginTran = true;
            //切换配置
            infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
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
        private DbContext dbContext;
        private readonly IDbContextFactory factory;
        private readonly IRepositoryInfrastructureBase infrastructureBase;
        private readonly UnitOfWorkOptions unitOfWorkOptions;
        /// <summary>
        /// 是否为主库的上下文
        /// </summary>
        private bool IsMaster = false;
        public RepositoryReadInfrastructure(IDbContextFactory _factory, IRepositoryInfrastructureBase _infrastructureBase, UnitOfWorkOptions _unitOfWorkOptions)
        {
            factory = _factory;
            infrastructureBase = _infrastructureBase;
            unitOfWorkOptions = _unitOfWorkOptions;
            //验证是否开启了事务 如果开启了事务 就获取主库的连接
            //或者没有开启读写分离 直接使用 主库的数据
            if (_unitOfWorkOptions.IsBeginTran || !_unitOfWorkOptions.IsOpenMasterSlave)
            {
                dbContext = _factory.GetMaster<TDbContext>();
                IsMaster = true;
            }
            else
            {
                dbContext = _factory.GetSlave<TDbContext>();
                _infrastructureBase.SwitchSlaveAsync(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
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
            infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            return action(dbContext);
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="action"></param>
        public void Exec(Action<DbContext> action)
        {
            infrastructureBase.SwitchAsync(dbContext).ConfigureAwait(false).GetAwaiter().GetResult();
            action(dbContext);
        }

        /// <summary>
        /// 切换主库的上下文
        /// </summary>
        /// <returns></returns>
        public Task SwitchMasterDbContextAsync()
        {
            if (!IsMaster && unitOfWorkOptions.IsOpenMasterSlave)
                dbContext = factory.GetMaster<TDbContext>();
            IsMaster = true;
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 基础设施的底层方法
    /// </summary>
    public class RepositoryInfrastructureBase : IRepositoryInfrastructureBase
    {
        /// <summary>
        /// 工作单元参数
        /// </summary>
        private readonly UnitOfWorkOptions unitOfWorkOptions;

        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly IOptions<List<EFOptions>> options;

        public RepositoryInfrastructureBase(UnitOfWorkOptions _unitOfWorkOptions, IOptions<List<EFOptions>> _options)
        {
            unitOfWorkOptions = _unitOfWorkOptions;
            options = _options;
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
                return options.Value.Where(a => a.DbContextType == dbContextType).Select(a => a.WriteReadConnectionString).FirstOrDefault();
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
