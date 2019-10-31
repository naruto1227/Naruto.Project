using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Fate.Common.Configuration.Management.Dashboard.Interface;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 存放静态资源的路由集合
    /// </summary>
    public class DashboardRouteCollections : IDashboardRouteCollections
    {
        /// <summary>
        /// 存放路由的集合 
        /// </summary>
        private readonly Dictionary<string, Tuple<string, string>> routes = new Dictionary<string, Tuple<string, string>>();

        /// <summary>
        /// 获取资源的路由信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<string, string> Get(string key)
        {
            if (!routes.ContainsKey(key))
                return default;
            return routes.Where(a => a.Key.Equals(key)).Select(a => a.Value).FirstOrDefault();
        }

        /// <summary>
        /// 添加一个路由规则
        /// </summary>
        /// <param name="pathTemplate"></param>
        /// <param name="contentType"></param>
        /// <param name="folderName">目录的名称</param>
        public void Add(string pathTemplate, string folderName, string contentType)
        {
            routes.Add(pathTemplate, (folderName, contentType).ToTuple());
        }
    }
}
