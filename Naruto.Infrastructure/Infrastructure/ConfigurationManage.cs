using Naruto.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Naruto.Infrastructure
{
    public static class ConfigurationManage
    {

        private static IConfigurationRoot configuration;

        static ConfigurationManage()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);
            //获取当目录下的所有的配置文件的路径
            var files = new System.IO.DirectoryInfo(AppContext.BaseDirectory).GetFiles("appsettings*.json");
            if (files == null && files.Count() <= 0)
                throw new NoFoundException("appsettings.json找不到");

            //需要在appsettings.json中添加ASPNETCORE_ENVIRONMENT环境变量的key 
            //获取当前系统的环境变量从appsetting中
            var environmentName = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build().GetValue<string>(EnvironmentName);
            //遍历名称
            foreach (var item in files)
            {
                if (item.Name.Equals("appsettings.json") || item.Name.Equals($"appsettings.{environmentName}.json"))
                {
                    configurationBuilder.AddJsonFile(item.Name, true, true);
                }
            }
            configuration = configurationBuilder.Build();
        }
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
        /// <summary>
         /// 获取配置中的环境变量
         /// </summary>
        public static string GetEnvironmentName()
        {
            return configuration.GetValue<string>(EnvironmentName);
        }

        /// <summary>
         /// 获取配置信息
         /// </summary>
        public static T GetSection<T>(string key) =>
             configuration.GetSection(key).Get<T>();

        /// <summary>
         /// 获取配置信息
         /// </summary>
        public static IConfigurationSection GetSection(string key) =>
             configuration.GetSection(key);
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key) => configuration.GetConnectionString(key);
        /// <summary>
        /// 存储在appsettings.js中的环境变量的key
        /// </summary>
        public static readonly string EnvironmentName = "ASPNETCORE_ENVIRONMENT";
    }
}
