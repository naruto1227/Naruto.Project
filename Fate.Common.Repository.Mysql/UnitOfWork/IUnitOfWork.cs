using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Repository.Base;
using Fate.Common.Repository.Interface;
using Fate.Domain.Model;
using Microsoft.EntityFrameworkCore;
namespace Fate.Common.Repository.UnitOfWork
{
    public interface IUnitOfWork {
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

        /// <summary>
        /// 更改数据库
        /// </summary>
        /// <returns></returns>
        Task ChangeDataBase(string dataBase);

        /// <summary>
        /// 执行slq语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, params object[] _params);

        /// <summary>
        /// 强制更改为只读或者读写连接字符串(当前作用域将不再更改)
        /// </summary>
        /// <returns></returns>
        Task ChangeReadOrWriteConnection(ReadWriteEnum readWriteEnum = ReadWriteEnum.ReadWrite);
    }

}
