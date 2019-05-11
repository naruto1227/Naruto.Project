using Fate.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Fate.Common.Repository.Mysql
{
    /// <summary>
    /// 仓储的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// 上下文
        /// </summary>
        protected DbContext repository { get; set; }
        public RepositoryBase(DbContext _dbContext)
        {
            this.repository = _dbContext;
        }
        /// <summary>
        /// 更改数据库
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public Task ChangeDBConnection(string connectionName)
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            repository.Database.GetDbConnection().ConnectionString = config.GetConnectionString(connectionName);
            return Task.FromResult(0);
        }
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
        /// 删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<T, bool>> condition)
        {
            var info = await Find(condition);
            if (info != null)
                repository.Set<T>().Remove(info);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task BulkDeleteAsync(Expression<Func<T, bool>> condition)
        {
            var list = await QueryAllFromCondition(condition).ToListAsync();
            if (list != null && list.Count() > 0)
                repository.Set<T>().RemoveRange(list);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            repository?.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 执行sql语句的 返回 受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] _params) => await repository.Database.ExecuteSqlCommandAsync(sql, _params);
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> QuerySqlAsync(string sql, params object[] _params) => repository.Set<T>().FromSql(sql, _params);
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync() => await repository.SaveChangesAsync();
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
            var info = await repository.Set<T>().Where(condition).FirstOrDefaultAsync();
            if (info!=null)
            {
                update(info);
            }
        }
        /// <summary>
        ///获取所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryAll() => repository.Set<T>().AsQueryable();

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryAllFromCondition(Expression<Func<T, bool>> condition) => repository.Set<T>().Where(condition).AsQueryable();

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Task<T> Find(Expression<Func<T, bool>> condition) => repository.Set<T>().Where(condition).FirstOrDefaultAsync();
    }
}
