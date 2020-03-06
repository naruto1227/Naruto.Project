using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.MongoDB.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongodb的仓储接口
    /// </summary>
    public interface IMongoRepository<TMongoContext> where TMongoContext : MongoContext
    {
        /// <summary>
        /// 切换库
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        Task ChangeDataBase(string dataBase);

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

        /// <summary>
        /// database的操作
        /// </summary>
        /// <returns></returns>
        IMongoDataBase DataBase();

        /// <summary>
        /// 索引的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IMongoIndex<T> Index<T>() where T : class;
        /// <summary>
        /// 操作GridFS对象进行文件操作
        /// </summary>
        /// <returns></returns>
        IMongoGridFS GridFS();
    }
}
