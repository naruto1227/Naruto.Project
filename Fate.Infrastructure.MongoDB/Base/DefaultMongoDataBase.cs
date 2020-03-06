using Fate.Infrastructure.MongoDB.Interface;
using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Infrastructure.MongoDB.Base
{
    /// <summary>
    /// 张海波
    /// 2019-12-5
    /// IMongoDataBase基础功能接口实现
    /// </summary>
    /// <typeparam name="TMongoContext"></typeparam>
    public class DefaultMongoDataBase<TMongoContext> : IMongoDataBaseInfrastructure<TMongoContext> where TMongoContext : MongoContext
    {

        private readonly IMongoInfrastructure<TMongoContext> infrastructure;

        public DefaultMongoDataBase(IMongoInfrastructure<TMongoContext> _infrastructure)
        {
            infrastructure = _infrastructure;
        }

        /// <summary>
        /// 创建集合
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void CreateCollection(string name, CreateCollectionOptions options = null)
        {
            infrastructure.Exec(database =>
            {
                database.CreateCollection(name, options);
            });
        }


        public Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
             {
                 return database.CreateCollectionAsync(name, options, cancellationToken);
             });
        }
        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="name"></param>
        public void DropCollection(string name)
        {
            infrastructure.Exec(database =>
            {
                database.DropCollection(name);
            });
        }

        public Task DropCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.DropCollectionAsync(name, cancellationToken);
            });
        }
        /// <summary>
        /// 获取集合的名字
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<string> ListCollectionNames(ListCollectionNamesOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.ListCollectionNames(options).ToList();
            });
        }

        public async Task<List<string>> ListCollectionNamesAsync(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                return await (await database.ListCollectionNamesAsync(options, cancellationToken)).ToListAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// 获取集合信息
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<BsonDocument> ListCollections(ListCollectionsOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.ListCollections(options).ToList();
            });
        }

        public async Task<List<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                return await (await database.ListCollectionsAsync(options, cancellationToken)).ToListAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// 重命名集合名称
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="options"></param>
        public void RenameCollection(string oldName, string newName, RenameCollectionOptions options = null)
        {
            infrastructure.Exec(database =>
           {
               database.RenameCollection(oldName, newName, options);
           });
        }

        public Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
             {
                 return database.RenameCollectionAsync(oldName, newName, options, cancellationToken);
             });
        }
        /// <summary>
        /// 运行命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="readPreference"></param>
        /// <returns></returns>
        public T RunCommand<T>(Command<T> command, ReadPreference readPreference = null) where T : class
        {
            return infrastructure.Exec(database =>
            {
                return database.RunCommand(command, readPreference);
            });
        }

        public Task<T> RunCommandAsync<T>(Command<T> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default) where T : class
        {
            return infrastructure.Exec(database =>
            {
                return database.RunCommandAsync(command, readPreference);
            });
        }
    }
}
