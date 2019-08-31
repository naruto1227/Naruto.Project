using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Base.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fate.Common.Repository.Interface;
using Microsoft.Extensions.Options;
using Fate.Common.Repository.Object;
using System.Collections.Generic;
using System.Diagnostics;
using Fate.Common.Repository.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Fate.Common.Repository.UnitOfWork
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
        /// 参数信息
        /// </summary>
        private IOptions<List<EFOptions>> options;
        /// <summary>
        /// 工作单元参数
        /// </summary>
        private UnitOfWorkOptions unitOfWorkOptions;
        /// <summary>
        /// 上下文工厂
        /// </summary>
        private IRepositoryFactory repositoryFactory;
        #endregion

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="_service"></param>
        public UnitOfWork(IOptions<List<EFOptions>> _options, IServiceProvider _service, UnitOfWorkOptions _unitOfWorkOptions, IRepositoryFactory _repositoryFactory)
        {
            options = _options;
            unitOfWorkOptions = _unitOfWorkOptions;
            //获取上下文类型
            unitOfWorkOptions.DbContextType = typeof(TDbContext);
            //获取当前的上下文
            dbContext = new Lazy<DbContext>(() => _service.GetService(unitOfWorkOptions.DbContextType) as DbContext);
            //获取主库的连接
            var dbInfo = _options.Value.Where(a => a.DbContextType == unitOfWorkOptions.DbContextType).FirstOrDefault();
            unitOfWorkOptions.WriteReadConnectionString = dbInfo?.WriteReadConnectionString;
            //是否开启读写分离操作
            unitOfWorkOptions.IsOpenMasterSlave = dbInfo.IsOpenMasterSlave;

            //设置上下文工厂
            repositoryFactory = _repositoryFactory;
            repositoryFactory.Set(dbContext?.Value);
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            //验证是否开启读写分离
            if (unitOfWorkOptions.IsOpenMasterSlave)
            {
                //验证当前是否为主库
                if (unitOfWorkOptions.IsSlaveOrMaster)
                {
                    //设置连接为主库
                    SetMasterConnection();
                }
                //更改事务的状态
                unitOfWorkOptions.IsSumbitTran = true;
            }
            dbContext.Value.Database.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            dbContext.Value.Database.CommitTransaction();
            //验证是否开启读写分离
            if (unitOfWorkOptions.IsOpenMasterSlave)
            {
                unitOfWorkOptions.IsSumbitTran = false;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            dbContext.Value.Database.RollbackTransaction();
            //验证是否开启读写分离
            if (unitOfWorkOptions.IsOpenMasterSlave)
            {
                unitOfWorkOptions.IsSumbitTran = false;
            }
        }

        /// <summary>
        /// 强制更改为只读或者读写连接字符串
        /// </summary>
        /// <returns></returns>
        public async Task ChangeReadOrWriteConnection(ReadWriteEnum readWriteEnum = ReadWriteEnum.Read)
        {
            //验证是否开启读写分离
            if (unitOfWorkOptions.IsOpenMasterSlave == false)
                throw new ApplicationException("当前上下文未开启读写分离服务!");
            if (unitOfWorkOptions.IsSumbitTran)
                throw new ApplicationException("无法在开启事务的时候执行读写库的更改!");
            await Task.Run(() =>
            {
                //获取连接信息
                var connec = dbContext.Value.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //读写
                if (readWriteEnum == ReadWriteEnum.ReadWrite)
                {
                    //更改连接字符串
                    connec.ConnectionString = unitOfWorkOptions.WriteReadConnectionString;
                }
                //只读
                else if (readWriteEnum == ReadWriteEnum.Read)
                {
                    connec.ConnectionString = SlaveConnection(unitOfWorkOptions.DbContextType);
                }
                //打开连接
                ChangeConnecState(connec, ConnectionState.Open);

                unitOfWorkOptions.IsMandatory = true;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync()
        {
            SetMaster();
            return await dbContext.Value.SaveChangesAsync();
        }
        /// <summary>
        /// 同步提交
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            SetMaster();
            return dbContext.Value.SaveChanges();
        }

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryQuery<T> Query<T>() where T : class, IEntity
        {
            SetSlave();

            IRepositoryQuery<T> repository = dbContext.Value.GetService<IRepositoryQuery<T>>();
            return repository;
        }

        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepositoryCommand<T> Command<T>() where T : class, IEntity
        {
            //SetSlaveConnec();
            IRepositoryCommand<T> repository = dbContext.Value.GetService<IRepositoryCommand<T>>();
            return repository;
        }
        /// <summary>
        /// 更改数据库的名字
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDataBase(string dataBase)
        {
            if (unitOfWorkOptions.IsSumbitTran)
                throw new ApplicationException("无法在事务中更改数据库!");
            await Task.Run(() =>
             {
                 //开启连接
                 ChangeConnecState(dbContext.Value.Database.GetDbConnection(), ConnectionState.Open);
                 dbContext.Value.Database.GetDbConnection().ChangeDatabase(dataBase);
                 unitOfWorkOptions.ChangeDataBaseName = dataBase;
             }).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行sql语句的 返回 受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] _params)
        {
            SetMaster();
            return await dbContext.Value.Database.ExecuteSqlCommandAsync(sql, _params);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            dbContext.Value?.Dispose();
            GC.SuppressFinalize(this);
        }

        #region base
        /// <summary>
        /// 设置从库的连接
        /// </summary>
        private void SetSlave()
        {
            //验证是否开启读写分离
            //如果当前没有开启事务 并且 当前为主库的话 则 更改连接字符串为 从库的 
            if (unitOfWorkOptions.IsOpenMasterSlave && unitOfWorkOptions.IsSumbitTran == false && unitOfWorkOptions.IsSlaveOrMaster == false && unitOfWorkOptions.IsMandatory == false)
            {
                var connec = dbContext.Value.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //更改连接
                connec.ConnectionString = SlaveConnection(unitOfWorkOptions.DbContextType);
                //开启连接
                ChangeConnecState(connec, ConnectionState.Open);
                //连接更改重新验证是否更改了数据库名
                ChangeDataBase();
                //更改连接的服务器为从库
                unitOfWorkOptions.IsSlaveOrMaster = true;
            }

        }
        /// <summary>
        /// 设置主库的 连接
        /// </summary>
        private void SetMaster()
        {
            //验证是否开启读写分离
            //如果当前没有开启事务 并且 当前为从库的话 则 更改连接字符串为 主库的 
            if (unitOfWorkOptions.IsOpenMasterSlave && unitOfWorkOptions.IsSumbitTran == false && unitOfWorkOptions.IsSlaveOrMaster && unitOfWorkOptions.IsMandatory == false)
            {
                //设置连接为主库
                SetMasterConnection();
                //更改连接的服务器为主库
                unitOfWorkOptions.IsSlaveOrMaster = false;
            }
        }
        /// <summary>
        /// 设置主库的连接
        /// </summary>
        private void SetMasterConnection()
        {
            var connec = dbContext.Value.Database.GetDbConnection();
            //关闭连接
            ChangeConnecState(connec, ConnectionState.Closed);
            //更改连接
            connec.ConnectionString = unitOfWorkOptions.WriteReadConnectionString;
            //开启连接
            ChangeConnecState(connec, ConnectionState.Open);
            //连接更改重新验证是否更改了数据库名
            ChangeDataBase();
        }
        /// <summary>
        /// 获取从库的连接字符串( 读取规则，当所有的从库无法使用的时候读取返回主库的连接字符串)
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        private string SlaveConnection(Type dbContextType)
        {
            //获取从库的信息
            var slaveInfo = SlavePools.slaveConnec.Where(a => a.Key == dbContextType).Select(a => a.Value).FirstOrDefault().Where(a => a.IsAvailable).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
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


        /// <summary>
        /// 更改连接的状态
        /// </summary>
        private void ChangeConnecState(IDbConnection connec, ConnectionState connectionState)
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
                connec.Open();
                return;
            }
        }

        /// <summary>
        /// 更改数据库的名字
        /// </summary>
        /// <returns></returns>
        private void ChangeDataBase()
        {
            if (!string.IsNullOrWhiteSpace(unitOfWorkOptions.ChangeDataBaseName))
            {
                ChangeConnecState(dbContext.Value.Database.GetDbConnection(), ConnectionState.Open);
                dbContext.Value.Database.GetDbConnection().ChangeDatabase(unitOfWorkOptions.ChangeDataBaseName);
            }
        }
        #endregion
    }
}
