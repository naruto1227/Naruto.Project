using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace CP.Common.Infrastructure
{
    /// <summary>
    /// 配置钉钉登录需要的数据
    /// </summary>
    public class DingDingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DingDingAppkey = "dingoaemubpncq8guikorj";
        /// <summary>
        /// 
        /// </summary>
        public const string DingDingAppSecret = "_DK0ziUbcspLVfRyoE5F1vjLzHoxPqM92wJV2FlwkdVvIZSxlOvB9Cr36RDL8SCf";
        /// <summary>
        /// 钉钉跳转的地址
        /// </summary>
        public const string DingDingReturnUrl = "http://cp.haibozs.xyz/account/dingdinglogin/";
        /// <summary>
        /// 钉钉获取用户信息的地址
        /// </summary>
        public const string DingDingGetUserInfoUrl = "https://oapi.dingtalk.com/sns/getuserinfo_bycode?access_token={0}";
        /// <summary>
        /// 获取钉钉的token地址
        /// </summary>
        public static string DingDingAccessTokenUrl = "https://oapi.dingtalk.com/sns/gettoken?appid={0}&appsecret={1}";
        /// <summary>
        /// 钉钉的登录的二维码地址
        /// </summary>
        public static string DingDingLoginQrCodeUrl = "https://oapi.dingtalk.com/connect/qrconnect?appid={0}&response_type=code&scope=snsapi_login&state={1}&redirect_uri={2}";
        /// <summary>
        /// 获取token
        /// </summary>
        public static async Task<MyJsonResult> GetTokenAsync()
        {
            var res = await HttpRequest.DoGetAsync(string.Format(DingDingAccessTokenUrl, DingDingAppkey, DingDingAppSecret));
            MyJsonResult myJsonResult = new MyJsonResult();
            if (res != null && res["errcode"].ToString().Equals("0"))
            {
                var token = res["access_token"] != null ? res["access_token"].ToString() : "";
                myJsonResult.data = token;
            }
            else
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.data = res["errmsg"] != null ? res["errmsg"].ToString() : "";
            }
            return myJsonResult;
        }
        /// <summary>
        /// 获取钉钉的用户信息
        /// </summary>
        /// <param name="token">接口凭证</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<MyJsonResult> GetUserInfoAsync(string token, string code)
        {
            var res = await HttpRequest.DoPostAsync(string.Format(DingDingGetUserInfoUrl, token), new StringContent("{ \"tmp_auth_code\": \"" + code + "\"}"), null, PostContentTypeEnum.JSON);
            MyJsonResult myJsonResult = new MyJsonResult();
            if (res != null && res["errcode"].ToString().Equals("0"))
            {
                var userinfo = res["user_info"] != null ? res["user_info"].ToString().ToJson<DB.MemSysDB.DingDingUserInfo>() : default;
                myJsonResult.data = userinfo;
            }
            else
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.data = res["errmsg"] != null ? res["errmsg"].ToString() : "";
            }
            return myJsonResult;
        }
    }
}
