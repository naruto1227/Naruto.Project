

using System.Collections.Generic;

namespace Naruto.Id4.Entities
{
    
    /// <summary>
    /// api的范围
    /// </summary>
    public class ApiScope : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 范围的唯一名称。这是客户端将用于授权/令牌请求中的scope参数的值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// 该值可以在例如同意屏幕上使用
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// 该值可以在例如同意屏幕上使用
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 指定用户是否可以在同意屏幕上取消选择范围(如果同意屏幕要实现此类功能).默认true
        /// </summary>
        public bool Required { get; set; } = true;
        /// <summary>
        /// 指定同意屏幕是否会强调此范围(如果同意屏幕要实现此功能).将此设置用于敏感或重要范围.默认false
        /// </summary>
        public bool Emphasize { get; set; } = false;
        /// <summary>
        /// 指定此范围是否显示在发现文档中.默认true
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
       

        public List<ApiScopeClaim> UserClaims { get; set; }
    }
}