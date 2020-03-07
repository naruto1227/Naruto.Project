using Fate.Infrastructure.Enum;
using Fate.Infrastructure.Extensions;
using Fate.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace Fate.Infrastructure
{
    public class DefaultHttpWebRequest : IHttpRequest
    {

        /// <summary>
        /// http请求 工厂
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger<DefaultHttpWebRequest> logger;

        public DefaultHttpWebRequest(IHttpClientFactory _httpClientFactory, ILogger<DefaultHttpWebRequest> _logger)
        {
            httpClientFactory = _httpClientFactory;
            logger = _logger;
        }

        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> DoGetAsync(string url, AuthenticationHeaderValue authToken = null, Dictionary<string, string> heart = null)
        {
            var res = await DoGetStringAsync(url, authToken, heart);
            return res.ToDic();
        }


        public async Task<string> DoGetStringAsync(string url, AuthenticationHeaderValue authToken = null, Dictionary<string, string> heart = null)
        {
            var request = httpClientFactory.CreateClient();
            if (authToken != null)
            {
                request.DefaultRequestHeaders.Authorization = authToken;
            }
            if (heart != null)
            {
                foreach (var item in heart)
                {
                    request.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }
            }
            using (var respon = await request.GetAsync(url))
            {
                var res = (await respon.Content.ReadAsStringAsync());//返回结果
                if (res.IsNullOrEmpty())
                    logger.LogInformation($"{url},ResponMessage:{respon.ToJson()}");
                return res;
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
        public async Task<Dictionary<string, object>> DoPostAsync(string url, HttpContent content, AuthenticationHeaderValue authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED)
        {
            var res = await DoPostStringAsync(url, content, authToken, heart, contentTypeEnum);
            return res.ToDic();
        }

        public async Task<string> DoPostStringAsync(string url, HttpContent content, AuthenticationHeaderValue authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED)
        {
            var request = httpClientFactory.CreateClient();

            if (authToken != null)
            {
                request.DefaultRequestHeaders.Authorization = authToken;
            }
            if (heart != null)
            {
                foreach (var item in heart)
                    content.Headers.Add(item.Key, item.Value);
            }
            if (contentTypeEnum == PostContentTypeEnum.URLENCODED)
                request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            else if (contentTypeEnum == PostContentTypeEnum.JSON)
                request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (var respon = await request.PostAsync(url, content))
            {
                var res = (await respon.Content.ReadAsStringAsync());//返回结果
                if (res.IsNullOrEmpty())
                    logger.LogInformation($"{url},ResponMessage:{respon.ToJson()}");
                return res;
            }
        }
    }
}
