
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Naruto.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 虚拟文件的参数
    /// </summary>
    public class VirtualFileOptions
    {
        /// <summary>
        /// 请求地址的前缀 默认(/Naruto)
        /// </summary>
        public PathString RequestPath { get; set; } = new PathString("/Naruto");

        /// <summary>
        /// 资源所处的程序集
        /// </summary>
        public Assembly ResouresAssembly { get; set; }

        /// <summary>
        /// 资源的前缀(例 Naruto.VirtualFile.Content)
        /// </summary>
        public string ResouresDirectoryPrefix { get; set; }

        /// <summary>
        /// 虚拟文件的授权接口过滤器
        /// </summary>
        public IEnumerable<IVirtualFileAuthorizationFilters> Authorization { get; set; }
    }
}
