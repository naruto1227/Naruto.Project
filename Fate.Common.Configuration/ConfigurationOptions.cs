using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 配置的参数
    /// </summary>
    public class ConfigurationOptions
    {
        /// <summary>
        /// 网关地址
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 环境类型
        /// </summary>
        public string EnvironmentType { get; set; }
    }
}
