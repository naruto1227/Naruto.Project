using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.Infrastructure.Configuration.Management.Dashboard.Interface;
using Fate.Infrastructure.Configuration.Management.DB;
using Fate.Infrastructure.Configuration.Management.Object;
using Microsoft.AspNetCore.Mvc;



namespace Fate.Infrastructure.Configuration.Management.Dashboard.Controllers
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
        /// 新增配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(ConfigurationEndPoint info)
        {
            if (info == null)
                return BadRequest($"{nameof(info)}参数校检错误");
            if (string.IsNullOrWhiteSpace(info.Key))
                return BadRequest($"{nameof(info.Key)}参数校检错误");
            if (string.IsNullOrWhiteSpace(info.Key))
                return BadRequest($"{nameof(info.Value)}参数校检错误");
            await services.AddConfiguration(info);
            return Ok();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ConfigurationEndPoint info)
        {
            if (info == null)
                return BadRequest($"{nameof(info)}参数校检错误");
            if (id != info.Id)
                return BadRequest($"{nameof(info.Id)}参数不一致");
            if (string.IsNullOrWhiteSpace(info.Key))
                return BadRequest($"{nameof(info.Key)}参数校检错误");
            if (string.IsNullOrWhiteSpace(info.Key))
                return BadRequest($"{nameof(info.Value)}参数校检错误");
            await services.UpdateConfiguration(info);
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
        [HttpGet("querylist")]
        public async Task<IActionResult> Query(int page, int limit)
        {
            string group = Request.Query["group"].ToString();
            int.TryParse(Request.Query["environmentType"], out int environmentType);
            var info = new QueryConfigurationDTO()
            {
                Page = page,
                Group = group,
                EnvironmentType = environmentType,
                Limit = limit
            };
            var list = await services.QueryConfiguration(info);
            return Ok(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await services.DeleteConfiguration(new string[] { id });
            return Ok();
        }
    }
}
