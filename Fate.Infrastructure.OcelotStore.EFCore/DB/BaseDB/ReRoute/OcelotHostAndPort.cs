using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace Fate.Infrastructure.OcelotStore.EFCore.DB
{
    [Table("OcelotHostAndPort")]
    /// <summary>
    /// 下游 的主机地址
    /// </summary>
    public class OcelotHostAndPort : Base.Model.IEntity
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
        /// 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
    }
}
