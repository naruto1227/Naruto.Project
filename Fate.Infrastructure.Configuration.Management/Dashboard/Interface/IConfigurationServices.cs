using Fate.Infrastructure.Configuration.Management.DB;
using Fate.Infrastructure.Configuration.Management.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration.Management.Dashboard.Interface
{
    /// <summary>
    /// z张海波
    /// 2019-11-14
    /// 配置操作的接口服务
    /// </summary>
    public interface IConfigurationServices
    {
        /// <summary>
        /// 添加服务配置
        /// </summary>
        /// <returns></returns>
        Task<bool> AddConfiguration(ConfigurationEndPoint info);

        /// <summary>
        /// 修改服务配置
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateConfiguration(ConfigurationEndPoint info);

        /// <summary>
        /// 删除服务配置
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteConfiguration(string[] ids);
        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <returns></returns>
        Task<DashboardResult> QueryConfiguration(QueryConfigurationDTO info);

        /// <summary>
        /// 查询单挑配置信息
        /// </summary>
        /// <returns></returns>
        Task<ConfigurationEndPoint> QueryFirstConfiguration(string id);
    }
}
