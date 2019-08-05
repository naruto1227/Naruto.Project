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

namespace Fate.Common.Repository.Mysql.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly DbContext dbContext;
        private IOptions<List<EFOptions>> options;
        private string WriteReadConnectionName;
        public UnitOfWork(IOptions<List<EFOptions>> _options, IServiceProvider _service)
        {
            options = _options;
            //获取上下文类型
            var dbContextType = typeof(TDbContext);
            //获取当前的上下文
            dbContext = _service.GetService(dbContextType) as DbContext;
            //获取主库的连接
            WriteReadConnectionName = _options.Value.Where(a => a.DbContextType == dbContextType).FirstOrDefault()?.WriteReadConnectionString;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            //事务开启更改连接字符串为主库master的连接字符串
            dbContext.Database.GetDbConnection().ConnectionString = WriteReadConnectionName;
            dbContext.Database.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            dbContext.Database.CommitTransaction();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            dbContext.Database.RollbackTransaction();
        }
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public IRepository<T> Respositiy<T>() where T : class, IEntity
        {
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
                 dbContext.Database.OpenConnection();
                 dbContext.Database.GetDbConnection().ChangeDatabase(dataBase);
             });
        }

        /// <summary>
        /// 执行sql语句的 返回 受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] _params) => await dbContext.Database.ExecuteSqlCommandAsync(sql, _params);
    }
}
