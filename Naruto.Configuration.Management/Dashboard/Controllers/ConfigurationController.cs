using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naruto.Configuration.Management.Dashboard.Interface;
using Naruto.Configuration.Management.DB;
using Naruto.Configuration.Management.Object;
using Microsoft.AspNetCore.Mvc;
namespace Naruto.Configuration.Management.Dashboard.Controllers
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
            var res = await services.AddConfiguration(info);
            DashboardResult dashboardResult = new DashboardResult();
            dashboardResult.code = 0;
            dashboardResult.data = res;
            return Ok(dashboardResult);
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
            var res = await services.UpdateConfiguration(info);
            DashboardResult dashboardResult = new DashboardResult();
            dashboardResult.code = 0;
            dashboardResult.data = res;
            return Ok(dashboardResult);
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
            DashboardResult dashboardResult = new DashboardResult();
            dashboardResult.code = 0;
            dashboardResult.data = info;
            return Ok(dashboardResult);
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery]QueryConfigurationDTO info)
        {
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
            var res = await services.DeleteConfiguration(id.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            DashboardResult dashboardResult = new DashboardResult();
            dashboardResult.code = 0;
            dashboardResult.data = res;
            return Ok(dashboardResult);
        }
    }
}
