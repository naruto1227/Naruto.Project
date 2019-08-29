using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 从库的 连接字符串的配置
    /// </summary>
    public class SlaveDbConnection
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否启用的状态
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
    }
}
