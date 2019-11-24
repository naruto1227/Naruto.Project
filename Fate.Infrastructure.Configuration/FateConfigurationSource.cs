using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 从远程配置源加载数据到配置接口中
    /// </summary>
    public class FateConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new FateConfigurationProvider();
        }
    }
}
