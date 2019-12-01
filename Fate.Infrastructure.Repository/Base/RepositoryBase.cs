using Fate.Infrastructure.BaseRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Fate.Infrastructure.Repository.Interface;

namespace Fate.Infrastructure.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 仓储的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// 上下文
        /// </summary>
        internal DbContext repository { get; set; }
        public RepositoryBase(DbContext _dbContext = null)
        {
            repository = _dbContext;
        }
        ///// <summary>
        /////获取所有数据
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <returns></returns>
        //public IQueryable<T> AsQueryable(string tableName = "")
        //{
        //    if (repository.Model.FindEntityType(typeof(T)).Relational() is RelationalEntityTypeAnnotations relational2)
        //    {

        //    }
        //    if (!string.IsNullOrWhiteSpace(tableName))
        //    {
        //        if (repository.Model.FindEntityType(typeof(T)).Relational() is RelationalEntityTypeAnnotations relational)
        //        {
        //            relational.TableName = tableName;
        //        }
        //    }
        //    return repository.Set<T>().AsQueryable();
        //}
        ///// <summary>
        ///// 更改数据库
        ///// </summary>
        ///// <param name="connectionName"></param>
        ///// <returns></returns>
        //public Task ChangeDBConnection(string connectionName)
        //{
        //    return Task.FromResult(0);
        //}
        ///// <summary>
        ///// 更改上下文
        ///// </summary>
        ///// <param name="dbContext"></param>
        ///// <returns></returns>
        //public Task ChangeDbContext(DbContext dbContext)
        //{
        //    repository = dbContext;
        //    return Task.FromResult(0);
        //}

        //#region 异步
        ///// <summary>
        ///// 单条数据添加
        ///// </summary>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //public async Task AddAsync(T info) => await repository.Set<T>().AddAsync(info);

        ///// <summary>
        ///// 批量添加数据
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //public async Task BulkAddAsync(IEnumerable<T> entities) => await repository.Set<T>().AddRangeAsync(entities);

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //public async Task DeleteAsync(Expression<Func<T, bool>> condition)
        //{
        //    var list = await Where(condition).ToArrayAsync();
        //    if (list != null && list.Count() > 0)
        //        BulkDelete(list);
        //}
        ///// <summary>
        ///// 批量删除数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //public async Task BulkDeleteAsync(params T[] entities)
        //{
        //    await Task.Run(() =>
        //    {
        //        repository.Set<T>().RemoveRange(entities);
        //    }).ConfigureAwait(false);
        //}
        ///// <summary>
        ///// 更新单条实体
        ///// </summary>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //public Task UpdateAsync(T info)
        //{
        //    repository.Set<T>().Update(info);
        //    return Task.FromResult(0);
        //}
        ///// <summary>
        ///// 更新个别的字段数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <param name="update"></param>
        ///// <returns></returns>
        //public async Task UpdateAsync(Expression<Func<T, bool>> condition, Func<T, T> update)
        //{
        //    var list = await Where(condition).ToListAsync();
        //    if (list != null && list.Count() > 0)
        //    {
        //        foreach (var item in list)
        //            update(item);
        //    }
        //}
        ///// <summary>
        ///// 编辑
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //public Task BulkUpdateAsync(params T[] entities)
        //{
        //    repository.Set<T>().UpdateRange(entities);
        //    return Task.FromResult(0);
        //}
        ///// <summary>
        ///// 获取单条记录
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <returns></returns>
        //public Task<T> FindAsync(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefaultAsync();

        //#endregion

        //#region 同步
        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="entity"></param>
        //public void Add(T entity)
        //{
        //    repository.Set<T>().Add(entity);
        //}

        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="info"></param>
        //public void BulkAdd(IEnumerable<T> entities)
        //{
        //    repository.Set<T>().AddRange(entities);
        //}

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //public void Delete(Expression<Func<T, bool>> condition)
        //{
        //    var list = Where(condition).ToArray();
        //    if (list != null && list.Count() > 0)
        //        BulkDelete(list);
        //}
        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //public void BulkDelete(params T[] entities) => repository.Set<T>().RemoveRange(entities);
        ///// <summary>
        ///// 更新单条实体
        ///// </summary>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //public void Update(T info) => repository.Set<T>().Update(info);
        ///// <summary>
        ///// 更新个别的字段数据
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <param name="update"></param>
        ///// <returns></returns>
        //public void Update(Expression<Func<T, bool>> condition, Func<T, T> update)
        //{
        //    var list = Where(condition).ToList();
        //    if (list != null && list.Count() > 0)
        //    {
        //        foreach (var item in list)
        //            update(item);
        //    }
        //}
        ///// <summary>
        ///// 编辑
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //public void BulkUpdate(params T[] entities) => repository.Set<T>().UpdateRange(entities);
        ///// <summary>
        ///// 获取单条记录
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <returns></returns>
        //public T Find(Expression<Func<T, bool>> condition) => Where(condition).FirstOrDefault();
        //#endregion

        //#region 查询

        ///// <summary>
        /////获取所有数据
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <returns></returns>
        //public IQueryable<T> AsQueryable() => repository.Set<T>().AsQueryable();

        ///// <summary>
        ///// 根据条件查询
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <returns></returns>
        //public IQueryable<T> Where(Expression<Func<T, bool>> condition) => repository.Set<T>().Where(condition).AsQueryable();
        ///// <summary>
        ///// sql语句查询
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<T> QuerySqlAsync(string sql, params object[] _params) => repository.Set<T>().FromSql(sql, _params);

        //#endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            repository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
