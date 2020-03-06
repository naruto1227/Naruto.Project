using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.MongoDB.Object
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongodb的上下文 配置mongo的参数
    /// 每次连接上下文需要继承此对象
    /// </summary>
    public abstract class MongoContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// 默认存储库
        /// </summary>
        public virtual string DataBase { get; set; }

        /// <summary>
        /// GridFS的bucket名称,(默认为当前机器的名称)
        /// </summary>
        public virtual string BucketName { get; set; } = Environment.MachineName;
    }
}
