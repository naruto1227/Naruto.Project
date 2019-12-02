using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongodb的仓储接口
    /// </summary>
    public interface IMongoRepository<TMongoContext> where TMongoContext : MongoContext
    {
        /// <summary>
        /// 查询接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IMongoQuery<T> Query<T>() where T : class;

        /// <summary>
        /// 增删改操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IMongoCommand<T> Command<T>() where T : class;
    }
}
