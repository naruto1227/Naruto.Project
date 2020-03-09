

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// oidc注销之后跳转地址
    /// </summary>
    
    public class ClientPostLogoutRedirectUri : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 重定向地址
        /// </summary>
        public string PostLogoutRedirectUri { get; set; }
    }
}