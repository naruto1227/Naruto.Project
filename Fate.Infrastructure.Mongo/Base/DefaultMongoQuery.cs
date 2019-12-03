﻿using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
        /// <summary>
        /// 实体的类型名
        /// </summary>
        private readonly string collectionTypeName = typeof(T).Name;

        public DefaultMongoQuery(IMongoReadInfrastructure<TMongoContext> _readInfrastructure)
        {
            readInfrastructure = _readInfrastructure;
        }


        /// <summary>
        /// mongo的IQueryable扩展
        /// </summary>
        /// <returns></returns>
        public IMongoQueryable<T> AsQueryable()
        {
            return AsQueryable(collectionTypeName);
        }

        /// <summary>
        /// mongo的IQueryable扩展
        /// </summary>
        /// <returns></returns>
        public IMongoQueryable<T> AsQueryable(string collectionName)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).AsQueryable();
            });
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long Count(string collectionName, FilterDefinition<T> filter, CountOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocuments(filter, options);
            });
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long Count(FilterDefinition<T> filter, CountOptions options = null)
        {
            return Count(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long Count(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocuments(filter, options);
            });
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long Count(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return Count(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<long> CountAsync(string collectionName, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocumentsAsync(filter, options, cancellationToken);
            });
        }
        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<long> CountAsync(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return CountAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<long> CountAsync(string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).CountDocumentsAsync(filter, options, cancellationToken);
            });
        }
        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<long> CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default)
        {
            return CountAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> Find(string collectionName, FilterDefinition<T> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).ToList();
            });
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> Find(FilterDefinition<T> filter, FindOptions options = null)
        {
            return Find(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> Find(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).ToList();
            });
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> Find(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return Find(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FindAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FindAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> FindByPage(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).Skip(((pageIndex < 0 ? 1 : pageIndex) - 1) * pageSize).Limit(pageSize).ToList();
            });
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> FindByPage(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return FindByPage(collectionTypeName, pageIndex, pageSize, options);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> FindByPage(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).Find(filter, options).Skip(((pageIndex < 0 ? 1 : pageIndex) - 1) * pageSize).Limit(pageSize).ToList();
            });
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> FindByPage(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions options = null)
        {
            return FindByPage(collectionTypeName, pageIndex, pageSize, options);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="filter">条件</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">行数</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> FindByPageAsync(string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            //验证分页参数
            if (options == null)
                options = new FindOptions<T>();
            if (pageIndex > 0 && options.Skip <= 0)
                options.Skip = (pageIndex - 1) * pageSize;
            if (pageSize > 0 && options.Limit <= 0)
                options.Limit = pageSize;

            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FindByPageAsync(collectionTypeName, filter, pageIndex, pageSize, options, cancellationToken);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindByPageAsync(string collectionName, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            //验证分页参数
            if (options == null)
                options = new FindOptions<T>();
            if (pageIndex > 0 && options.Skip <= 0)
                options.Skip = (pageIndex - 1) * pageSize;
            if (pageSize > 0 && options.Limit <= 0)
                options.Limit = pageSize;

            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).ToListAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<List<T>> FindByPageAsync(Expression<Func<T, bool>> filter, int pageIndex, int pageSize, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FindByPageAsync(collectionTypeName, filter, pageIndex, pageSize, options, cancellationToken);
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FirstOrDefault(string collectionName, FilterDefinition<T> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
          {
              return (database.GetCollection<T>(collectionName).Find(filter, options)).FirstOrDefault();
          });
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FirstOrDefault(FilterDefinition<T> filter, FindOptions options = null)
        {
            return FirstOrDefault(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FirstOrDefault(string collectionName, Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return readInfrastructure.Exec(database =>
            {
                return (database.GetCollection<T>(collectionName).Find(filter, options)).FirstOrDefault();
            });
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return FirstOrDefault(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 异步获取第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(string collectionName, FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FirstOrDefaultAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await readInfrastructure.Exec(async database =>
            {
                return await (await database.GetCollection<T>(collectionName).FindAsync(filter, options, cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            });
        }
        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, FindOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return await FirstOrDefaultAsync(collectionTypeName, filter, options, cancellationToken);
        }
    }
}