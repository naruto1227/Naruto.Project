using Fate.Infrastructure.Base.Model;
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
    public class RepositoryCommand<T, TDbContext> : IRepositoryCommand<T, TDbContext> where T : class, IEntity where TDbContext:DbContext
    {

        private DbContext repository;

        public RepositoryCommand(IDbContextFactory factory)
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
        #region 异步
        /// <summary>
        /// 单条数据添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task AddAsync(T info) => await repository.Set<T>().AddAsync(info);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkAddAsync(IEnumerable<T> entities) => await repository.Set<T>().AddRangeAsync(entities);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<T, bool>> condition)
        {
            var list = await Where(condition).ToArrayAsync();
            if (list != null && list.Count() > 0)
                BulkDelete(list);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task BulkDeleteAsync(params T[] entities)
        {
            await Task.Run(() =>
            {
                repository.Set<T>().RemoveRange(entities);
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Task UpdateAsync(T info)
        {
            repository.Set<T>().Update(info);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Expression<Func<T, bool>> condition, Func<T, T> update)
        {
            var list = await Where(condition).ToListAsync();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                    update(item);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task BulkUpdateAsync(params T[] entities)
        {
            repository.Set<T>().UpdateRange(entities);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private Task<T> FindAsync(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefaultAsync();

        #endregion

        #region 同步
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            repository.Set<T>().Add(entity);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info"></param>
        public void BulkAdd(IEnumerable<T> entities)
        {
            repository.Set<T>().AddRange(entities);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<T, bool>> condition)
        {
            var list = Where(condition).ToArray();
            if (list != null && list.Count() > 0)
                BulkDelete(list);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void BulkDelete(params T[] entities) => repository.Set<T>().RemoveRange(entities);
        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Update(T info) => repository.Set<T>().Update(info);
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public void Update(Expression<Func<T, bool>> condition, Func<T, T> update)
        {
            var list = Where(condition).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                    update(item);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BulkUpdate(params T[] entities) => repository.Set<T>().UpdateRange(entities);

        #endregion

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private T Find(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefault();
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private IQueryable<T> Where(Expression<Func<T, bool>> condition) => repository.Set<T>().Where(condition).AsQueryable();
    }
}
