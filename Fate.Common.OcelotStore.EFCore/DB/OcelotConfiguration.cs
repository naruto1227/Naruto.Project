using System;
using System.Collections.Generic;
using System.Text;
using Fate.Common.Base.Model;

namespace Fate.Common.OcelotStore.EFCore
{
    /// <summary>
    /// 张海波
    /// 2019-08-14
    /// ocelot的数据存放表
    /// </summary>
    public class OcelotConfiguration : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// ocelot的路由
        /// </summary>
        public string ReRoutes { get; set; }

        /// <summary>
        /// 自定义路由（暂时用不到）
        /// </summary>
        public string DynamicReRoutes { get; set; }
        /// <summary>
        /// 请求聚合
        /// </summary>
        public string Aggregates { get; set; }
        /// <summary>
        /// 全局配置
        /// </summary>
        public string GlobalConfiguration { get; set; }
    }
}
