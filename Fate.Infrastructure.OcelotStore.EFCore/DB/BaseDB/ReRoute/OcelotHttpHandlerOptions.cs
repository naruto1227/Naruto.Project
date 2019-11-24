using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{

    [Table("OcelotHttpHandlerOptions")]
    /// <summary>
    /// （单条记录）
    /// </summary>
    public class OcelotHttpHandlerOptions : Base.Model.IEntity
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
        /// 当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点 
        /// </summary>
        public int IsReRouteOrGlobal { get; set; }

        public bool AllowAutoRedirect { get; set; }
        public bool UseCookieContainer { get; set; }
        public bool UseTracing { get; set; }
        public bool UseProxy { get; set; }
    }
}
