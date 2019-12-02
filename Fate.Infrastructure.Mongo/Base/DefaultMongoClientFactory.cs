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
        private readonly ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>> ReadOnlyPool;

        /// <summary>
        /// 读写的对象池
        /// </summary>
        private readonly ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>> WriteReadPool;

        public DefaultMongoClientFactory(IOptions<List<MongoContext>> _mongoContexts)
        {
            mongoContexts = _mongoContexts;
            ReadOnlyPool = new ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>>();
            WriteReadPool = new ConcurrentDictionary<string, Tuple<IMongoClient, MongoContext>>();
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
            var mongoClientInfo = WriteReadPool.Where(a => a.Key == contextType).Select(a => a.Value).FirstOrDefault();
            if (mongoClientInfo != null)
                return mongoClientInfo;

            //获取上下文信息
            var mongoContextInfo = mongoContexts.Value.Where(a => a.ContextTypeName == contextType).FirstOrDefault();
            mongoContextInfo.CheckNull();
            //实例化mongo客户端信息
            mongoClientInfo = new Tuple<IMongoClient, MongoContext>(new MongoClient(mongoContextInfo.ConnectionString), mongoContextInfo);
            //存储进字典
            WriteReadPool.TryAdd(contextType, mongoClientInfo);
            return mongoClientInfo;
        }
        /// <summary>
        /// 异步获取
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>

        public Task<Tuple<IMongoClient, MongoContext>> GetMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext
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

        public Tuple<IMongoClient, MongoContext> GetReadMongoClient<TMongoContext>() where TMongoContext : MongoContext
        {
            var contextType = typeof(TMongoContext).Name;
            //获取上下文信息
            var mongoContextInfo = mongoContexts.Value.Where(a => a.ContextTypeName == contextType).FirstOrDefault();
            mongoContextInfo.CheckNull();
            //验证是否开启读写分离
            if (mongoContextInfo.IsOpenMasterSlave == false)
            {
                return GetMongoClient<TMongoContext>();
            }
            //从内存中读取mongodb客户端
            var mongoClientInfo = ReadOnlyPool.Where(a => a.Key == contextType).Select(a => a.Value).FirstOrDefault();
            if (mongoClientInfo != null)
                return mongoClientInfo;
            //实例化mongo客户端信息
            mongoClientInfo = new Tuple<IMongoClient, MongoContext>(new MongoClient(mongoContextInfo.ReadOnlyConnectionString.FirstOrDefault()), mongoContextInfo);

            //存储进字典
            ReadOnlyPool.TryAdd(contextType, mongoClientInfo);
            return mongoClientInfo;
        }

        /// <summary>
        /// 获取只写的客户端
        /// </summary>
        /// <typeparam name="TMongoContext"></typeparam>
        /// <returns></returns>
        public Task<Tuple<IMongoClient, MongoContext>> GetReadMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext
        {
            return Task.Factory.StartNew(() =>
            {
                return GetReadMongoClient<TMongoContext>();
            });
        }
    }
}
