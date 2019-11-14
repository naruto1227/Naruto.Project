using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace Fate.Common.Configuration.Management.Dashboard.Controllers
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 配置操作的接口
    /// </summary>
    public class ConfigurationController : BaseController
    {
        /// <summary>
        /// 新增修改配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUpdConfigurationEndPoint()
        {
            return Ok();
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> QueryConfigurationEndPoint()
        {
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConfigurationEndPoint()
        {
            return Ok();
        }
    }
}
