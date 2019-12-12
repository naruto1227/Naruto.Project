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
        /// mongoclient的对象池
        /// </summary>
        private readonly ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>> MongoClientPool;

        public DefaultMongoClientFactory(IOptions<List<MongoContext>> _mongoContexts)
        {
            mongoContexts = _mongoContexts;
            MongoClientPool = new ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>>();
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>
        public Tuple<IMongoClient, MongoContext> GetMongoClient<TMongoContext>() where TMongoContext : MongoContext
        {
            var contextType = typeof(TMongoContext).Name;
            //从内存中读取mongodb客户端
            var mongoClientInfo = MongoClientPool.Where(a => a.Key == contextType).Select(a => a.Value).FirstOrDefault();
            if (mongoClientInfo != null)
                return mongoClientInfo;

            //获取上下文信息
            var mongoContextInfo = mongoContexts.Value.Where(a => a.ContextTypeName == contextType).FirstOrDefault();
            mongoContextInfo.CheckNull();
            //实例化mongo客户端信息
            mongoClientInfo = new Tuple<IMongoClient, MongoContext>(new MongoClient(mongoContextInfo.ConnectionString), mongoContextInfo);
            //存储进字典
            MongoClientPool.TryAdd(contextType, mongoClientInfo);
            return mongoClientInfo;
        }
        /// <summary>
        /// 异步获取
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>

        public Task<Tuple<IMongoClient, MongoContext>> GetMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext
        {
            return Task.FromResult(GetMongoClient<TMongoContext>());
        }

        private MongoClientSettings MongoClientSettings()
        {
            return new MongoClientSettings()
            {
            };
        }
    }
}
