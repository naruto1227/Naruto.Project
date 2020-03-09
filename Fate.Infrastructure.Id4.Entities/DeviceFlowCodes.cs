using System;

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// 设备代码
    /// </summary>
    public class DeviceFlowCodes : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 设备代码
        /// </summary>
        /// <value>
        /// The device code.
        /// </value>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        /// <value>
        /// The user code.
        /// </value>
        public string UserCode { get; set; }

        /// <summary>
        /// 同意的主题Id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId { get; set; }

        /// <summary>
        /// 同意的客户端id名称
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }
    }
}