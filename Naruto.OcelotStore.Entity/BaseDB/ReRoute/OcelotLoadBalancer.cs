using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace Naruto.OcelotStore.Entity
{
    [Table("OcelotLoadBalancer")]
    /// <summary>
    /// 负载均衡的实体 (单条记录)
    /// </summary>
    public class OcelotLoadBalancer : BaseRepository.Model.IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 父节点的id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 当前记录是 路由节点下面的 还是 全局配置节点下的 0 路由节点 1 全局配置节点 
        /// </summary>
        public int IsReRouteOrGlobal { get; set; }
        /// <summary>
        /// 负载均衡 的类型
        /// </summary>
        public string Type { get; set; }

        public string Key { get; set; }

        public int Expiry { get; set; }
    }
}
