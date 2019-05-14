using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fate.Common.Enum;
using System.Security.Cryptography;
using System.Text;

namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <returns></returns>
        public static async Task<string> DoGetAsync(string url, string authToken = null, Dictionary<string, string> heart = null)
        {
            using (var request = new HttpClient())
            {
                request.BaseAddress = new Uri(url);
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authToken);
                }
                if (heart != null)
                {
                    foreach (var item in heart)
                    {
                        request.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }
                using (var res = await request.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadAsStringAsync();//返回结果
                    }
                    return "fail";
                }
            }
        }

        /// <summary>
        /// post 请求 (key value 使用 FormUrlEncodedContent) json 格式使用 StringContent
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <param name="contentTypeEnum">传输类型</param>
        /// <returns></returns>
        public static async Task<string> DoPostAsync(string url, HttpContent content, string authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED)
        {
            using (var request = new HttpClient())
            {
                request.BaseAddress = new Uri(url);
                if (!string.IsNullOrWhiteSpace(authToken))
                {
                    request.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authToken);
                }
                if (heart != null)
                {
                    foreach (var item in heart)
                    {
                        request.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }
                if (contentTypeEnum == PostContentTypeEnum.URLENCODED)
                {
                    request.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                }
                else if (contentTypeEnum == PostContentTypeEnum.JSON)
                {
                    request.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                }
                using (var res = await request.PostAsync(url, content))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadAsStringAsync();//返回结果
                    }
                    return "fail";
                }
            }
        }


        /// 32位加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get32MD5(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2").PadLeft(2, '0'));
                }
                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }
    }
}
