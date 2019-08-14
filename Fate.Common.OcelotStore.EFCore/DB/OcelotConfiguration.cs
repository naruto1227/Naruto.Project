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
        /// ocelot的配置信息
        /// </summary>
        public string Config { get; set; }
    }
}
