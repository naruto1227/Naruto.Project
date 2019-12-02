using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Mongo.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo层的查询操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultMongoQuery<T, TMongoContext> : IMongoQuery<T, TMongoContext> where T : class where TMongoContext : MongoContext
    {
        /// <summary>
        /// 只读的基础设施
        /// </summary>
        private readonly IMongoReadInfrastructure<TMongoContext> readInfrastructure;


        public DefaultMongoQuery(IMongoReadInfrastructure<TMongoContext> _readInfrastructure)
        {
            readInfrastructure = _readInfrastructure;
        }

        public long Count(string collectionName, FilterDefinition<T> filter, CountOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocuments(filter, options);
            });
        }

        public long Count(FilterDefinition<T> filter, CountOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return Count(collectionName, filter, options);
        }

        public long Count(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocuments(filter, options);
            });
        }

        public long Count(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return Count(collectionName, filter, options);
        }

        public Task<long> CountAsync(string collectionName, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocumentsAsync(filter, options, cancellationToken);
            });
        }

        public Task<long> CountAsync(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return CountAsync(collectionName, filter, options, cancellationToken);
        }

        public Task<long> CountAsync(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocumentsAsync(filter, options, cancellationToken);
            });
        }

        public Task<long> CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return CountAsync(collectionName, filter, options, cancellationToken);
        }

        public List<T> Find(string collectionName, FilterDefinition<T> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).ToList();
            });
        }

        public List<T> Find(FilterDefinition<T> filter, FindOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return Find(collectionName, filter, options);
        }

        public List<T> Find(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).ToList();
            });
        }

        public List<T> Find(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return Find(collectionName, filter, options);
        }

        public async Task<List<T>> FindAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }

        public async Task<List<T>> FindAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return await FindAsync(collectionName, filter, options, cancellationToken);
        }

        public async Task<List<T>> FindAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return await FindAsync(collectionName, filter, options, cancellationToken);
        }

        public List<T> FindByPage(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).Skip(((pageIndex < 0 ? 1 : pageIndex) - 1) * pageSize).Limit(pageSize).ToList();
            });
        }

        public List<T> FindByPage(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return FindByPage(collectionName, pageIndex, pageSize, options);
        }

        public List<T> FindByPage(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).Skip(((pageIndex < 0 ? 1 : pageIndex) - 1) * pageSize).Limit(pageSize).ToList();
            });
        }

        public List<T> FindByPage(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            //获取实体的名称
            var collectionName = typeof(T).Name;
            return FindByPage(collectionName, pageIndex, pageSize, options);
        }

        public async Task<List<T>> FindByPageAsync(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            //return await readInfrastructure.Exec(async database =>
            //{
            //    return ( database.GetCollection<T>(collectionName).Find(filter, options)).Skip(((pageIndex < 0 ? 1 : pageIndex) - 1) * pageSize).Limit(pageSize).ToList();
            //});
            return null;
        }

        public Task<List<T>> FindByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> FindByPageAsync(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> FindByPageAsync(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(string collectionName, FilterDefinition<T> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(FilterDefinition<T> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
