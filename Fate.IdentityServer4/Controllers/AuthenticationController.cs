using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Fate.IdentityServer4.Model;
using System.Net.Http;

namespace Fate.IdentityServer4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        MyJsonResult myJsonResult;
        public AuthenticationController(MyJsonResult _myJsonResult)
        {
            myJsonResult = _myJsonResult;
        }
        /// <summary>
        /// 获取授权的token
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ConnectionToken()
        {
            var client = new HttpClient();
            //从元数据中发现终结点,查找IdentityServer
            var url = (Request.IsHttps ? "https://" : "http://") + Request.Host.Host + ":" + Request.Host.Port;
            var disco = await client.GetDiscoveryDocumentAsync(url);
            if (disco.IsError)
            {
                myJsonResult.Code = "1001";
                myJsonResult.FailMsg = disco.Error;
                return await Json(myJsonResult);
            }
            //通过账号密码访问
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",
                UserName = "zhangsan",
                Password = "password",
                Scope = "api1"
            });
            if (tokenResponse.IsError)
            {
                myJsonResult.Code = "1002";
                myJsonResult.FailMsg = tokenResponse.Error;
                return await Json(myJsonResult);
            }
            myJsonResult.Code = "0";
            myJsonResult.Rows = tokenResponse.Json;
            return await Json(myJsonResult);
        }
        /// <summary>
        /// json格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Task<JsonResult> Json(object value)
        {
            return Task.Run(() =>
            {
                return new JsonResult(value);
            });
        }
    }
}