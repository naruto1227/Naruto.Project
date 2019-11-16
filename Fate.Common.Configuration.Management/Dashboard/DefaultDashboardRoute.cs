using Fate.Common.Configuration.Management.Dashboard.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 面板的路由 配置
    /// </summary>
    public class DefaultDashboardRoute : IDashboardRoute
    {
        /// <summary>
        /// 资源的请求前缀
        /// </summary>
        public string ResourceRequestPrefix { get => "/fate"; }
        /// <summary>
        /// 首页的地址
        /// </summary>
        public string MainPageName { get => "/fate/pages/index.html"; }

        /// <summary>
        /// 页面的资源
        /// </summary>
        private readonly string[] HtmlPages = {
            "index.html"
        };
        /// <summary>
        /// 存放用到的js文件的名字
        /// </summary>
        private readonly string[] Javascripts =
       {
            "MD5.js",
        };

        /// <summary>
        /// 存放需要用的样式的名字
        /// </summary>
        private readonly string[] Stylesheets =
        {

        };


        /// <summary>
        /// 初始化
        /// </summary>
        public DefaultDashboardRoute(IDashboardRouteCollections routeCollections)
        {
            foreach (var item in Javascripts)
            {
                routeCollections.Add($"{ResourceRequestPrefix}/js/{item}", "js", "application/x-javascript");
            }
            foreach (var item in Stylesheets)
            {
                routeCollections.Add($"{ResourceRequestPrefix}/css/{item}", "css", "application/x-javascript");
            }
            foreach (var item in HtmlPages)
            {
                routeCollections.Add($"{ResourceRequestPrefix}/pages/{item}", "pages", "text/html");
            }
        }
        /// <summary>
        /// 获取请求的文件名
        /// </summary>
        /// <param name="requestPath">请求的路径</param>
        /// <param name="folderName">资源存放的文件夹</param>
        /// <returns></returns>
        public string GetFileName(string requestPath, string folderName)
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
        public string GetContentResourceName(string contentFolder, string resourceName)
        {
            return $"{GetContentFolderNamespace(contentFolder)}{resourceName}";
        }
        /// <summary>
        /// 获取文件夹的名称
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        private string GetContentFolderNamespace(string contentFolder)
        {
            return $"{typeof(DefaultDashboardRoute).Namespace}.Content.{contentFolder}";
        }
    }
}
