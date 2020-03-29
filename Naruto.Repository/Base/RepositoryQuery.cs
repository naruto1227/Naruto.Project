using Naruto.BaseRepository.Model;
using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Naruto.Repository.Base
{
    /// <summary>
    /// 从库查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryQuery<T, TDbContext> : RepositoryQueryAbstract<T>, IRepositoryQuery<T, TDbContext> where T : class, IEntity where TDbContext : DbContext
    {
        public RepositoryQuery(IRepositoryReadInfrastructure<TDbContext> _infrastructure):base(_infrastructure)
        {
        }
    }
    /// <summary>
    /// 主库查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryMasterQuery<T, TDbContext> : RepositoryQueryAbstract<T>, IRepositoryMasterQuery<T, TDbContext> where T : class, IEntity where TDbContext : DbContext
    {
        public RepositoryMasterQuery(IRepositoryWriteInfrastructure<TDbContext> _infrastructure) : base(_infrastructure)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryQueryAbstract<T> : IRepositoryQuery<T> where T : class, IEntity
    {
        /// <summary>
        /// 获取主库的基础设施
        /// </summary>
        private readonly IRepositoryInfrastructure infrastructure;

        public RepositoryQueryAbstract(IRepositoryInfrastructure _infrastructure)
        {
            infrastructure = _infrastructure;
        }
        #region 查询

        /// <summary>
        ///获取所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> AsQueryable() => infrastructure.Exec(repository => repository.Set<T>().AsQueryable());

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => repository.Set<T>().AsQueryable().Where(condition));
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> QuerySqlAsync(string sql, params object[] _params) => infrastructure.Exec(repository => repository.Set<T>().FromSqlRaw(sql, _params));

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefault());

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual Task<T> FindAsync(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefaultAsync());

        #endregion
    }
}
