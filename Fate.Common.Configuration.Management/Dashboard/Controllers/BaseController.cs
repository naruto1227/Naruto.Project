using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard.Controllers
{

    /// <summary>
    /// 所有的控制器的基类
    /// </summary>
    [Route("Management/[controller]/[action]")]
    public class BaseController : Controller
    {
        public IActionResult Test() {
            return Json("1");
        }
    }
}
