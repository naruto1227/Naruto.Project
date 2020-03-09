

using System;
using System.Collections.Generic;

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// 认证资源信息
    /// </summary>
    public class IdentityResource : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 指示此资源是否已启用且可以请求.默认true
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 标识资源的唯一名称。这是客户端将用于授权请求中的scope参数的值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该值将用于例如同意屏幕上
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 该值将用于例如同意屏幕上
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 指定用户是否可以在同意屏幕上取消选择范围(如果同意屏幕要实现此类功能).默认false
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 指定同意屏幕是否会强调此范围(如果同意屏幕要实现此功能).将此设置用于敏感或重要范围.默认false
        /// </summary>
        public bool Emphasize { get; set; }
        /// <summary>
        /// 指定此范围是否显示在发现文档中.默认true
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        /// <summary>
        /// 不可编辑
        /// </summary>
        public bool NonEditable { get; set; }

        public List<IdentityClaim> UserClaims { get; set; }
        public List<IdentityResourceProperty> Properties { get; set; }
    }
}
