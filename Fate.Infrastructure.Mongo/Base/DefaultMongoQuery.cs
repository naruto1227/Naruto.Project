using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

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

        public DefaultMongoQuery()
        {

        }
        public int Count(string collectionName, FilterDefinition<T> filter, CountOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int Count(FilterDefinition<T> filter, CountOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int Count(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            throw new NotImplementedException();
        }

        public int CountAsync(string collectionName, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int CountAsync(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int CountAsync(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> Find(string collectionName, FilterDefinition<T> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> Find(FilterDefinition<T> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> Find(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> Find(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> FindAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPage(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPage(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPage(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPage(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPageAsync(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPageAsync(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByPageAsync(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
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

        public T FirstOrDefaultAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefaultAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefaultAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
