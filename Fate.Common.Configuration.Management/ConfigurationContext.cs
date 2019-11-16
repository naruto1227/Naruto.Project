using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 配置中心上下文抽象
    /// </summary>
    public abstract class ConfigurationContext
    {
        /// <summary>
        /// 请求的上下文
        /// </summary>
        public virtual HttpContext HttpContext { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; internal set; }
    }
}
