using MongoDB.Bson;
using System;

namespace Fate.Infrastructure.BaseMongo.Model
{

    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// 所有的mongo实体需要继承此抽象类
    /// </summary>
    public abstract class IMongoEntity : IEntity
    {
        public virtual ObjectId MongoId { get; set; }
    }

}
