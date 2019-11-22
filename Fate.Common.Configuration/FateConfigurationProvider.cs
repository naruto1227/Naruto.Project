using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-14
    /// 提供远程配置的提供者
    /// </summary>
    public class FateConfigurationProvider : ConfigurationProvider
    {

        private readonly FateConfigurationLoadAbstract fateConfiguration = new DefaultFateConfigurationLoad();

        /// <summary>
        /// 加载配置
        /// </summary>
        public override void Load()
        {
            //存储数据
            Data = fateConfiguration.LoadConfiguration().ConfigureAwait(false).GetAwaiter().GetResult();
            //存储配置的key
            FateConfigurationInfrastructure.Keys = new ConcurrentBag<string>(Data.Keys);
            //FateConfigurationInfrastructure.changeToken = GetReloadToken();
            //FateConfigurationInfrastructure.changeToken.RegisterChangeCallback((obj) =>
            //{
            //    Console.WriteLine("1");
            //}, null);
        }
    }
}
