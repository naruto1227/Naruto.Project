using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fate.Domain.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fate.Common.Repository.Mysql.Interface;
using Microsoft.Extensions.Options;
using Fate.Common.Repository.Mysql.Base;
using System.Collections.Generic;
using System.Diagnostics;
using Fate.Common.Repository.Mysql.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Fate.Common.Repository.Mysql.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        #region  paramater
        /// <summary>
        /// 当前上下文
        /// </summary>
        private readonly DbContext dbContext;
        /// <summary>
        /// 参数信息
        /// </summary>
        private IOptions<List<EFOptions>> options;
        /// <summary>
        /// master 库的连接字符串
        /// </summary>
        private string WriteReadConnectionString;
        /// <summary>
        /// 上下文的类型
        /// </summary>
        private Type dbContextType;
        /// <summary>
        /// 是否提交事务
        /// </summary>
        private bool isSumbitTran = false;

        /// <summary>
        /// 是否当前的连接为读库的还是主库的 true 为从库 false 为主库
        /// </summary>
        private bool isSlaveOrMaster = false;

        /// <summary>
        /// 更改的数据库的名字
        /// </summary>
        private string changeDataBaseName = default;

        /// <summary>
        /// 是否已经强制更改数据库连接 
        /// </summary>
        private bool isMandatory = false;

        #endregion
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="_service"></param>
        public UnitOfWork(IOptions<List<EFOptions>> _options, IServiceProvider _service)
        {
            options = _options;
            //获取上下文类型
            dbContextType = typeof(TDbContext);
            //获取当前的上下文
            dbContext = _service.GetService(dbContextType) as DbContext;
            //获取主库的连接
            WriteReadConnectionString = _options.Value.Where(a => a.DbContextType == dbContextType).FirstOrDefault()?.WriteReadConnectionString;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            //关闭连接
            ChangeConnecState(dbContext.Database.GetDbConnection(), ConnectionState.Closed);
            //事务开启更改连接字符串为主库master的连接字符串
            dbContext.Database.GetDbConnection().ConnectionString = WriteReadConnectionString;
            //开启连接
            ChangeConnecState(dbContext.Database.GetDbConnection(), ConnectionState.Open);

            dbContext.Database.BeginTransaction();
            //更改事务的状态
            isSumbitTran = true;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            dbContext.Database.CommitTransaction();
            isSumbitTran = false;
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            dbContext.Database.RollbackTransaction();
            isSumbitTran = false;
        }

        /// <summary>
        /// 强制更改为只读或者读写连接字符串
        /// </summary>
        /// <returns></returns>
        public async Task ChangeReadOrWriteConnection(ReadWriteEnum readWriteEnum = ReadWriteEnum.Read)
        {
            if (isSumbitTran)
                throw new ApplicationException("无法在开启事务的时候执行读写库的更改!");
            await Task.Run(() =>
            {
                //获取连接信息
                var connec = dbContext.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //读写
                if (readWriteEnum == ReadWriteEnum.ReadWrite)
                {
                    //更改连接字符串
                    connec.ConnectionString = WriteReadConnectionString;
                }
                //只读
                else if (readWriteEnum == ReadWriteEnum.Read)
                {
                    connec.ConnectionString = SlaveConnection(dbContextType);
                }
                //打开连接
                ChangeConnecState(connec, ConnectionState.Open);

                isMandatory = true;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync()
        {
            //如果当前没有开启事务 并且 当前为从库的话 则 更改连接字符串为 主库的 
            if (isSumbitTran == false && isSlaveOrMaster && isMandatory == false)
            {
                var connec = dbContext.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //更改连接
                connec.ConnectionString = WriteReadConnectionString;
                //开启连接
                ChangeConnecState(connec, ConnectionState.Open);
                //验证是否更改了数据库名
                ChangeDataBase();
                //更改连接的服务器为主库
                isSlaveOrMaster = false;
            }
            return await dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 同步提交
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            //如果当前没有开启事务 并且 当前为从库的话 则 更改连接字符串为 主库的 
            if (isSumbitTran == false && isSlaveOrMaster && isMandatory == false)
            {
                var connec = dbContext.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //更改连接
                connec.ConnectionString = WriteReadConnectionString;
                //开启连接
                ChangeConnecState(connec, ConnectionState.Open);
                //验证是否更改了数据库名
                ChangeDataBase();
                //更改连接的服务器为主库
                isSlaveOrMaster = false;
            }
            return dbContext.SaveChanges();
        }
        /// <summary>
        /// 仓储服务的入口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Respositiy<T>() where T : class, IEntity
        {
            //如果当前没有开启事务 并且 当前为主库的话 则 更改连接字符串为 从库的 
            if (isSumbitTran == false && isSlaveOrMaster == false && isMandatory == false)
            {
                var connec = dbContext.Database.GetDbConnection();
                //关闭连接
                ChangeConnecState(connec, ConnectionState.Closed);
                //更改连接
                connec.ConnectionString = SlaveConnection(dbContextType);
                //开启连接
                ChangeConnecState(connec, ConnectionState.Open);
                //验证是否更改了数据库名
                ChangeDataBase();
                //更改连接的服务器为从库
                isSlaveOrMaster = true;
            }
            //获取仓储服务（需先注入仓储集合，否则将报错）
            IRepository<T> repository = dbContext.GetService<IRepository<T>>();
            repository.ChangeDbContext(dbContext);
            return repository;
        }

        /// <summary>
        /// 更改数据库的名字
        /// </summary>
        /// <returns></returns>
        public async Task ChangeDataBase(string dataBase)
        {
            await Task.Run(() =>
             {
                 //开启连接
                 ChangeConnecState(dbContext.Database.GetDbConnection(), ConnectionState.Open);
                 dbContext.Database.GetDbConnection().ChangeDatabase(dataBase);
                 changeDataBaseName = dataBase;
             }).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行sql语句的 返回 受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] _params) => await dbContext.Database.ExecuteSqlCommandAsync(sql, _params);

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        #region base

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
        public void ChangeDataBase()
        {
            if (!string.IsNullOrWhiteSpace(changeDataBaseName))
            {
                ChangeConnecState(dbContext.Database.GetDbConnection(), ConnectionState.Open);
                dbContext.Database.GetDbConnection().ChangeDatabase(changeDataBaseName);
            }
        }

        #endregion
    }
}
