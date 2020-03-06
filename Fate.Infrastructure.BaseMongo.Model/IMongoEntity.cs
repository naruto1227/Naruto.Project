using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Fate.Infrastructure.BaseMongo.Model
{
    [BsonIgnoreExtraElements(Inherited = true)]
    /// <summary>
    /// 张海波
    /// 2019-12-1
    /// mongo实体
    /// </summary>
    public abstract class IMongoEntity : IMongoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        /// <summary>
        /// mongodb的主键id ,当实体中不存在Id或者id或者_id 字段的时候
        /// 此实体需要继承当前接口
        /// </summary>
        public virtual ObjectId Id { get; set; }
    }
    /// <summary>
    /// 实体模型
    /// </summary>
    public interface IMongoModel
    {

    }
}
