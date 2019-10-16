using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace Fate.Common.Configuration.Management.Controllers
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 对外开放获取 配置信息的接口
    /// </summary>
    [Route("api/{controller}")]
    [ApiController]
    public class ForeignController : ControllerBase
    {

        public JsonResult Get()
        {
            return new JsonResult("1");
        }
    }
}
