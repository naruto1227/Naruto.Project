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
    /// mongo层的增删改操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultMongoCommand<T, TMongoContext> : IMongoCommand<T, TMongoContext> where T : class where TMongoContext : MongoContext
    {
        public BulkWriteResult<T> BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public BulkWriteResult<T> BulkWrite(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<BulkWriteResult<T>> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BulkWriteResult<T>> BulkWriteAsync(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(FilterDefinition<T> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteManyAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteManyAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteManyAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteManyAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOne(FilterDefinition<T> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOne(Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOne(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOne(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOneAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOneAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOneAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOneAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndDelete(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndDelete(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndDelete(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndDelete(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndDeleteAsync(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndDeleteAsync(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndDeleteAsync(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndDeleteAsync(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndReplace(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndReplace(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndReplace(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndReplace(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndReplaceAsync(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndReplaceAsync(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndReplaceAsync(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndReplaceAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndUpdate(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndUpdate(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public T FindOneAndUpdate(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndUpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndUpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndUpdateAsync(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAndUpdateAsync(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(IEnumerable<T> documents, InsertManyOptions options = null)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void InsertOne(T document, InsertOneOptions options = null)
        {
            throw new NotImplementedException();
        }

        public void InsertOne(string collectionName, T document, InsertOneOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(string collectionName, T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ReplaceOneResult ReplaceOne(FilterDefinition<T> filter, T replacement, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public ReplaceOneResult ReplaceOne(Expression<Func<T, bool>> filter, T replacement, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public ReplaceOneResult ReplaceOne(string collectionName, FilterDefinition<T> filter, T replacement, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public ReplaceOneResult ReplaceOne(string collectionName, Expression<Func<T, bool>> filter, T replacement, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(string collectionName, FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ReplaceOneResult> ReplaceOneAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateMany(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateMany(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateMany(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateManyAsync(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateManyAsync(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateOne(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public UpdateResult UpdateOne(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateOneAsync(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdateOneAsync(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
