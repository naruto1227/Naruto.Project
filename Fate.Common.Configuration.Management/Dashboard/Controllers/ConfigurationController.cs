using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.Common.Configuration.Management.Dashboard.Interface;
using Fate.Common.Configuration.Management.DB;
using Fate.Common.Configuration.Management.Object;
using Microsoft.AspNetCore.Mvc;



namespace Fate.Common.Configuration.Management.Dashboard.Controllers
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 配置操作的接口
    /// </summary>
    [Route("api/Management/[controller]/")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationServices services;
        public ConfigurationController(IConfigurationServices _services)
        {
            services = _services;
        }
        /// <summary>
        /// 新增修改配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUpd(ConfigurationEndPoint info)
        {
            if (info == null)
                return BadRequest($"{nameof(info)}参数校检错误");
            if (!string.IsNullOrWhiteSpace(info.Id))
            {
                await services.UpdateConfiguration(info);
            }
            else
            {
                await services.AddConfiguration(info);
            }
            return Ok();
        }
        /// <summary>
        /// 查询单条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> QueryFirst(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest($"{nameof(id)}参数校检失败");
            var info = await services.QueryFirstConfiguration(id);
            return Ok(info);
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryList")]
        public async Task<IActionResult> Query(QueryConfigurationDTO info)
        {
            if (info == null)
                return BadRequest($"{nameof(info)}参数校检错误");
            var list = await services.QueryConfiguration(info);
            return Ok(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok();
        }
    }
}
