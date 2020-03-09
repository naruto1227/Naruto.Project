
using System;

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class PersistedGrant : BaseMongo.Model.IMongoEntity
    {
        public string Key { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 同意的主题Id
        /// </summary>
        public string SubjectId { get; set; }
        /// <summary>
        /// 同意的客户端Id名称
        /// </summary>
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }
    }
}