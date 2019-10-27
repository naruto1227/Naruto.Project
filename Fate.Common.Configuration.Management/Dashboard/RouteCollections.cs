using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Fate.Common.Configuration.Management.Dashboard
{
    /// <summary>
    /// 路由的几个
    /// </summary>
    internal class RouteCollections
    {
        /// <summary>
        /// 存放路由的集合 
        /// </summary>
        private readonly List<Tuple<string, IDashboardRender>> routes = new List<Tuple<string, IDashboardRender>>();

        public IDashboardRender this[string index]
        {
            get
            {
                return routes.Where(a => a.Item1.StartsWith(index)).Select(a => a.Item2).FirstOrDefault();
            }
        }
        /// <summary>
        /// 添加一个路由规则
        /// </summary>
        /// <param name="pathTemplate"></param>
        /// <param name="dashboardRender"></param>
        public void Add(string pathTemplate, IDashboardRender dashboardRender)
        {
            routes.Add(new Tuple<string, IDashboardRender>(pathTemplate, dashboardRender));
        }

    }
}
