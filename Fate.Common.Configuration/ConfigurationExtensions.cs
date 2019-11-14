using Fate.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 自定义配置扩展
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// 将自定义的配置加入
        /// </summary>
        /// <returns></returns>
        public static IConfigurationBuilder AddFateConfiguration(this IConfigurationBuilder @this)
        {
            return @this.Add(new FateConfigurationSource());
        }
    }
}
