
using System;

namespace Fate.Infrastructure.Id4.Entities
{
    [NoCollection]
    public abstract class Secret : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 秘钥描述,默认null
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// 密钥的类型
        /// IdentityServerConstants.SecretTypes
        /// </summary>
        public string Type { get; set; } = "SharedSecret";

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}