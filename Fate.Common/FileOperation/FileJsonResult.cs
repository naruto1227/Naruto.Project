using Fate.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.FileOperation
{
    /// <summary>
    /// 文件上传返回结果的类
    /// </summary>
    public class FileJsonResult : ICommonClassDependency
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 请求名称
        /// </summary>
        public string requestName { get; set; }
    }
}
