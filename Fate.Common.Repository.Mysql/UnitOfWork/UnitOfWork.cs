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
        /// 更改为只读连接字符串
        /// </summary>
        /// <returns></returns>
        public async Task ChangeReadOnlyConnection()
        {
            //if (options?.Value.Where(a => a.DbContextType == typeof(TDbContext)).FirstOrDefault()?.ReadOnlyConnectionString == null)
            //    throw new ApplicationException("数据库只读连接字符串不能为空");
            ////获取连接字符串的数组 多个用|分割开
            //var connections = options?.Value.Where(a => a.DbContextType == typeof(TDbContext)).FirstOrDefault()?.ReadOnlyConnectionString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //if (connections == null || connections.Count() <= 0)
            //    throw new ApplicationException("数据库只读连接字符串不能为空");
            ////随机数
            //var random = new Random();
            //dbContext.Database.GetDbConnection().ConnectionString = connections[random.Next(0, connections.Count() - 1)];
            //await Task.FromResult(0).ConfigureAwait(false);
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
