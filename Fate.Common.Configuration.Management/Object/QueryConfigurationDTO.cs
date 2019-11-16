using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.Object
{
    /// <summary>
    ///接口查询参数 
    /// </summary>
    public class QueryConfigurationDTO: BaseQueryConfigurationDTO
    {
        public int Page { get; set; }

        public int Limit { get; set; }
    }

    public class BaseQueryConfigurationDTO
    {
        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 环境类型
        /// </summary>
        public int EnvironmentType { get; set; }
    }
}
