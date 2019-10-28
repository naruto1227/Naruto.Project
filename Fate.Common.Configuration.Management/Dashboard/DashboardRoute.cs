using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 面板的路由 配置
    /// </summary>
    internal static class DashboardRoute
    {
        /// <summary>
        /// 静态资源的请求前缀
        /// </summary>
        internal static string ResourceRequestPrefix = "/fate";
        /// <summary>
        /// 存放用到的js文件的名字
        /// </summary>
        private static readonly string[] Javascripts =
       {
            "MD5.js",

        };

        /// <summary>
        /// 存放需要用的样式的名字
        /// </summary>
        private static readonly string[] Stylesheets =
        {

        };

        /// <summary>
        /// 静态初始化
        /// </summary>
        static DashboardRoute()
        {
            Routes = new RouteCollections();
            foreach (var item in Javascripts)
            {
                Routes.Add($"{ResourceRequestPrefix}/js/{item}", "js", "application/x-javascript");
            }
            foreach (var item in Stylesheets)
            {
                Routes.Add($"{ResourceRequestPrefix}/css/{item}", "css", "application/x-javascript");
            }
        }
        /// <summary>
        /// 路由集合
        /// </summary>
        public static RouteCollections Routes { get; }

        /// <summary>
        /// 获取请求的文件名
        /// </summary>
        /// <param name="requestPath">请求的路径</param>
        /// <param name="folderName">资源存放的文件夹</param>
        /// <returns></returns>
        internal static string GetFileName(string requestPath, string folderName)
        {
            var folders = requestPath.Split(new string[] { $"{ResourceRequestPrefix}/{folderName}" }, StringSplitOptions.RemoveEmptyEntries);
            if (folders != null)
            {
                return folders[folders.Length - 1].Replace("/", ".");
            }
            return default;
        }
        /// <summary>
        /// 获取文件资源的名字
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        internal static string GetContentResourceName(string contentFolder, string resourceName)
        {
            return $"{GetContentFolderNamespace(contentFolder)}{resourceName}";
        }
        /// <summary>
        /// 获取文件夹的名称
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        internal static string GetContentFolderNamespace(string contentFolder)
        {
            return $"{typeof(DashboardRoute).Namespace}.Content.{contentFolder}";
        }
    }
}
