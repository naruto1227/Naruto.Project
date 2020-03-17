using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 增删改服务
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="TMongoContext">mongo上下文</typeparam>

    public interface IMongoCommand<T, TMongoContext> : IMongoCommand<T> where T : class where TMongoContext : MongoContext
    {

    }

    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongo的增删改操作
    /// </summary>
    public interface IMongoCommand<T> where T : class
    {
        #region 同步

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        void Add(T document, InsertOneOptions options = null);

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="options"></param>
        void BulkAdd(IEnumerable<T> documents, InsertManyOptions options = null);

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="document"></param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="options"></param>
        void Add(string collectionName, T document, InsertOneOptions options = null);

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="options"></param>
        void BulkAdd(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null);

        /// <summary>
        /// 批量写
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        BulkWriteResult<T> BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null);

        /// <summary>
        /// 批量写
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="options"></param>
        /// <returns></returns>
        BulkWriteResult<T> BulkWrite(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Delete(FilterDefinition<T> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Delete(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Delete(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        long BulkDelete(FilterDefinition<T> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        long BulkDelete(Expression<Func<T, bool>> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        long BulkDelete(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        long BulkDelete(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null);

        /// <summary>
        /// 查找一个数据 并且删除掉
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndDelete(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndDelete(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndDelete(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndDelete(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null);

        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateField">需要更新的字段的key和value</param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Update(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新单条
        /// </summary>
        /// <param name="updateField">需要更新的字段的key和value</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Update(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="updateField">需要更新的字段的key和value</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Update(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新单条
        /// </summary>
        /// <param name="updateField">需要更新的字段的key和value</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Update(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool BulkUpdate(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool BulkUpdate(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null);
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool BulkUpdate(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool BulkUpdate(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndUpdate(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndUpdate(FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndUpdate(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndUpdate(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool ReplaceOne(FilterDefinition<T> filter, T replacement, ReplaceOptions options = null);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool ReplaceOne(Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool ReplaceOne(string collectionName, FilterDefinition<T> filter, T replacement, ReplaceOptions options = null);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool ReplaceOne(string collectionName, Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null);

        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndReplace(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null);
        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndReplace(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null);


        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndReplace(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null);
        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T FindOneAndReplace(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null);

        #endregion

        #region 异步

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        Task AddAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="options"></param>
        Task BulkAddAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="document"></param>
        /// <param name="options"></param>
        Task AddAsync(string collectionName, T document, InsertOneOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="documents"></param>
        /// <param name="options"></param>
        Task BulkAddAsync(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量写
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<BulkWriteResult<T>> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量写
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="requests"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<BulkWriteResult<T>> BulkWriteAsync(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<long> BulkDeleteAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<long> BulkDeleteAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<long> BulkDeleteAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<long> BulkDeleteAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndDeleteAsync(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndDeleteAsync(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndDeleteAsync(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一个数据 并且删除掉(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndDeleteAsync(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="updateField">需要更改的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新单条
        /// </summary>
        /// <param name="updateField">需要更改的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="updateField">需要更改的字段</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新单条
        /// </summary>
        /// <param name="updateField">需要更改的字段</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> BulkUpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndUpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndUpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndUpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找一条数据并且更新(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndUpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> ReplaceOneAsync(FilterDefinition<T> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> ReplaceOneAsync(string collectionName, FilterDefinition<T> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 替换单个文档
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> ReplaceOneAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndReplaceAsync(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndReplaceAsync(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndReplaceAsync(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查找单个并替换(返回旧数据)
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> FindOneAndReplaceAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default);
        #endregion
    }

}
