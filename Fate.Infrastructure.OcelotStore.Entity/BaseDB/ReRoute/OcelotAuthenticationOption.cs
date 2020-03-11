using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fate.Infrastructure.OcelotStore.Entity
{
    /// <summary>
    /// (单条记录)
    /// </summary>
    [Table("OcelotAuthenticationOption")]
    /// <summary>
    /// 认证中心
    /// </summary>
    public class OcelotAuthenticationOption: BaseRepository.Model.IEntity
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
        public string AuthenticationProviderKey { get; set; }
        /// <summary>
        /// 允许的范围 多个参数逗号分隔
        /// </summary>
        public string AllowedScopes { get; set; }
        //public List<string> AllowedScopes { get; set; }
    }
}
