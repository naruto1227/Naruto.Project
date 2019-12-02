using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Fate.Infrastructure.Mongo.Base
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
        public DefaultMongoRepository(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// 增删改操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCommand<T> Command<T>() where T : class
        {
            return serviceProvider.GetRequiredService<IMongoCommand<T, TMongoContext>>();
        }
        /// <summary>
        /// 获取查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoQuery<T> Query<T>() where T : class
        {
            return serviceProvider.GetRequiredService<IMongoQuery<T, TMongoContext>>();
        }
    }
}
