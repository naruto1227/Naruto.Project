using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard.Controllers
{

    /// <summary>
    /// 所有的控制器的基类
    /// </summary>
    [Route("api/Management/[controller]/")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
