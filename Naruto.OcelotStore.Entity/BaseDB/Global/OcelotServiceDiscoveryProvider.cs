using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Naruto.OcelotStore.Entity
{
    /// <summary>
    /// 服务发现的实体配置
    /// 单条记录
    /// </summary>
    [Table("OcelotServiceDiscoveryProvider")]
    public class OcelotServiceDiscoveryProvider : BaseRepository.Model.IEntity
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
        /// 服务发现的主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 服务发现的 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 服务发现的类型 默认 使用Consul
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConfigurationKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PollingInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Namespace { get; set; }
    }
}
