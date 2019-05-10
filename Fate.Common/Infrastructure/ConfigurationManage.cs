using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Infrastructure
{
   public class ConfigurationManage
    {
        private static IConfigurationSection _appSection = null;

        /// <summary>
         /// 获取配置信息
         /// </summary>
        public static string GetAppSetting(string key)
        {
            string str = string.Empty;
            if (_appSection.GetSection(key) != null)
            {
                str = _appSection.GetSection(key).Value;
            }
            return str;
        }

        /// <summary>
        /// 设置访问配置信息 _appSection的初始值 
        /// </summary>
        /// <param name="section"></param>
        public static void SetAppSetting(IConfigurationSection section)
        {
            _appSection = section;
        }
    }
}
