using Fate.Infrastructure.Mongo.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Mongo.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongo的连接实例工厂获取
    /// </summary>
    public interface IMongoClientFactory
    {
        /// <summary>
        /// 获取mongo 客户端
        /// </summary>
        /// <returns></returns>
        Task<IMongoClient> GetMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IMongoClient GetMongoClient<TMongoContext>() where TMongoContext : MongoContext;

        /// <summary>
        /// 获取读的mongo客户端
        /// </summary>
        /// <returns></returns>
        IMongoClient GetReadMongoClient<TMongoContext>() where TMongoContext : MongoContext;

        /// <summary>
        /// 获取读的mongo客户端
        /// </summary>
        /// <returns></returns>
        Task<IMongoClient> GetReadMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext;
    }
}
