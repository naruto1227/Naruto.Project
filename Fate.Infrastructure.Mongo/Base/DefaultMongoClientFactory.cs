using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;

namespace Fate.Infrastructure.Mongo.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 客户端工厂的默认实现
    /// </summary>
    public class DefaultMongoClientFactory : IMongoClientFactory
    {
        private readonly IOptions<List<MongoContext>> mongoContexts;
        /// <summary>
        /// 只读的对象池
        /// </summary>
        private readonly ConcurrentDictionary<string, IMongoClient> ReadOnlyPool;

        /// <summary>
        /// 读写的对象池
        /// </summary>
        private readonly ConcurrentDictionary<string, IMongoClient> WriteReadPool;

        public DefaultMongoClientFactory(IOptions<List<MongoContext>> _mongoContexts)
        {
            mongoContexts = _mongoContexts;
            ReadOnlyPool = new ConcurrentDictionary<string, IMongoClient>();
            WriteReadPool = new ConcurrentDictionary<string, IMongoClient>();
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>
        public IMongoClient GetMongoClient<TMongoContext>() where TMongoContext : MongoContext
        {
            var contextType = typeof(TMongoContext).Name;
            //从内存中读取mongodb客户端
            var mongoClientInfo = WriteReadPool.Where(a => a.Key == contextType).Select(a => a.Value).FirstOrDefault();
            if (mongoClientInfo != null)
                return mongoClientInfo;

            //获取上下文信息
            var mongoContextInfo = mongoContexts.Value.Where(a => a.ContextTypeName == contextType).FirstOrDefault();
            mongoContextInfo.CheckNull();
            mongoClientInfo = new MongoClient(mongoContextInfo.ConnectionString);
            //存储进字典
            WriteReadPool.TryAdd(contextType, mongoClientInfo);
            return mongoClientInfo;
        }
        /// <summary>
        /// 异步获取
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>

        public Task<IMongoClient> GetMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext
        {
            return Task.Factory.StartNew(() =>
             {
                 return GetMongoClient<TMongoContext>();
             });
        }

        /// <summary>
        /// 获取只读的客户端
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>

        public IMongoClient GetReadMongoClient<TMongoContext>() where TMongoContext : MongoContext
        {
            var contextType = typeof(TMongoContext).Name;
            //从内存中读取mongodb客户端
            var mongoClientInfo = ReadOnlyPool.Where(a => a.Key == contextType).Select(a => a.Value).FirstOrDefault();
            if (mongoClientInfo != null)
                return mongoClientInfo;

            //获取上下文信息
            var mongoContextInfo = mongoContexts.Value.Where(a => a.ContextTypeName == contextType).FirstOrDefault();
            mongoContextInfo.CheckNull();
            mongoClientInfo = new MongoClient(mongoContextInfo.ReadOnlyConnectionString.FirstOrDefault());
            //存储进字典
            ReadOnlyPool.TryAdd(contextType, mongoClientInfo);
            return mongoClientInfo;
        }

        /// <summary>
        /// 获取只写的客户端
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>
        public Task<IMongoClient> GetReadMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext
        {
            return Task.Factory.StartNew(() =>
            {
                return GetReadMongoClient<TMongoContext>();
            });
        }
    }
}
