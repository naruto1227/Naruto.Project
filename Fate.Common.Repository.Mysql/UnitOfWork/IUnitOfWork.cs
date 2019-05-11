using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Domain.Model;
namespace Fate.Common.Repository.Mysql.UnitOfWork
{
    public interface IUnitOfWork:IDisposable, IRepositoryDependency
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollBackTransaction();

        IRepository<T> Respositiy<T>() where T : class, IEntity;
    }
}
