

namespace Fate.Infrastructure.Id4.Entities
{
    
    /// <summary>
    /// 客户端的授权类型
    /// </summary>
    public class ClientGrantType : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 授权类型
        /// GrantTypes.ClientCredentials
        /// </summary>
        public string GrantType { get; set; }

    }
}