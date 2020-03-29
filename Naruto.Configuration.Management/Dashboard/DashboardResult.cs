using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 面板接口的返回值
    /// </summary>
    public class DashboardResult
    {
        public int code { get; set; } = 0;
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 消息
        /// </summary>

        public string msg { get; set; } = "操作成功";
        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; }
    }
}
