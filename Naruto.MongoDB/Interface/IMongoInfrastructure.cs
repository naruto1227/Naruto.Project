using Naruto.MongoDB.Object;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.MongoDB.Interface
{


    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo基础设施的通用接口
    /// </summary>
    public interface IMongoInfrastructure
    {
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        TResult Exec<TResult>(Func<IMongoDatabase, TResult> action);
        /// <summary>
        /// 执行操作 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        void Exec(Action<IMongoDatabase> action);
    }

    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo基础设施的读写抽象接口
    /// </summary>
    public interface IMongoInfrastructure<TMongoContext> : IMongoInfrastructure where TMongoContext : MongoContext
    {

    }


    /// <summary>
    /// 张海波
    /// 2020-03-04
    /// mongo使用GridFS操作接口
    /// </summary>
    public interface IMongoGridFSInfrastructure
    {
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        TResult Exec<TResult>(Func<IGridFSBucket, TResult> action);
        /// <summary>
        /// 执行操作 无返回值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        void Exec(Action<IGridFSBucket> action);
    }

    /// <summary>
    /// 张海波
    /// 2020-03-04
    /// mongo使用GridFS操作接口
    /// </summary>
    public interface IMongoGridFSInfrastructure<TMongoContext> : IMongoGridFSInfrastructure where TMongoContext : MongoContext
    {

    }
}
