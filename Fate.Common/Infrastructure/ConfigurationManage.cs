using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Infrastructure
{
    public class ConfigurationManage
    {

        private static IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        /// <summary>
         /// 获取配置信息
        /// 
         /// </summary>
        /// <param name="isAppsetting">是否来自于AppSetting 节点 ，是的话值需填写AppSetting下的路径,否则的话写出全路径</param>
        public static string GetValue(string key, bool isAppsetting = true)
        {
            if (isAppsetting)
                key = "AppSetting:" + key;
            var value = configuration.GetValue<string>(key);
            if (string.IsNullOrWhiteSpace(value))
                return "";
            else
                return value;
        }
    }
}
