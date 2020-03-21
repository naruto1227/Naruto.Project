using Fate.Infrastructure.MongoDB.Interface;
using Fate.Infrastructure.MongoDB.Object;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Fate.Infrastructure.MongoDB.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo基础设施接口的默认实现
    /// </summary>
    /// <typeparam name="TMongoContext"></typeparam>
    public class MongoInfrastructure<TMongoContext> : IMongoInfrastructure<TMongoContext> where TMongoContext : MongoContext
    {

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private readonly MongoContextOptions currentContextOptions;
        /// <summary>
        /// mongo存储库接口
        /// </summary>
        private Lazy<IMongoDatabase> currentMongoDatabase;

        /// <summary>
        /// mongo客户端
        /// </summary>
        private readonly IMongoClient currentMongoClient;

        public MongoInfrastructure(MongoContextOptions<TMongoContext> _mongoContextOptions, IMongoClientFactory _mongoClientFactory)
        {
            currentContextOptions = _mongoContextOptions;
            //获取mongo客户端信息
            var mongoClientInfo = _mongoClientFactory.GetMongoClient<TMongoContext>();
            currentMongoClient = mongoClientInfo.Item1;
            currentMongoDatabase = Build(mongoClientInfo.Item2.DataBase);
        }

        public TResult Exec<TResult>(Func<IMongoDatabase, TResult> action)
        {
            ChangeDataBase();
            return action(currentMongoDatabase.Value);
        }

        public void Exec(Action<IMongoDatabase> action)
        {
            ChangeDataBase();
            action(currentMongoDatabase.Value);
        }
        /// <summary>
        /// 切换存储库
        /// </summary>
        private void ChangeDataBase()
        {
            if (!currentContextOptions.ChangeDataBase.IsNullOrEmpty() && !currentMongoDatabase.Value.DatabaseNamespace.DatabaseName.Equals(currentContextOptions.ChangeDataBase))
                currentMongoDatabase = Build(currentContextOptions.ChangeDataBase);


        }

        private Lazy<IMongoDatabase> Build(string database)
        {
            return new Lazy<IMongoDatabase>(currentMongoClient.GetDatabase(database));
        }
    }

    /// <summary>
    /// 张海波
    /// 2020-03-04
    /// mongo使用GridFS操作接口
    /// </summary>
    public class DefaultMongoGridFSInfrastructure<TMongoContext> : IMongoGridFSInfrastructure<TMongoContext> where TMongoContext : MongoContext
    {

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private readonly MongoContextOptions currentContextOptions;

        /// <summary>
        /// GridFS对象
        /// </summary>
        private IGridFSBucket gridFSBucket;

        /// <summary>
        /// mongo客户端
        /// </summary>
        private readonly IMongoClient currentMongoClient;
        /// <summary>
        /// 当前上下文
        /// </summary>
        private readonly MongoContext currentMongoContext;

        public DefaultMongoGridFSInfrastructure(MongoContextOptions<TMongoContext> _mongoContextOptions, IMongoClientFactory _mongoClientFactory, IServiceProvider serviceProvider)
        {
            currentContextOptions = _mongoContextOptions;
            //获取mongo客户端信息
            var mongoClientInfo = _mongoClientFactory.GetMongoClient<TMongoContext>();
            //获取客户端
            currentMongoClient = mongoClientInfo.Item1;
            //获取当前上下文信息
            currentMongoContext = serviceProvider.GetService(MergeNamedType.Get(typeof(TMongoContext).Name)) as TMongoContext;
            //构建GridFS对象   
            gridFSBucket = Build(currentMongoClient.GetDatabase(mongoClientInfo.Item2.DataBase), currentMongoContext.BucketName);
        }

        public TResult Exec<TResult>(Func<IGridFSBucket, TResult> action)
        {
            Change();
            return action(gridFSBucket);
        }

        public void Exec(Action<IGridFSBucket> action)
        {
            Change();
            action(gridFSBucket);
        }

        private GridFSBucket Build(IMongoDatabase mongoDatabase, string bucketName)
        {
            return new GridFSBucket(mongoDatabase, new GridFSBucketOptions
            {
                BucketName = bucketName
            });
        }

        /// <summary>
        /// 切换
        /// </summary>
        private void Change()
        {
            //验证是否切换数据库
            var isChangeDatabase = false;
            if (!currentContextOptions.ChangeDataBase.IsNullOrEmpty() && !gridFSBucket.Database.DatabaseNamespace.DatabaseName.Equals(currentContextOptions.ChangeDataBase))
                isChangeDatabase = true;

            //验证是否切换bucket
            var isChangeBucketName = false;
            if (!currentContextOptions.BucketName.IsNullOrEmpty() && !currentContextOptions.BucketName.Equals(gridFSBucket.Options.BucketName))
                isChangeBucketName = true;

            //验证是否更改
            if (isChangeDatabase == false && isChangeBucketName == false)
            {
                return;
            }
            //重新构建对象
            gridFSBucket = Build(isChangeDatabase ? currentMongoClient.GetDatabase(currentContextOptions.ChangeDataBase) : gridFSBucket.Database, isChangeBucketName ? currentContextOptions.BucketName : gridFSBucket.Options.BucketName);
        }

    }
}
