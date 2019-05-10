using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}