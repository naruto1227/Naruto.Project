using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Common.OcelotStore.EFCore.DB
{
    /// <summary>
    /// 请求限流的 全局配置 
    /// </summary>
    [Table("OcelotRateLimitOptions")]
    public class OcelotRateLimitOptions : Base.Model.IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 修改在请求头中传递的ClientId 头 的key
        /// </summary>
        public string ClientIdHeader { get; set; } = "ClientId";
        /// <summary>
        /// 限流返回的消息提示
        /// </summary>
        public string QuotaExceededMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RateLimitCounterPrefix { get; set; } = "ocelot";
        /// <summary>
        /// 是否启用限流的头
        /// </summary>
        public bool DisableRateLimitHeaders { get; set; }
        /// <summary>
        /// 响应的http状态码
        /// </summary>
        public int HttpStatusCode { get; set; } = 429;
    }
}
