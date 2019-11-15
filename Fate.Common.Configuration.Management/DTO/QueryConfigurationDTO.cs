using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.DTO
{
    public class QueryConfigurationDTO
    {
        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 环境类型
        /// </summary>
        public int EnvironmentType { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }
    }
}
