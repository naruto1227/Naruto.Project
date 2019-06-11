using Fate.Common.Enum;
using Fate.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Infrastructure
{
    public class HttpWebRequest
    {
        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <returns></returns>
        public static async Task<Dictionary<string, object>> DoGetAsync(string url, string authToken = null, Dictionary<string, string> heart = null)
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
                    return (await res.Content.ReadAsStringAsync()).ToDic();//返回结果
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
        public static async Task<Dictionary<string, object>> DoPostAsync(string url, HttpContent content, string authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED)
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
                    request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                }
                else if (contentTypeEnum == PostContentTypeEnum.JSON)
                {
                    request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                using (var res = await request.PostAsync(url, content))
                {
                    return (await res.Content.ReadAsStringAsync()).ToDic();//返回结果
                }
            }
        }

    }
}
