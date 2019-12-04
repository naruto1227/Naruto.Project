using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo.Object
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// 存储mongo每个作用域的上下文的参数配置
    /// </summary>
    public class MongoContextOptions
    {
        /// <summary>
        /// 存储库
        /// </summary>
        internal string ChangeDataBase { get; set; } = default;
    }
}
