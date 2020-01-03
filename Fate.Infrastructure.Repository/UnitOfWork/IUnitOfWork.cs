using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Infrastructure.Repository.Object;
using Fate.Infrastructure.Repository.Interface;
using Fate.Infrastructure.BaseRepository.Model;
using Microsoft.EntityFrameworkCore;
namespace Fate.Infrastructure.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        int CommandTimeout { set; }
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
        /// 异步开始事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryQuery<T> Query<T>() where T : class, IEntity;

        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryCommand<T> Command<T>() where T : class, IEntity;

        /// <summary>
        /// 更改数据库
        /// </summary>
        /// <returns></returns>
        Task ChangeDataBase(string dataBase);

        /// <summary>
        /// 返回sql查询的对象
        /// </summary>
        /// <returns></returns>
        ISqlQuery SqlQuery();
        /// <summary>
        /// 返回sql增删改的对象
        /// </summary>
        /// <returns></returns>
        ISqlCommand SqlCommand();
    }

}
