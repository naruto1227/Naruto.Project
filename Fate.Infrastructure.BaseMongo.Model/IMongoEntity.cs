using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Fate.Infrastructure.BaseMongo.Model
{
    [BsonIgnoreExtraElements(Inherited = true)]
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// 所有的mongo实体需要继承此抽象类
    /// </summary>
    public abstract class IMongoEntity 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        /// <summary>
        /// mongodb的主键id  不可改 字段名称必须为此名称
        /// </summary>
        public virtual ObjectId _id { get; set; }
    }
}
