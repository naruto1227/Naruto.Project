using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Ioc.Core;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
namespace Fate.WebApi.Controllers
{
    /// <summary>
    /// 公共基础类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]

        public async Task<JsonResult> Json(object value)
        {
            //JsonSerializerSettings中设置 ContractResolver=new CamelCasePropertyNamesContractResolver() 小驼峰
            return await Task.Run(() => { return new JsonResult(value,new JsonSerializerSettings() {  DateFormatString= "yyyy-MM-dd HH:mm:ss" }); });
        }
        [NonAction]
        /// <summary>
        /// 控制反转
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult AutofacResolve<TResult>() where TResult : class
        {
            return AutofacDI.Resolve<TResult>();
        }
    }
}