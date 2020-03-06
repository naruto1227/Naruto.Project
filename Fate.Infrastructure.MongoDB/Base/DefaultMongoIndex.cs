using Fate.Infrastructure.MongoDB.Interface;
using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-5
    /// 索引操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMongoContext"></typeparam>
    public class DefaultMongoIndex<T, TMongoContext> : IMongoIndexInfrastructure<T, TMongoContext> where T : class where TMongoContext : MongoContext
    {
        /// <summary>
        ///基础设施
        /// </summary>
        private readonly IMongoInfrastructure<TMongoContext> infrastructure;

        /// <summary>
        /// 实体的类型名
        /// </summary>
        private readonly string collectionTypeName = typeof(T).Name;


        public DefaultMongoIndex(IMongoInfrastructure<TMongoContext> _infrastructure)
        {
            infrastructure = _infrastructure;
        }

        public IEnumerable<string> CreateMany(IEnumerable<CreateIndexModel<T>> models)
            => CreateMany(collectionTypeName, models);


        public IEnumerable<string> CreateMany(string collectionName, IEnumerable<CreateIndexModel<T>> models)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.CreateMany(models);
            });
        }

        public Task<IEnumerable<string>> CreateManyAsync(string collectionName, IEnumerable<CreateIndexModel<T>> models, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.CreateManyAsync(models, cancellationToken);
            });
        }

        public Task<IEnumerable<string>> CreateManyAsync(IEnumerable<CreateIndexModel<T>> models, CancellationToken cancellationToken = default)
        => CreateManyAsync(collectionTypeName, models);

        public string CreateOne(CreateIndexModel<T> model, CreateOneIndexOptions options = null)
        => CreateOne(collectionTypeName, model, options);


        public string CreateOne(string collectionName, CreateIndexModel<T> model, CreateOneIndexOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.CreateOne(model, options);
            });
        }

        public Task<string> CreateOneAsync(CreateIndexModel<T> model, CreateOneIndexOptions options = null, CancellationToken cancellationToken = default)
        => CreateOneAsync(collectionTypeName, model, options);

        public Task<string> CreateOneAsync(string collectionName, CreateIndexModel<T> model, CreateOneIndexOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.CreateOneAsync(model, options, cancellationToken);
            });
        }

        public void DropAll()
        => DropAll(collectionTypeName);

        public void DropAll(string collectionName)
        {
            infrastructure.Exec(database =>
           {
               database.GetCollection<T>(collectionName).Indexes.DropAll();
           });
        }

        public Task DropAllAsync(CancellationToken cancellationToken = default)
        => DropAllAsync(collectionTypeName, cancellationToken);

        public Task DropAllAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.DropAllAsync(cancellationToken);
            });
        }

        public void DropOne(string name)
        => DropOne(collectionTypeName, name);

        public void DropOne(string collectionName, string name)
        {
            infrastructure.Exec(database =>
           {
               database.GetCollection<T>(collectionName).Indexes.DropOne(name);
           });
        }

        public Task DropOneAsync(string name, CancellationToken cancellationToken = default)
        => DropOneAsync(collectionTypeName, name, cancellationToken);

        public Task DropOneAsync(string collectionName, string name, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Indexes.DropOneAsync(name, cancellationToken);
            });
        }
    }
}
