using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Enum;
using Fate.Common.Extensions;
using Fate.Common.Infrastructure;
using Fate.Common.Interface;

namespace Fate.Common.Config
{
    /// <summary>
    /// 微博的基本配置
    /// </summary>
    public class WeiBoConfig : ICommonClassSigleDependency
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Appid = "";

        public const string AppSecret = "";

        /// <summary>
        /// 授权地址 (获取code)
        /// </summary>
        public const string AuthorizeUrl = "https://api.weibo.com/oauth2/authorize?client_id={0}&redirect_uri={1}&state={2}";


        /// <summary>
        /// 获取token地址
        /// </summary>
        public const string AccessTokenUrl = "https://api.weibo.com/oauth2/access_token";

        /// <summary>
        /// 登录授权跳转地址
        /// </summary>
        public const string ReturnUrl = "";

        /// <summary>
        /// 授权绑定的跳转地址
        /// </summary>
        public const string BindReturnUrl = "";

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public const string GetUserInfoUrl = "https://api.weibo.com/2/users/show.json?access_token={0}&uid={1}";

        private MyJsonResult myJsonResult;

        /// <summary>
        /// http请求
        /// </summary>
        private readonly IHttpRequest httpRequest;

        public WeiBoConfig(MyJsonResult myJson, IHttpRequest _httpRequest)
        {
            myJsonResult = myJson;
            httpRequest = _httpRequest;
        }

        /// <summary>
        /// 获取授权token等信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<MyJsonResult> AccessTokenAsync(string code)
        {
            if (code.IsNullOrEmpty())
                throw new ArgumentException("code无效");
            //拼接参数
            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "client_id",Appid},
                 { "client_secret",AppSecret},
                  { "grant_type","authorization_code"},
                   { "code",code},
                    { "redirect_uri",ReturnUrl}
            });
            //调用接口获取token
            var res = (await httpRequest.DoPostAsync(AccessTokenUrl, content, null));
            if (res == null)
                throw new ApplicationException("接口调用失败");
            if (!res.ContainsKey("access_token") || res["access_token"].ToString().IsNullOrEmpty())
                throw new ApplicationException(res.ContainsKey("error_description") ? res["error_description"].ToString() : "token获取失败");
            myJsonResult.rows = res.ToJson();
            return myJsonResult;
        }

        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <param name="tokenResult">获取到的token的信息</param>
        /// <returns></returns>
        public async Task<MyJsonResult> GetUserInfoAsync(string tokenResult)
        {
            var tokenInfo = tokenResult.ToDic();
            //拼接获取用户信息接口地址
            var userUrl = string.Format(GetUserInfoUrl, tokenInfo["access_token"].ToString(), tokenInfo["uid"].ToString());
            //调用
            var res = await httpRequest.DoGetAsync(userUrl);
            if (res == null)
                throw new ApplicationException("用户信息获取失败");
            if (res.ContainsKey("error") && res["error"] != null)
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.failMsg = "error_code=" + res["error_code"].ToString() + ",error=" + res["error"].ToString();
            }
            else
                myJsonResult.rows = res.ToJson();
            return myJsonResult;
        }
    }
}
