using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo读写接口的默认实现
    /// </summary>
    /// <typeparam name="TMongoContext"></typeparam>
    public class MongoWriteReadInfrastructure<TMongoContext> : IMongoWriteReadInfrastructure<TMongoContext> where TMongoContext : MongoContext
    {

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private readonly MongoContextOptions mongoContextOptions;
        /// <summary>
        /// mongo存储库接口
        /// </summary>
        private Lazy<IMongoDatabase> mongoDatabase;

        /// <summary>
        /// mongo客户端
        /// </summary>
        private readonly IMongoClient mongoClient;

        public MongoWriteReadInfrastructure(MongoContextOptions _mongoContextOptions, IMongoClientFactory _mongoClientFactory)
        {
            mongoContextOptions = _mongoContextOptions;
            //获取mongo客户端信息
            var mongoClientInfo = _mongoClientFactory.GetMongoClient<TMongoContext>();
            mongoClient = mongoClientInfo.Item1;
            mongoDatabase = new Lazy<IMongoDatabase>(mongoClient.GetDatabase(mongoClientInfo.Item2.DataBase));
        }

        public TResult Exec<TResult>(Func<IMongoDatabase, TResult> action)
        {
            ChangeDataBase();
            return action(mongoDatabase.Value);
        }

        public void Exec(Action<IMongoDatabase> action)
        {
            ChangeDataBase();
            action(mongoDatabase.Value);
        }
        /// <summary>
        /// 切换存储库
        /// </summary>
        private void ChangeDataBase()
        {
            if (!mongoContextOptions.DataBase.IsNullOrEmpty())
            {
                mongoDatabase = new Lazy<IMongoDatabase>(mongoClient.GetDatabase(mongoContextOptions.DataBase));
            }
        }
    }


    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo只读接口的默认实现
    /// </summary>
    /// <typeparam name="TMongoContext"></typeparam>
    public class MongoReadInfrastructure<TMongoContext> : IMongoReadInfrastructure<TMongoContext> where TMongoContext : MongoContext
    {

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private readonly MongoContextOptions mongoContextOptions;
        /// <summary>
        /// mongo存储库接口
        /// </summary>
        private Lazy<IMongoDatabase> mongoDatabase;

        /// <summary>
        /// mongo客户端
        /// </summary>
        private readonly IMongoClient mongoClient;


        public MongoReadInfrastructure(MongoContextOptions _mongoContextOptions, IMongoClientFactory _mongoClientFactory)
        {
            mongoContextOptions = _mongoContextOptions;
            //获取mongo客户端信息
            var mongoClientInfo = _mongoClientFactory.GetReadMongoClient<TMongoContext>();
            mongoClient = mongoClientInfo.Item1;
            mongoDatabase = new Lazy<IMongoDatabase>(mongoClient.GetDatabase(mongoClientInfo.Item2.DataBase));
        }

        public TResult Exec<TResult>(Func<IMongoDatabase, TResult> action)
        {
            ChangeDataBase();
            return action(mongoDatabase.Value);
        }

        public void Exec(Action<IMongoDatabase> action)
        {
            ChangeDataBase();
            action(mongoDatabase.Value);
        }
        /// <summary>
        /// 切换存储库
        /// </summary>
        private void ChangeDataBase()
        {
            if (!mongoContextOptions.DataBase.IsNullOrEmpty())
            {
                mongoDatabase = new Lazy<IMongoDatabase>(mongoClient.GetDatabase(mongoContextOptions.DataBase));
            }
        }
    }
}
