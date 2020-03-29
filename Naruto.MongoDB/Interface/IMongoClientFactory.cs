using Naruto.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.MongoDB.Interface
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
        Task<Tuple<IMongoClient, MongoContext>> GetMongoClientAsync<TMongoContext>() where TMongoContext : MongoContext;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Tuple<IMongoClient, MongoContext> GetMongoClient<TMongoContext>() where TMongoContext : MongoContext;
    }
}
