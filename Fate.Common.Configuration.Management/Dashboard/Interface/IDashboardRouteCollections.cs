using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Dashboard.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-10-31
    /// 当前集合为了操作静态资源的信息
    /// </summary>
    public interface IDashboardRouteCollections
    {
        /// <summary>
        /// 获取资源的信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Tuple<string, string> Get(string key);
        /// <summary>
        /// 添加一个路由规则
        /// </summary>
        /// <param name="pathTemplate"></param>
        /// <param name="folderName"></param>
        /// <param name="contentType"></param>
        void Add(string pathTemplate, string folderName, string contentType);
    }
}
