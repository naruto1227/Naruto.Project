using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Ioc.Core;
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
            return await Task.Run(() => { return new JsonResult(value); });
        }
        [NonAction]
        /// <summary>
        /// 控制反转
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult AutofacResolve<TResult>() where TResult : class
        {
            return AutofacInit.Resolve<TResult>();
        }
    }
}