using Fate.Infrastructure.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    public interface IRepositoryQuery<T, TDbContext> : IRepositoryQuery<T>, IRepositoryDependency where T : IEntity
    {

    }
    /// <summary>
    /// 张海波
    /// 2019-08-29
    ///仓储的查询接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryQuery<T> : IRepositoryDependency where T : IEntity
    {
        /// <summary>
        /// 更改仓储的上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        Task ChangeDbContext(DbContext dbContext);
        #region 查询
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <returns></returns>
        IQueryable<T> QuerySqlAsync(string sql, params object[] _params);
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> condition);
        #endregion
    }
}
