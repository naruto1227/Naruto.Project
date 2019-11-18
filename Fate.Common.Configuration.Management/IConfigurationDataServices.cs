using Fate.Common.Configuration.Management.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management
{
    /// <summary>
    /// 获取配置数据的接口服务
    /// </summary>
    public interface IConfigurationDataServices 
    {
        /// <summary>
        /// 获取配置数据 返回字典
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task QueryDataAsync(RequestContext requestContext);
    }
}
