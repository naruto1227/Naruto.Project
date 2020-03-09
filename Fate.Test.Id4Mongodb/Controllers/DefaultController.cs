using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fate.Test.Id4Mongodb.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public string Get()
        {
            return "hello api";
        }
        [HttpPost]
        /// <summary>
        /// 获取授权的token
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ConnectionToken()
        {
            #region 接收参数
            //客户id
            var clientId = Request.Form["clientId"];
            //客户密钥
            var clientSecret = Request.Form["clientSecret"];
            //权限范围
            var scope = Request.Form["scope"];
            #endregion
            var client = new HttpClient();
            ////从元数据中发现终结点,查找IdentityServer
            //var disco = await client.GetDiscoveryDocumentAsync(ConfigurationManage.GetAppSetting("AuthorityUrl"));
            //if (disco.IsError)
            //{
            //    myJsonResult.code = (int)MyJsonResultEnum.serviceError;
            //    myJsonResult.msg = disco.Error;
            //    return Json(myJsonResult);
            //}
            //通过客户端访问
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = "http://localhost:5000" + "/connect/token",//disco.TokenEndpoint,
                ClientId = clientId,//"ro.client",
                ClientSecret = clientSecret, //"secret",
                Scope = scope//"api1",
            });
            return new JsonResult(tokenResponse.Json);
        }
    }
}