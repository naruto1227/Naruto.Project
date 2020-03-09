

namespace Fate.Infrastructure.Id4.Entities
{
    [NoCollection]
    /// <summary>
    /// 客户端IdPs(入侵检测与防御系统)
    /// </summary>
    public class ClientIdPRestriction : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 指定可以与此客户端一起使用的外部IdP(如果列表为空，则允许所有IdP).默认空
        /// </summary>
        public string Provider { get; set; }
    }
}