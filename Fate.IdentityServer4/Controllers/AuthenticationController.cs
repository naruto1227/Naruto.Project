using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Fate.IdentityServer4.Model;

namespace Fate.IdentityServer4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        MyJsonResult myJsonResult;
        public AuthenticationController(MyJsonResult _myJsonResult) {
            myJsonResult = _myJsonResult;
        }
        /// <summary>
        /// 获取授权的token
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ConnectionToken()
        {
            //从元数据中发现终结点,查找IdentityServer http://localhost:10284 为当前地址
            var disco = await DiscoveryClient.GetAsync("http://localhost:54717");
            if (disco.IsError)
            {
                myJsonResult.Code = "1001";
                myJsonResult.FailMsg = disco.Error;
                return await Json(myJsonResult);
            }
            //向IdentityServer请求令牌 (账号密码模式)
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            //通过账号密码访问
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("zhangsan", "password", "api1");
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