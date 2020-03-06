using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.MongoDB.Interface;
using Fate.Infrastructure.MongoDB.Object;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// 仓储的默认实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultMongoRepository<TMongoContext> : IMongoRepository<TMongoContext> where TMongoContext : MongoContext
    {

        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 上下文的作用域参数
        /// </summary>
        private MongoContextOptions mongoContextOptions;

        public DefaultMongoRepository(IServiceProvider _serviceProvider, MongoContextOptions<TMongoContext> _mongoContextOptions)
        {
            serviceProvider = _serviceProvider;

            mongoContextOptions = _mongoContextOptions;
        }

        /// <summary>
        /// 切换库
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        public Task ChangeDataBase(string dataBase)
        {
            dataBase.CheckNull();
            return Task.Factory.StartNew(() =>
            {
                mongoContextOptions.ChangeDataBase = dataBase;
            });
        }
        /// <summary>
        /// 增删改操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCommand<T> Command<T>() where T : class => serviceProvider.GetRequiredService<IMongoCommand<T, TMongoContext>>();

        /// <summary>
        /// 获取查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoQuery<T> Query<T>() where T : class => serviceProvider.GetRequiredService<IMongoQuery<T, TMongoContext>>();

        /// <summary>
        /// database的操作
        /// </summary>
        /// <returns></returns>
        public IMongoDataBase DataBase() => serviceProvider.GetRequiredService<IMongoDataBaseInfrastructure<TMongoContext>>();

        /// <summary>
        /// 索引的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoIndex<T> Index<T>() where T : class => serviceProvider.GetRequiredService<IMongoIndexInfrastructure<T, TMongoContext>>();


        /// <summary>
        /// 操作GridFS对象进行文件操作
        /// </summary>
        /// <returns></returns>
        public IMongoGridFS GridFS() => serviceProvider.GetRequiredService<IMongoGridFS<TMongoContext>>();
    }
}
