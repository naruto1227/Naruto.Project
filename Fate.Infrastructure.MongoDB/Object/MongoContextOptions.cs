using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.MongoDB.Object
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 存储mongo每个作用域的上下文的参数配置
    /// </summary>
    public abstract class MongoContextOptions
    {
        /// <summary>
        /// 存储库
        /// </summary>
        internal virtual string ChangeDataBase { get; set; } = default;
        /// <summary>
        /// 使用GridFS操作的bucketname
        /// </summary>
        internal virtual string BucketName { get; set; } = default;
    }

    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 存储mongo每个作用域的上下文的参数配置
    /// </summary>
    public class MongoContextOptions<TMongoContext> : MongoContextOptions where TMongoContext : MongoContext
    {
    }
}
