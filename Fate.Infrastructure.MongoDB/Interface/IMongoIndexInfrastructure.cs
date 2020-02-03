using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Interface
{
    /// <summary>
    /// 索引的操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMongoIndexInfrastructure<T> where T : class
    {
        #region 同步

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="model"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string CreateOne(CreateIndexModel<T> model, CreateOneIndexOptions options = null);
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="collectionName">集合名称(数据表名称)</param>
        /// <param name="model"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string CreateOne(string collectionName, CreateIndexModel<T> model, CreateOneIndexOptions options = null);
        /// <summary>
        /// 创建多个索引
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IEnumerable<string> CreateMany(IEnumerable<CreateIndexModel<T>> models);

        /// <summary>
        /// 创建多个索引
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IEnumerable<string> CreateMany(string collectionName, IEnumerable<CreateIndexModel<T>> models);
        /// <summary>
        /// 移除索引 根据索引名称
        /// </summary>
        /// <param name="name"></param>
        void DropOne(string name);
        /// <summary>
        /// 移除索引 根据索引名称
        /// </summary>
        /// <param name="name"></param>
        void DropOne(string collectionName, string name);

        /// <summary>
        /// 删除所有索引
        /// </summary>
        void DropAll();


        /// <summary>
        /// 删除所有索引
        /// </summary>
        void DropAll(string collectionName);


        #endregion

        #region 异步

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="model"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<string> CreateOneAsync(CreateIndexModel<T> model, CreateOneIndexOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="model"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<string> CreateOneAsync(string collectionName, CreateIndexModel<T> model, CreateOneIndexOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建多个索引
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateManyAsync(string collectionName, IEnumerable<CreateIndexModel<T>> models, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建多个索引
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateManyAsync(IEnumerable<CreateIndexModel<T>> models, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除索引 根据索引名称
        /// </summary>
        /// <param name="name"></param>
        Task DropOneAsync(string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// 移除索引 根据索引名称
        /// </summary>
        /// <param name="name"></param>
        Task DropOneAsync(string collectionName, string name, CancellationToken cancellationToken = default);


        /// <summary>
        /// 删除所有索引
        /// </summary>
        Task DropAllAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 删除所有索引
        /// </summary>
        Task DropAllAsync(string collectionName, CancellationToken cancellationToken = default);

        #endregion

    }

    /// <summary>
    /// 张海波
    /// 2019-12-5
    /// mongo的索引操作
    /// </summary>
    public interface IMongoIndexInfrastructure<T, TMongoContext> : IMongoIndexInfrastructure<T> where T : class where TMongoContext : MongoContext
    {

    }
}
