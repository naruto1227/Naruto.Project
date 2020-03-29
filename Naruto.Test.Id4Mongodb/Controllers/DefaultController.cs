using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Naruto.Test.Id4Mongodb.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        [HttpGet]
        /// <summary>
        /// 获取授权的token
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ConnectionToken()
        {
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
                Address = $"http://localhost:5000" + "/connect/token",//disco.TokenEndpoint,
                ClientId = "test",
                ClientSecret = "123456",
                Scope = "api",
            });
           
            return Ok(tokenResponse.Json);
        }
    }
}