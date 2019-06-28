using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Fate.Domain.Model;
namespace Fate.Common.Repository.Mysql
{
    /// <summary>
    /// 数据访问仓库的接口
    /// </summary>
    public interface IRepository<T> : IDisposable, IRepositoryDependency where T : IEntity
    {
        /// <summary>
        /// 更改数据库
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task ChangeDBConnection(string connectionName);
        /// <summary>
        /// 更改上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        Task ChangeDbContext(DbContext dbContext);
        /// <summary>
        /// 提交保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task AddAsync(T info);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task UpdateAsync(T info);
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        Task UpdateAsync(Expression<Func<T, bool>> condition, Func<T, T> update);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkAddAsync(IEnumerable<T> entities);

        /// <summary>
        /// 批量编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkUpdateAsync(params T[] entities);
        /// <summary>
        /// 执行slq语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, params object[] _params);
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
        Task<T> Find(Expression<Func<T, bool>> condition);
    }
}
