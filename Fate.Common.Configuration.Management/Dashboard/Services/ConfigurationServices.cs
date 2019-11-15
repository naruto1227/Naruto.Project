using Fate.Common.Configuration.Management.Dashboard.Interface;
using Fate.Common.Configuration.Management.DB;
using Fate.Common.Configuration.Management.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management.Dashboard.Services
{
    /// <summary>
    /// z张海波
    /// 2019-11-14
    /// 配置操作的服务提供
    /// </summary>
    public class ConfigurationServices : IConfigurationServices
    {
        public Task<bool> AddConfiguration(ConfigurationEndPoint info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteConfiguration(ConfigurationEndPoint info)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, string>> QueryConfiguration(QueryConfigurationDTO info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateConfiguration(ConfigurationEndPoint info)
        {
            throw new NotImplementedException();
        }
    }
}
