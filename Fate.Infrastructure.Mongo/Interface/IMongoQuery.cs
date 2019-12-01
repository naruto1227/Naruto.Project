using Fate.Infrastructure.BaseMongo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongodb的查询操作
    /// </summary>
    public interface IMongoQuery<T> where T : class, IEntity
    {

    }
}
