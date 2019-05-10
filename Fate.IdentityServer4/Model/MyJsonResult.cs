using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fate.IdentityServer4.Model
{
    /// <summary>
    /// 返回值
    /// </summary>
    public class MyJsonResult
    {
        /// <summary>
        /// 错误代码 0 正常
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string FailMsg { get; set; }

        /// <summary>
        /// 结果存放token 的信息
        /// </summary>
        public object Rows { get; set; }
    }
}
