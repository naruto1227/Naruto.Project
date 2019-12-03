using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo.Object
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
        public string ConnectionString { get; set; }

        /// <summary>
        /// 存储库
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 上下文类型名称
        /// </summary>
        public string ContextTypeName { get; set; }

        /// <summary>
        /// 是否开启读写分离的操作 (默认不开启)
        /// </summary>
        public bool IsOpenMasterSlave { get; set; } = false;

        /// <summary>
        /// 只读的连接字符串的key 当IsOpenMasterSlave为true时 必须设置
        /// </summary>
        public string[] ReadOnlyConnectionString { get; set; }

    }
}
