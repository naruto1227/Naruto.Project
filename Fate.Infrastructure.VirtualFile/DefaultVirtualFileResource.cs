using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.VirtualFile
{

    /// <summary>
    /// 虚拟文件资源信息操作的默认实现
    /// </summary>
    public class DefaultVirtualFileResource : IVirtualFileResource
    {
        /// <summary>
        /// 资源的命名空间
        /// </summary>
        private readonly string ResouresDirectoryPrefix;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="virtualFileOptions">配置参数</param>
        /// <param name="routeCollections">路由的集合</param>
        public DefaultVirtualFileResource(IOptions<VirtualFileOptions> virtualFileOptions, IVirtualFileRouteCollections routeCollections)
        {
            ResouresDirectoryPrefix = virtualFileOptions.Value.ResouresDirectoryPrefix;

            LoadResoure(routeCollections);
        }

        /// <summary>
        /// 加载资源信息到集合
        /// </summary>
        /// <param name="routeCollections"></param>
        private void LoadResoure(IVirtualFileRouteCollections routeCollections)
        {
            //初始化资源信息
            foreach (var item in VirtualFileResoureInfos.Javascripts.Names)
            {
                routeCollections.Add($"{VirtualFileResoureInfos.ResourceRequestPrefix}/{VirtualFileResoureInfos.Javascripts.DirectoryName}/{item}.js", VirtualFileResoureInfos.Javascripts.DirectoryName, "application/x-javascript");
            }
            foreach (var item in VirtualFileResoureInfos.Stylesheets.Names)
            {
                routeCollections.Add($"{VirtualFileResoureInfos.ResourceRequestPrefix}/{VirtualFileResoureInfos.Stylesheets.DirectoryName}/{item}.css", VirtualFileResoureInfos.Stylesheets.DirectoryName, "application/x-javascript");
            }
            foreach (var item in VirtualFileResoureInfos.HtmlPages.Names)
            {
                routeCollections.Add($"{VirtualFileResoureInfos.ResourceRequestPrefix}/{VirtualFileResoureInfos.HtmlPages.DirectoryName}/{item}.html", VirtualFileResoureInfos.HtmlPages.DirectoryName,
                  "text/html");
            }

            foreach (var item in VirtualFileResoureInfos.Jsons.Names)
            {
                routeCollections.Add($"{VirtualFileResoureInfos.ResourceRequestPrefix}/{VirtualFileResoureInfos.Jsons.DirectoryName}/{item}.json", VirtualFileResoureInfos.Jsons.DirectoryName,
                  "text/json");
            }
            foreach (var item in VirtualFileResoureInfos.Pngs.Names)
            {
                routeCollections.Add($"{VirtualFileResoureInfos.ResourceRequestPrefix}/{VirtualFileResoureInfos.Pngs.DirectoryName}/{item}.png", VirtualFileResoureInfos.Pngs.DirectoryName,
                  "image/png");
            }
        }
        /// <summary>
        /// 获取请求的文件名
        /// </summary>
        /// <param name="requestPath">请求的路径</param>
        /// <param name="folderName">资源存放的文件夹(例 data/data1)</param>
        /// <returns></returns>
        public string GetFileName(string requestPath, string folderName)
        {
            var folders = requestPath.Split(new string[] { $"{VirtualFileResoureInfos.ResourceRequestPrefix}/{folderName}" }, StringSplitOptions.RemoveEmptyEntries);
            if (folders != null)
            {
                return folders[folders.Length - 1].Replace("/", ".");
            }
            return default;
        }
        /// <summary>
        /// 获取文件资源的名字
        /// </summary>
        /// <param name="contentFolder">资源存在的目录 当为多级目录的时候 应为/分割</param>
        /// <param name="resourceName">资源的名称</param>
        /// <returns></returns>
        public string GetContentResourceName(string contentFolder, string resourceName)
        {
            return $"{GetContentFolderNamespace(contentFolder)}{resourceName}".Replace("/", ".");
        }
        /// <summary>
        /// 获取文件夹的名称
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <returns></returns>
        private string GetContentFolderNamespace(string contentFolder)
        {
            return $"{ResouresDirectoryPrefix}.{contentFolder}";
        }
    }
}
