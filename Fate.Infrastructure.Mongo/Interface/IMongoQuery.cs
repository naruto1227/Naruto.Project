using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Fate.Infrastructure.Mongo.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 查询服务
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="TMongoContext">mongo上下文</typeparam>

    public interface IMongoQuery<T, TMongoContext> : IMongoQuery<T> where T : class where TMongoContext : MongoContext
    {

    }
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongodb的查询操作
    /// </summary>
    public interface IMongoQuery<T> where T : class
    {
        #region  同步

        #region FilterDefinition
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> Find(string collectionName, FilterDefinition<T> filter, FindOptions options = null);

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> Find(FilterDefinition<T> filter, FindOptions options = null);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefault(string collectionName, FilterDefinition<T> filter, FindOptions options = null);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefault(FilterDefinition<T> filter, FindOptions options = null);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPage(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPage(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int Count(string collectionName, FilterDefinition<T> filter, CountOptions options = null);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int Count(FilterDefinition<T> filter, CountOptions options = null);

        #endregion

        #region Expression
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> Find(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null);

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> Find(Expression<Func<T, bool>> filter, FindOptions options = null);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefault(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> filter, FindOptions options = null);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPage(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPage(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int Count(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> filter, CountOptions options = null);

        #endregion

        #endregion

        #region  异步

        #region FilterDefinition

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefaultAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPageAsync(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int CountAsync(string collectionName, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int CountAsync(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default);

        #endregion

        #region Expression

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="collectionName">集合的名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefaultAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FirstOrDefaultAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPageAsync(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查找数据 
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">条数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> FindByPageAsync(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int CountAsync(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="filter">条件筛选</param>
        /// <param name="options"></param>
        /// <returns></returns>
        int CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default);

        #endregion

        #endregion
    }
}
