using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Fate.Infrastructure.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 接口请求的参数配置
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// 接口访问的地址 默认地址(/api/configuration/data)
        /// </summary>
        public PathString RequestPath { get; set; } = new PathString("/api/configuration/data");
        /// <summary>
        /// 接口请求的方式 目前仅支持get和post (post需要传递json格式)
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 接口的授权过滤器
        /// </summary>
        public IEnumerable<IRequestAuthorizationFilters> Authorization { get; set; }
    }
}
