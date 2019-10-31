using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-10-31
    /// 面板路由配置接口
    /// </summary>
    public interface IDashboardRoute
    {
        string ResourceRequestPrefix { get; }

        string MainPageName { get; }

        /// <summary>
        /// 获取文件的名字
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        string GetFileName(string requestPath, string folderName);

        /// <summary>
        /// 获取资源的名称
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        string GetContentResourceName(string contentFolder, string resourceName);
    }
}
