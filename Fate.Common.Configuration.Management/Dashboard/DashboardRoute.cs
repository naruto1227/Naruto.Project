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
            Routes.Add("/fate/js/MD5.js", new DashboardRender());
        }
        /// <summary>
        /// 路由集合
        /// </summary>
        public static RouteCollections Routes { get; }
    }
}
