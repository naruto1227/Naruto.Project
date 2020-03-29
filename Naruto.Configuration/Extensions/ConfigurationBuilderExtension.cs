using Naruto.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtension
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
