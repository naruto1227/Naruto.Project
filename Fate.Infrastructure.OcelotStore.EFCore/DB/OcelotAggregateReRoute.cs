using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{
    /// <summary>
    /// ocelot请求聚合的路由
    /// </summary>
    [Table("OcelotAggregateReRoute")]
    public class OcelotAggregateReRoute : Base.Model.IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 请求聚合的路由key  （多个逗号隔开）
        /// </summary>。
        public string ReRouteKeys { get; set; }

        //public List<string> ReRouteKeys { get; set; }

        //public List<AggregateReRouteConfig> ReRouteKeysConfig { get; set; }

        /// <summary>
        /// 上游请求模板
        /// </summary>
        public string UpstreamPathTemplate { get; set; }
        /// <summary>
        /// 上有主机
        /// </summary>
        public string UpstreamHost { get; set; }
        /// <summary>
        /// 标识着传递的上游的url和上游的模板的地址是否完全匹配 （例 true） 默认不区分大小写
        /// </summary>
        public bool ReRouteIsCaseSensitive { get; set; }

        /// <summary>
        /// 聚合器
        /// </summary>
        public string Aggregator { get; set; }
        /// <summary>
        /// 等级 
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// 上有请求方式  当前仅支持get
        /// </summary>
        public string UpstreamHttpMethod { get; set; } = "Get";
        //public List<string> UpstreamHttpMethod
        //{
        //    get { return new List<string> { "Get" }; }
        //}
        #region 误删

        public virtual IList<OcelotAggregateReRouteConfig> OcelotAggregateReRouteConfigs { get; set; }
        #endregion
    }
}
