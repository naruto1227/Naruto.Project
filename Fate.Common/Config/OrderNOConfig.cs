using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Config
{
    /// <summary>
    /// 单号表的表名的配置
    /// </summary>
    public class OrderNOConfig
    {
        /// <summary>
        /// 单号表的集合
        /// </summary>
        public static IReadOnlyList<string> TableNameList = new List<string>() { "table1", "table2" };
    }
}
