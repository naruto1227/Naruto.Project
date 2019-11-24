using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Fate.Infrastructure.Extensions;
using Fate.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Infrastructure.Enum;
using Fate.Infrastructure.Interface;

namespace Fate.Infrastructure.Config
{
    /// <summary>
    /// 支付宝配置信息
    /// </summary>
    public class ZhiFuBaoConfig: ICommonClassSigleDependency
    {
        /// <summary>
        /// 商户公钥(存储)
        /// </summary>
        public const string MerchantPubKey = "";

        /// <summary>
        /// 商户私钥
        /// </summary>
        public const string PriKey = "";
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public const string PubKey = "";
        /// <summary>
        /// 
        /// </summary>
        public const string Appid = "";
        /// <summary>
        /// 支付宝网关
        /// </summary>
        public const string ServerUrl = "https://openapi.alipay.com/gateway.do";
        /// <summary>
        /// 登录授权跳转地址
        /// </summary>
        public const string ReturnUrl = "";

        /// <summary>
        /// 授权绑定的跳转地址
        /// </summary>
        public const string BindReturnUrl = "";
        /// <summary>
        /// 授权地址
        /// </summary>
        public static string LoginUrl = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id={0}&scope=auth_user&redirect_uri={1}&state=" + DateTime.Now.Ticks;

        //public static string AccessTokenUrl = "https://openapi.alipay.com/gateway.do?{0}&sign={1}";

        #region
        ///// <summary>
        ///// 将需要签名的字典 排序
        ///// </summary>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //public static string SignContent(IDictionary<string, string> parameters)
        //{
        //    // 第一步：把字典按Key的字母顺序排序
        //    IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
        //    IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
        //    // 第二步：把所有参数名和参数值串在一起
        //    StringBuilder query = new StringBuilder("");
        //    while (dem.MoveNext())
        //    {
        //        string key = dem.Current.Key;
        //        string value = dem.Current.Value;
        //        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        //        {
        //            query.Append(key).Append("=").Append(value).Append("&");
        //        }
        //    }
        //    string content = query.ToString().Substring(0, query.Length - 1);
        //    return content;
        //}

        //public static string SignData(string content)
        //{
        //    return AlipaySignature.RSASign(content, PriKey, null, "RSA2", false);
        //}
        #endregion

        private MyJsonResult myJsonResult;
        public ZhiFuBaoConfig(MyJsonResult myJson)
        {
            myJsonResult = myJson;
        }
        /// <summary>
        /// 获取授权token等信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<MyJsonResult> AccessTokenAsync(string code)
        {
            //定义一个响应的信息
            var res = "";
            IAopClient client = new DefaultAopClient(ServerUrl, Appid, PriKey, "json", "1.0", "RSA2", PubKey, null, false);
            AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest();
            request.GrantType = "authorization_code";
            request.Code = code;
            //request.RefreshToken = "201208134b203fe6c11548bcabd8da5bb087a83b";
            AlipaySystemOauthTokenResponse response = await client.ExecuteAsync(request);
            if (response.AccessToken.IsNullOrEmpty())
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.failMsg = response.SubMsg;
                return myJsonResult;
            }
            res = response.ToJson();
            myJsonResult.rows = res;
            return myJsonResult;
        }

        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<MyJsonResult> GetUserInfoAsync(string token)
        {
            //构建公共参数
            IAopClient client = new DefaultAopClient(ServerUrl, Appid, PriKey, "json", "1.0", "RSA2", PubKey, null, false);
            AlipayUserInfoShareRequest request = new AlipayUserInfoShareRequest();
            //执行
            AlipayUserInfoShareResponse response = await client.ExecuteAsync(request, token);
            //构建返回的结果
            if (response.Code != "10000")
            {
                myJsonResult.code = (int)MyJsonResultEnum.thirdError;
                myJsonResult.failMsg = response.SubMsg;
            }
            myJsonResult.rows = response;
            return myJsonResult;
        }
    }
}
