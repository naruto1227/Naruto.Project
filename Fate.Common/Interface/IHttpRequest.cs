using Fate.Common.Enum;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Interface
{
    /// <summary>
    /// http请求服务
    /// </summary>
    public interface IHttpRequest:ICommonSingleDependency
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Dictionary<string, object>> DoGetAsync(string url);

        /// <summary>
        /// get请求 获取字符串
        /// </summary>
        /// <param name="url"></param>
        Task<string> DoGetStringAsync(string url);

        /// <summary>
        /// get 请求 带token的
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> DoGetAsync(string url, string authToken = null, Dictionary<string, string> heart = null);


        /// <summary>
        /// get 请求 带token的 返回字符串
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <returns></returns>
        Task<string> DoGetStringAsync(string url, string authToken = null, Dictionary<string, string> heart = null);



        /// <summary>
        /// post 请求 (key value 使用 FormUrlEncodedContent) json 格式使用 StringContent
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <param name="contentTypeEnum">传输类型</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> DoPostAsync(string url, HttpContent content, string authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED);

        /// <summary>
        /// post 请求 (key value 使用 FormUrlEncodedContent) json 格式使用 StringContent
        /// </summary>
        /// <param name="url">访问地址</param>
        /// <param name="authToken">oauth token</param>
        /// <param name="heart">访问头</param>
        /// <param name="contentTypeEnum">传输类型</param>
        /// <returns></returns>
        Task<string> DoPostStringAsync(string url, HttpContent content, string authToken = null, Dictionary<string, string> heart = null, PostContentTypeEnum contentTypeEnum = PostContentTypeEnum.URLENCODED);

    }
}
