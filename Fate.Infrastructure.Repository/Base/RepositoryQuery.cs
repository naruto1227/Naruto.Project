using Fate.Infrastructure.Base.Model;
using Fate.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryQuery<T, TDbContext> : IRepositoryQuery<T, TDbContext> where T : class, IEntity where TDbContext : DbContext
    {

        private DbContext repository;

        public RepositoryQuery(IDbContextFactory factory)
        {
            repository = factory.Get<TDbContext>();
        }
        /// <summary>
        /// 更改仓储的上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public Task ChangeDbContext(DbContext dbContext)
        {
            repository = dbContext;
            return Task.FromResult(0);
        }
        #region 查询

        /// <summary>
        ///获取所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AsQueryable() => repository.Set<T>().AsQueryable();

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> condition) => repository.Set<T>().Where(condition).AsQueryable();
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> QuerySqlAsync(string sql, params object[] _params) => repository.Set<T>().FromSqlRaw(sql, _params);

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefault();

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Task<T> FindAsync(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefaultAsync();

        #endregion
    }
}
