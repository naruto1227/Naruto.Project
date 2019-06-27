using System;
using System.Text;
using System.Threading.Tasks;
using Fate.Domain.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Fate.Common.Repository.Mysql.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MysqlDbContent dbContext;
        public UnitOfWork(MysqlDbContent _dbContext)
        {
            dbContext = _dbContext;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
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
            //获取仓储服务
            IRepository<T> repository = dbContext.GetService<IRepository<T>>();
            if (repository == null)
            {
                repository = new RepositoryBase<T>(dbContext); ;
            }
            return repository;
        }
    }
}
