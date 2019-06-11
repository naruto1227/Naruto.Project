using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Fate.IdentityServer4.Model;
using System.Net.Http;
using Fate.Common.Extensions;
using Fate.Common.Enum;
using Fate.Common.Infrastructure;

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
            //用户名
            var userName = Request.Form["userName"];
            //密钥
            var password = Request.Form["password"];
            //权限范围
            var scope = Request.Form["scope"];
            #endregion

            #region 数据校验
            if (string.IsNullOrWhiteSpace(clientId))
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "客户id不能为空";
                return Json(myJsonResult);
            }
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "客户密钥不能为空";
                return Json(myJsonResult);
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "用户名不能为空";
                return Json(myJsonResult);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "密码不能为空";
                return Json(myJsonResult);
            }
            if (string.IsNullOrWhiteSpace(scope))
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.DATACODE;
                myJsonResult.msg = "授权范围不能为空";
                return Json(myJsonResult);
            }
            #endregion
            var client = new HttpClient();
            //从元数据中发现终结点,查找IdentityServer
            var url = (Request.IsHttps ? "https://" : "http://") + Request.Host.Host + ":" + Request.Host.Port;
            var disco = await client.GetDiscoveryDocumentAsync(url);
            if (disco.IsError)
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.SERVERCODE;
                myJsonResult.msg = disco.Error;
                return Json(myJsonResult);
            }
            //通过账号密码访问
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,//"ro.client",
                ClientSecret = clientSecret, //"secret",
                UserName = userName,// "zhangsan",
                Password = password,//"password",
                Scope = scope//"api1"
            });
            if (tokenResponse.IsError)
            {
                myJsonResult.code = (int)MyJsonResultCodeEnum.SERVERCODE;
                myJsonResult.msg = tokenResponse.Error;
                return Json(myJsonResult);
            }
            myJsonResult.code = (int)MyJsonResultCodeEnum.SUCCESSCODE;
            myJsonResult.rows = tokenResponse.Json;
            return Json(myJsonResult);
        }
        /// <summary>
        /// json格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private JsonResult Json(object value)
        {
            return new JsonResult(value);
        }
    }
}