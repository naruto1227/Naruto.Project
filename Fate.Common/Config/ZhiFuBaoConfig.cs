using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Fate.Common.Extensions;
using Fate.Common.Infrastructure;
using Fate.Common.Redis.IRedisManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Enum;
namespace Fate.Common.Config
{
    /// <summary>
    /// 支付宝配置信息
    /// </summary>
    public class ZhiFuBaoConfig
    {
        /// <summary>
        /// 商户公钥(存储)
        /// </summary>
        public const string MerchantPubKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAu60mXk+ol71MCABu6Gx5 BbPWzRzeCadpSkKOU4xXa4dUqIF+86VrxM7I1Eitlkdw+dmxSzwbGZJNiQS6gZqW Ahk2nkghdvvCKnGQprPdN1Ankl2YkejbRV4Fqq0kgMgJIHxfRDSqEa17j3AhPuZt rsad/SU34BVcVnsnEIwG6+f5vPJeV5ZirzejnM3p48uCJSds8b5XTRSFkI5BJzgZ 9Rypacx7zFVY4oX23ipzD7xE9u9KD2Drc67a03j96JIIAfSpgCd8Sc3hbca4SsBM Ys+FyPpmDdu0bTYXUPn3kRH+3jM69S4Au39/ZmRcWBIv6jmN/7hVi7o8R+5aSBRd gwIDAQAB";

        /// <summary>
        /// 商户私钥
        /// </summary>
        public const string PriKey = "MIIEowIBAAKCAQEAu60mXk+ol71MCABu6Gx5BbPWzRzeCadpSkKOU4xXa4dUqIF+86VrxM7I1Eitlkdw+dmxSzwbGZJNiQS6gZqWAhk2nkghdvvCKnGQprPdN1Ankl2YkejbRV4Fqq0kgMgJIHxfRDSqEa17j3AhPuZtrsad/SU34BVcVnsnEIwG6+f5vPJeV5ZirzejnM3p48uCJSds8b5XTRSFkI5BJzgZ9Rypacx7zFVY4oX23ipzD7xE9u9KD2Drc67a03j96JIIAfSpgCd8Sc3hbca4SsBMYs+FyPpmDdu0bTYXUPn3kRH+3jM69S4Au39/ZmRcWBIv6jmN/7hVi7o8R+5aSBRdgwIDAQABAoIBAFIc5gapj7gkSJnPprbmjuTh1H+Vu8g5iSXGjQMdCjqv0WiQj/0GSWqoltHaoqh3xYRrrNigCbNcgbQLb5a5Dh0I7w69vHaUnFV5rrJhS86hsU6myNQ+L4HgK1aLvsbhvqyJ/hyXdjmZz8/oXYNpyl+H4yQZHqNadTYZV/Qzb+vSqRQeSAFXW8lTKUrW39q3iwtEnE/9kZpjeHC3phKZ2GWpQhdTbgJaYGobbYQGCcLRD8SZMaffylcvB2JjjalXr9hA2KdQyP11gOcj8pmo3w0JKSoQtD52M0aHF7pv6kSA6X7qRvMz+wD/Cj044mWFyy73W3ukRQhLy7QJVLBiEIECgYEA6SJxoj8YyRrgOGY+oT8gn6J7cXWXMTRCuPJPl/R4dxQt+o75WPU4VOQ9jNCeWML2u60doZasw8EHHWAoSeiSDWizudP9EH11MVEJWtuY5YuDK9LXKtseMGt/9YMHuNHlBSeiGO9hnWQeiM/BjKgBiRPJEg2F8DfZWLYXh1G3YyMCgYEAzhVW3I+hf4D1mUNzPmpbBHujwSmI5UfJ7296uJsLrPQqjloR9YYiJrf8UNhk7ccKnZNgbnvegeEaoz5BywIc2/DLgft4AJJGT5z9s7w5hOSwMyZQYtPweo3uNxM2ZQ/R+1buyc0iIVZTKt4+9unc5iXbi0sRX84c0Dcjy1u5ciECgYAZXuI24d70o7Qa7yWJrrECLlB4vG9Dr4hDUDtRMg2aB9wpFD4WbDlBI8V++YUB69Wl/uTmnNsYQn1fuQMpZ+HdC7PjWSqFDOIgB82Y0aUF9fTEZCF+THIwmsJGGYhRmKXvtaxyQjrBQhAAm3tYrz/bhweuq0IULj+847QTydTHOQKBgCoAZWSsjWC8OtIS0jEdYhGG1Xgv6+u29uwqz1tCll2YoffDWbetr9YuV2luwRtJHtvAxr2d11qnM1OkA6rJcnJNIF0MkIelSFk/iOGR8jMonNS/8VBDG9cOiEeHTeXJFKXiMObGPTeILYkmJLUUMuZhPt6j3RWmn268Xjo7Jo+hAoGBAOM0GiFdgyUPSF4heeIYydzPnPS6yt+eJCQHoxaNal5ApQFKmWUViraH1XyaLISvHF32kyTcOgIWJtSkxIfCwGBln0y/fAd+2E2YVCQIf3XXtQubRmXfK/TYX78ZMxPwmeLz580sx59LGTjyP6bFP+mO3AdDLTn4ZeqOqrBEFEYR";
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public const string PubKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuP9lCY52sCtXfeSpfoHIn7mheHSFRnZnH4NJmn45S6g1SRQzycpS+BCyZ7NYbIQwm0NRMCjIEJOYvZ+2XHVM2bf79I6B1PWl/2vSYPYRxBTySJ4IAVCmgtIGyJUCZAIr707MMmP9uzakfXFHe6PwLzwJ/w2Pa7i+qvGnCEqcbtw6S8icKUY9jSK+KcYLtVWhJStju6fD2qgcApg7WkBmGRTPQxfN4LWYxOSVCaadf4YHsI9bRL83/fKy8xrmtUFkBRlDjRPBeFsGR15hO3jDkBrxgbDWCR6IlwWZEJo0Sa4JSkJ8HdLY/ZQOzsYjwpOMWSEhhFJmsL+zu8MqIDarBwIDAQAB";
        /// <summary>
        /// 
        /// </summary>
        public const string Appid = "2019061365534186";
        /// <summary>
        /// 支付宝网关
        /// </summary>
        public const string ServerUrl = "https://openapi.alipay.com/gateway.do";
        /// <summary>
        /// 登录授权跳转地址
        /// </summary>
        public const string ReturnUrl = "http://cp.haibozs.xyz/account/zhifubaologin";

        /// <summary>
        /// 授权绑定的跳转地址
        /// </summary>
        public const string BindReturnUrl = "http://cp.haibozs.xyz/user/zhifubaobind";
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
                myJsonResult.code = (int)MyJsonResultCodeEnum.thirdError;
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
                myJsonResult.code = (int)MyJsonResultCodeEnum.thirdError;
                myJsonResult.failMsg = response.SubMsg;
            }
            myJsonResult.rows = response;
            return myJsonResult;
        }
    }
}
