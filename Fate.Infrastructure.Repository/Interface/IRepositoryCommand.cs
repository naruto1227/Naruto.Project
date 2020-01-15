using Fate.Infrastructure.BaseRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fate.Infrastructure.Repository.Interface
{
    public interface IRepositoryCommand<T, TDbContext> : IRepositoryCommand<T>, IRepositoryDependency where T : IEntity where TDbContext : DbContext
    {

    }
    /// <summary>
    /// 张海波
    /// 2019-08-29
    /// 仓储的增删改的 接口层
    /// </summary>
    public interface IRepositoryCommand<T> : IRepositoryDependency where T : IEntity
    {
        #region 异步
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
        Task BulkDeleteAsync(params T[] entities);

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
        /// 批量编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkUpdateAsync(params T[] entities);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkAddAsync(IEnumerable<T> entities);

        #endregion

        #region 同步
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info"></param>
        void Add(T entity);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info"></param>
        void BulkAdd(IEnumerable<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        void Delete(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        void BulkDelete(params T[] entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        void BulkDelete(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        void Update(T info);
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        void Update(Expression<Func<T, bool>> condition, Func<T, T> update);

        /// <summary>
        /// 批量编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void BulkUpdate(params T[] entities);

        #endregion
    }
}
