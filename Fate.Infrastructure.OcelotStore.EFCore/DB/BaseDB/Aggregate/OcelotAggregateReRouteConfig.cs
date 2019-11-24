using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{
    /// <summary>
    /// 请求聚合路由的配置
    /// </summary>
    [Table("OcelotAggregateReRouteConfig")]
    public class OcelotAggregateReRouteConfig : Base.Model.IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 路由key
        /// </summary>
        public string ReRouteKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JsonPath { get; set; }
    }
}
