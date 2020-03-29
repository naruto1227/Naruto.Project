

using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Naruto.Id4.Entities
{
    public class Client : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 是否启用 默认启用
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 客户端id名称(唯一的)
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 客户端描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 协议类型
        /// IdentityServerConstants.ProtocolTypes
        /// </summary>
        public string ProtocolType { get; set; } = "oidc";
        /// <summary>
        /// 指定此客户端是否需要密钥才能从令牌端点请求令牌.默认true
        /// </summary>

        public bool RequireClientSecret { get; set; } = true;

        /// <summary>
        /// 有关客户端的更多信息的URI(在同意屏幕上使用)
        /// </summary>
        public string ClientUri { get; set; }
        /// <summary>
        /// URI到客户端徽标(在同意屏幕上使用)
        /// </summary>
        public string LogoUri { get; set; }
        /// <summary>
        /// 指定是否需要同意屏幕.默认true
        /// </summary>
        public bool RequireConsent { get; set; } = true;
        /// <summary>
        /// 指定用户是否可以选择存储同意决策.默认true
        /// </summary>
        public bool AllowRememberConsent { get; set; } = true;
        /// <summary>
        /// 在请求id token和access token时，如果用户声明始终将其添加到id token而不是请求客户端使用userinfo endpoint.默认false
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;
        /// <summary>
        /// 指定使用基于授权代码的授权类型的客户端是否必须发送校验密钥
        /// </summary>
        public bool RequirePkce { get; set; }
        /// <summary>
        /// 指定使用PKCE的客户端是否可以使用纯文本代码质询(不推荐).默认false
        /// </summary>
        public bool AllowPlainTextPkce { get; set; } = false;
        /// <summary>
        /// 指定是否允许此客户端通过浏览器接收访问令牌
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; }
        /// <summary>
        /// 指定客户端的注销URI，以用于基于HTTP的前端通道注销
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }
        /// <summary>
        /// 指定是否应将用户的会话ID发送到FrontChannelLogoutUri
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// 指定客户端的注销URI，以用于基于HTTP的反向通道注销
        /// </summary>
        public string BackChannelLogoutUri { get; set; }
        /// <summary>
        /// 指定是否应在请求中将用户的会话ID发送到BackChannelLogoutUri
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// 指定此客户端是否可以请求刷新令牌
        /// </summary>
        public bool AllowOfflineAccess { get; set; } = false;
        /// <summary>
        /// 身份令牌的生命周期,以秒为单位.默认300(5分钟)
        /// </summary>
        public int IdentityTokenLifetime { get; set; } = 300;
        /// <summary>
        /// 访问令牌的生命周期,以秒为单位.默认为3600(1小时)
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 3600;
        /// <summary>
        /// 授权代码的生命周期,以秒为单位.默认为300(5分钟)
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; } = 300;
        /// <summary>
        /// 用户同意的生命周期,以秒为单位.默认null(无到期)
        /// </summary>
        public int? ConsentLifetime { get; set; } = null;
        /// <summary>
        /// 刷新令牌的最长生命周期,以秒为单位.默认2592000(30天)
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        /// <summary>
        /// 滑动刷新令牌的生命周期,以秒为单位.默认为1296000(15天)
        /// </summary>
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        /// <summary>
        /// 默认OneTime 刷新令牌时将更新刷新令牌句柄.ReUse 刷新令牌时.刷新令牌句柄将保持不变.
        /// </summary>
        public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;
        /// <summary>
        /// 获取或设置一个值，该值指示是否应在刷新令牌请求上更新访问令牌(及其声明)
        /// </summary>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        /// <summary>
        /// Absolute 刷新令牌将在固定时间点到期(由AbsoluteRefreshTokenLifetime指定)Sliding刷新令牌时,将刷新刷新令牌的生命周期(按SlidingRefreshTokenLifetime中指定的数量).生命周期不会超过AbsoluteRefreshTokenLifetime
        /// </summary>
        public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
        /// <summary>
        /// 指定访问令牌是引用令牌还是自包含JWT令牌.默认jwt
        /// </summary>
        public int AccessTokenType { get; set; } = 0;
        /// <summary>
        /// 指定此客户端是否可以仅使用本地帐户或外部IP.默认true
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;
        /// <summary>
        /// 指定JWT访问令牌是否应具有嵌入的唯一ID(通过jti声明)
        /// </summary>
        public bool IncludeJwtId { get; set; }
        /// <summary>
        /// true为每个流发送客户端声明.false如果不是,仅用于客户端凭证流.默认false
        /// </summary>

        public bool AlwaysSendClientClaims { get; set; }
        /// <summary>
        /// 如果设置,将以前缀为前缀客户端声明类型.默认为client_.目的是确保它们不会意外地与用户声明冲突
        /// </summary>
        public string ClientClaimsPrefix { get; set; } = "client_";
        /// <summary>
        /// 对于此客户端的用户,在成对的subjectId生成中使用的salt值
        /// </summary>
        public string PairWiseSubjectSalt { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastAccessed { get; set; }
        /// <summary>
        /// 自上次用户进行身份验证以来的最长持续时间(单位:秒).默认为null.您可以调整会话令牌的生命周期,以控制在使用Web应用程序时,用户需要重新输入凭据的时间和频率,而不是进行静默身份验证
        /// </summary>
        public int? UserSsoLifetime { get; set; }
        /// <summary>
        /// 指定用于客户端的用户代码的类型.否则使用默认值
        /// </summary>
        public string UserCodeType { get; set; }
        /// <summary>
        /// 设备代码的生命周期,以秒为单位.默认300(5分钟)
        /// </summary>
        public int DeviceCodeLifetime { get; set; } = 300;
        /// <summary>
        /// 不可编辑的
        /// </summary>
        public bool NonEditable { get; set; }
        /// <summary>
        /// 客户端密钥
        /// </summary>
        public List<ClientSecret> ClientSecrets { get; set; }
        /// <summary>
        /// 允许的授权类型
        /// </summary>
        public List<ClientGrantType> AllowedGrantTypes { get; set; }
        /// <summary>
        /// 允许的授权范围
        /// </summary>
        public List<ClientScope> AllowedScopes { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public List<ClientRedirectUri> RedirectUris { get; set; }
        /// <summary>
        /// 登出地址
        /// </summary>
        public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// 声明
        /// </summary>
        public List<ClientClaim> Claims { get; set; }
        /// <summary>
        /// 跨域地址
        /// </summary>
        public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public List<ClientProperty> Properties { get; set; }

        public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
    }
}