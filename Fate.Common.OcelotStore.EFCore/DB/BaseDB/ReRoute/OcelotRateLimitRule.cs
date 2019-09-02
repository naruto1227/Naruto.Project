using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Common.OcelotStore.EFCore.DB
{
    [Table("OcelotRateLimitRule")]
    /// <summary>
    /// 请求限流的实体 (单条记录)
    /// </summary>
    public class OcelotRateLimitRule : Base.Model.IEntity
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
        /// 当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点  2 自定义路由节点
        /// </summary>
        public int IsReRouteOrGlobal { get; set; }
        /// <summary>
        /// 客户端白名单 (多个逗号分隔)
        /// 一个字符串数组，在请求头中包含ClientId=xxx的请求不受限流控制，其中ClientId这个key可以修改，xxx表示配置的白名单。
        /// </summary>
        public string ClientWhitelist { get; set; }

        //public List<string> ClientWhitelist { get; set; }

        /// <summary>
        ///是否启用限流
        /// </summary>
        public bool EnableRateLimiting { get; set; }

        /// <summary>
        /// 统计时间段：1s, 5m, 1h, 1d
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 多少秒之后客户端可以重试
        /// </summary>

        public double PeriodTimespan { get; set; }

        /// <summary>
        /// 在统计时间段内允许的最大请求数量
        /// </summary>
        public long Limit { get; set; }
    }
}
