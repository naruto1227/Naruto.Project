using Fate.Infrastructure.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.MongoDB.Interface
{


    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo基础设施的通用接口
    /// </summary>
    public interface IMongoInfrastructureBase
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
    public interface IMongoInfrastructureBase<TMongoContext> : IMongoInfrastructureBase where TMongoContext : MongoContext
    {

    }

}
