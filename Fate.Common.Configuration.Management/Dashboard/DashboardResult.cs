using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 面板接口的返回值
    /// </summary>
    public class DashboardResult
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object rows { get; set; }
        /// <summary>
        /// 消息
        /// </summary>

        public string msg { get; set; } = "操作成功";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string failMsg { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
    }
}
