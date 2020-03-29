using Naruto.MongoDB.Object;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.MongoDB.Interface
{
    /// <summary>
    /// 
    /// 张海波
    /// 2019-12-5
    /// IMongoDataBase基础功能接口
    /// </summary>
    public interface IMongoDataBase
    {
        #region 同步
        /// <summary>
        /// 新建集合(数据表)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        void CreateCollection(string name, CreateCollectionOptions options = null);
        /// <summary>
        /// 删除集合(数据表)
        /// </summary>
        /// <param name="name"></param>
        void DropCollection(string name);

        /// <summary>
        /// 获取集合名称 （数据表）
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        List<string> ListCollectionNames(ListCollectionNamesOptions options = null);

        /// <summary>
        /// 获取所有集合信息
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        List<BsonDocument> ListCollections(ListCollectionsOptions options = null);

        /// <summary>
        /// 重新命名集合
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        void RenameCollection(string oldName, string newName, RenameCollectionOptions options = null);

        /// <summary>
        /// 运行命令脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="readPreference"></param>
        /// <returns></returns>
        T RunCommand<T>(Command<T> command, ReadPreference readPreference = null) where T : class;

        #endregion

        #region 异步

        /// <summary>
        /// 新建集合
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除集合(数据表)
        /// </summary>
        /// <param name="name"></param>
        Task DropCollectionAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取集合名称 （数据表）
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<string>> ListCollectionNamesAsync(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有的集合(数据表)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 重新命名集合(数据表)
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 运行命令脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="readPreference"></param>
        /// <returns></returns>
        Task<T> RunCommandAsync<T>(Command<T> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default) where T : class;

        #endregion
    }

    /// <summary>
    /// 张海波
    /// 2019-12-5
    /// IMongoDataBase基础功能泛型接口
    /// </summary>
    /// <typeparam name="TMongoContext"></typeparam>
    public interface IMongoDataBaseInfrastructure<TMongoContext> : IMongoDataBase where TMongoContext : MongoContext
    {

    }

}
