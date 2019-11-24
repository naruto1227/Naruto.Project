using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Fate.Infrastructure.Infrastructure;
using Fate.Infrastructure.Enum;
using Fate.Infrastructure.Interface;

namespace Fate.Infrastructure.Config
{
    /// <summary>
    /// 配置钉钉登录需要的数据
    /// </summary>
    public class DingDingConfig : ICommonClassSigleDependency
    {
        /// <summary>
        /// 应用的appid
        /// </summary>
        public const string DingDingAppkey = "";
        /// <summary>
        /// 应用的app密钥
        /// </summary>
        public const string DingDingAppSecret = "";
        /// <summary>
        /// 钉钉扫码成功跳转的地址
        /// </summary>
        public const string DingDingReturnUrl = "";
        /// <summary>
        /// 钉钉获取用户信息的地址
        /// </summary>
        public const string DingDingGetUserInfoUrl = "https://oapi.dingtalk.com/sns/getuserinfo_bycode?access_token={0}";
        /// <summary>
        /// 获取钉钉的token地址
        /// </summary>
        public const string DingDingAccessTokenUrl = "https://oapi.dingtalk.com/sns/gettoken?appid={0}&appsecret={1}";
        /// <summary>
        /// 钉钉的登录的二维码地址
        /// </summary>
        public const string DingDingLoginQrCodeUrl = "https://oapi.dingtalk.com/connect/qrconnect?appid={0}&response_type=code&scope=snsapi_login&state={1}&redirect_uri={2}";

        private MyJsonResult myJsonResult;

        private readonly IHttpRequest httpRequest;

        public DingDingConfig(MyJsonResult myJson, IHttpRequest _httpRequest)
        {
            myJsonResult = myJson;
            httpRequest = _httpRequest;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        public async Task<MyJsonResult> GetTokenAsync()
        {
            var res = await httpRequest.DoGetAsync(string.Format(DingDingAccessTokenUrl, DingDingAppkey, DingDingAppSecret));
            if (res != null && res["errcode"].ToString().Equals("0"))
            {
                var token = res["access_token"] != null ? res["access_token"].ToString() : "";
                myJsonResult.rows = token;
            }
            else
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.rows = res["errmsg"] != null ? res["errmsg"].ToString() : "";
            }
            return myJsonResult;
        }
        /// <summary>
        /// 获取钉钉的用户信息
        /// </summary>
        /// <param name="token">接口凭证</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<MyJsonResult> GetUserInfoAsync(string token, string code)
        {
            var res = await httpRequest.DoPostAsync(string.Format(DingDingGetUserInfoUrl, token), new StringContent("{ \"tmp_auth_code\": \"" + code + "\"}"), null, null, PostContentTypeEnum.JSON);
            if (res != null && res["errcode"].ToString().Equals("0"))
            {
                var userinfo = res["user_info"] != null ? res["user_info"].ToString() : "";
                myJsonResult.rows = userinfo;
            }
            else
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.rows = res["errmsg"] != null ? res["errmsg"].ToString() : "";
            }
            return myJsonResult;
        }
    }
}
