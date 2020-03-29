using Naruto.BaseRepository.Model;
using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Naruto.Repository.Base
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
        public async Task AddAsync(T info, CancellationToken cancellationToken = default) => await await infrastructure.ExecAsync(async repository => await repository.Set<T>().AddAsync(info, cancellationToken)).ConfigureAwait(false);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkAddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => await await infrastructure.ExecAsync(async repository => await repository.Set<T>().AddRangeAsync(entities, cancellationToken)).ConfigureAwait(false);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(async repository =>
             {
                 var list = await Where(condition).ToArrayAsync().ConfigureAwait(false);
                 if (list != null && list.Count() > 0)
                     BulkDelete(list);
             });
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task BulkDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(repository =>
           {
               repository.Set<T>().RemoveRange(entities);
               return Task.CompletedTask;
           });
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task BulkDeleteAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(async repository =>
             {
                 var list = await Where(condition).ToListAsync();
                 if (list != null && list.Count() > 0)
                     repository.Set<T>().RemoveRange(list);
             });
        }
        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T info, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(repository =>
           {
               repository.Set<T>().Update(info);
               return Task.CompletedTask;
           });
        }
        /// <summary>
        /// 更新个别的字段数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Expression<Func<T, bool>> condition, Func<T, T> update, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(async repository =>
            {
                var list = await Where(condition).ToListAsync().ConfigureAwait(false);
                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                        update(item);
                }
            });
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await await infrastructure.ExecAsync(repository =>
           {
               repository.Set<T>().UpdateRange(entities);
               return Task.CompletedTask;
           });
        }


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
            });
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
            });
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
            });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void BulkDelete(IEnumerable<T> entities) => infrastructure.Exec(repository => repository.Set<T>().RemoveRange(entities));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void BulkDelete(Expression<Func<T, bool>> condition)
        {
            infrastructure.Exec(repository =>
            {
                var list = Where(condition).ToList();
                if (list != null && list.Count() > 0)
                    repository.Set<T>().RemoveRange(list);
            });
        }

        /// <summary>
        /// 更新单条实体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Update(T info) => infrastructure.Exec(repository => repository.Set<T>().Update(info));
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
            });
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BulkUpdate(IEnumerable<T> entities) => infrastructure.Exec(repository => repository.Set<T>().UpdateRange(entities));

        #endregion

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private IQueryable<T> Where(Expression<Func<T, bool>> condition) => infrastructure.Exec(repository => repository.Set<T>().Where(condition).AsQueryable());
    }
}
