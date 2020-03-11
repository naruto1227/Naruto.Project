using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.Entity
{
    /// <summary>
    /// 缓存的配置
    /// Ocelot可以对下游请求结果进行缓存 ，目前缓存的功能还不是很强大。它主要是依赖于CacheManager 来实现的，我们只需要在路由下添加以下配置即可
    /// 单条记录
    /// </summary>
    [Table("OcelotCacheOptions")]
    public class OcelotCacheOptions : BaseRepository.Model.IEntity
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
        /// 
        /// </summary>
        public int TtlSeconds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Region { get; set; }
    }
}
