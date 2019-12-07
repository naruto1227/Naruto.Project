using Fate.Infrastructure.BaseRepository.Model;
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

        /// <summary>
        /// 获取读写的基础设施
        /// </summary>
        private readonly IRepositoryReadInfrastructure<TDbContext> infrastructure;

        public RepositoryQuery(IRepositoryReadInfrastructure<TDbContext> _infrastructure)
        {
            infrastructure = _infrastructure;
        }
        #region 查询

        /// <summary>
        ///获取所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AsQueryable() => infrastructure.Exec(repository => repository.Set<T>().AsQueryable());

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => repository.Set<T>().Where(condition).AsQueryable());
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> QuerySqlAsync(string sql, params object[] _params) => infrastructure.Exec(repository => repository.Set<T>().FromSqlRaw(sql, _params));

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefault());

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Task<T> FindAsync(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefaultAsync());

        #endregion
    }
}
