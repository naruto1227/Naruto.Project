

using Naruto.BaseMongo.Model;
using System;
using System.Collections.Generic;

namespace Naruto.Id4.Entities
{
    /// <summary>
    /// api资源
    /// </summary>
    public class ApiResource:IMongoEntity
    {
        /// <summary>
        /// 指示此资源是否已启用且可以请求.默认true
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// API的唯一名称.此值用于内省身份验证,并将添加到传出访问令牌的受众
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该值可以在例如同意屏幕上使用
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 该值可以在例如同意屏幕上使用
        /// </summary>
        public string Description { get; set; }
      
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastAccessed { get; set; }
        /// <summary>
        /// 不可编辑的
        /// </summary>
        public bool NonEditable { get; set; }

        public List<ApiSecret> Secrets { get; set; }
        public List<ApiScope> Scopes { get; set; }
        public List<ApiResourceClaim> UserClaims { get; set; }
        public List<ApiResourceProperty> Properties { get; set; }
    }
}
