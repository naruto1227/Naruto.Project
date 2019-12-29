using Fate.Infrastructure.BaseRepository.Model;
using Fate.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Fate.Infrastructure.Repository.Base
{
    public class RepositoryCommand<T, TDbContext> : IRepositoryCommand<T, TDbContext> where T : class, IEntity where TDbContext : DbContext
    {
        /// <summary>
        /// 获取读写的基础设施
        /// </summary>
        private readonly IRepositoryWriteInfrastructure<TDbContext> infrastructure;

        public RepositoryCommand(IRepositoryWriteInfrastructure<TDbContext> _infrastructure)
        {
            infrastructure = _infrastructure;
        }

        #region 异步
        /// <summary>
        /// 单条数据添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task AddAsync(T info) => await infrastructure.Exec(async repository => await repository.Set<T>().AddAsync(info),false).ConfigureAwait(false);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkAddAsync(IEnumerable<T> entities) => await infrastructure.Exec(async repository => await repository.Set<T>().AddRangeAsync(entities), false).ConfigureAwait(false);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<T, bool>> condition)
        {
            await infrastructure.Exec(async repository =>
            {
                var list = await Where(condition).ToArrayAsync().ConfigureAwait(false);
                if (list != null && list.Count() > 0)
                    BulkDelete(list);
            }, false);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Task BulkDeleteAsync(params T[] entities)
        {
            return infrastructure.Exec(repository =>
           {
               repository.Set<T>().RemoveRange(entities);
               return Task.CompletedTask;
           }, false);
        }
        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Task UpdateAsync(T info)
        {
            return infrastructure.Exec(repository =>
           {
               repository.Set<T>().Update(info);
               return Task.CompletedTask;
           }, false);
        }
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Expression<Func<T, bool>> condition, Func<T, T> update)
        {
            await infrastructure.Exec(async repository =>
           {
               var list = await Where(condition).ToListAsync().ConfigureAwait(false);
               if (list != null && list.Count() > 0)
               {
                   foreach (var item in list)
                       update(item);
               }
           }, false);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task BulkUpdateAsync(params T[] entities)
        {
            return infrastructure.Exec(repository =>
           {
               repository.Set<T>().UpdateRange(entities);
               return Task.CompletedTask;
           }, false);
        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private Task<T> FindAsync(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefaultAsync());

        #endregion

        #region 同步
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            infrastructure.Exec(repository =>
            {
                repository.Set<T>().Add(entity);
            }, false);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info"></param>
        public void BulkAdd(IEnumerable<T> entities)
        {
            infrastructure.Exec(repository =>
            {
                repository.Set<T>().AddRange(entities);
            }, false);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<T, bool>> condition)
        {
            infrastructure.Exec(repository =>
            {
                var list = Where(condition).ToArray();
                if (list != null && list.Count() > 0)
                    BulkDelete(list);
            }, false);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void BulkDelete(params T[] entities) => infrastructure.Exec(repository => repository.Set<T>().RemoveRange(entities), false);
        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Update(T info) => infrastructure.Exec(repository => repository.Set<T>().Update(info), false);
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public void Update(Expression<Func<T, bool>> condition, Func<T, T> update)
        {
            infrastructure.Exec(repository =>
            {
                var list = Where(condition).ToList();
                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                        update(item);
                }
            }, false);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BulkUpdate(params T[] entities) => infrastructure.Exec(repository => repository.Set<T>().UpdateRange(entities), false);

        #endregion

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private T Find(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => Where(condition).FirstOrDefault());
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private IQueryable<T> Where(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => repository.Set<T>().Where(condition).AsQueryable());
    }
}
