using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Naruto.OcelotStore.Entity
{

    [Table("OcelotQoSOptions")]
    /// <summary>
    /// 熔断 （单条记录）
    /// </summary>
    public class OcelotQoSOptions : BaseRepository.Model.IEntity
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
        ///  允许多少个异常请求
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; }
        /// <summary>
        /// 熔断的时间，单位为秒
        /// </summary>
        public int DurationOfBreak { get; set; }
        /// <summary>
        /// 如果下游请求的处理时间超过多少则自如将请求设置为超时
        /// </summary>
        public int TimeoutValue { get; set; }
    }
}
