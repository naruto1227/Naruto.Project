

namespace Fate.Infrastructure.Id4.Entities
{
    
    /// <summary>
    /// oidc 登录成功的跳转地址
    /// </summary>
    public class ClientRedirectUri : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 重定向地址
        /// </summary>
        public string RedirectUri { get; set; }

    }
}