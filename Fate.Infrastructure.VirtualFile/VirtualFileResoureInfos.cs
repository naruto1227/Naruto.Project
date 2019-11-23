
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.VirtualFile
{
    /// <summary>
    /// 资源信息实体
    /// </summary>
    public class ResoureInfo
    {
        /// <summary>
        /// 资源的目录名称 (相对于 ResouresDirectoryPrefix 值的目录名称 多目录用/分割) 例 images/png
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        /// 资源的名称
        /// </summary>
        public string[] Names { get; set; } = new string[] { };
    }
    /// <summary>
    /// 资源的名称配置
    /// </summary>
    public class VirtualFileResoureInfos
    {
        /// <summary>
        /// 资源的请求前缀
        /// </summary>
        public static string ResourceRequestPrefix { get => "/fate"; }

        /// <summary>
        /// 页面的资源
        /// 第一个参数为资源存在的目录名称
        /// 第二个为
        /// </summary>
        public static ResoureInfo HtmlPages { get; set; } = new ResoureInfo();
        /// <summary>
        /// 存放用到的js文件的名字
        /// </summary>
        public static ResoureInfo Javascripts { get; set; } = new ResoureInfo();
        /// <summary>
        /// 存放需要用的样式的名字
        /// </summary>
        public static ResoureInfo Stylesheets { get; set; } = new ResoureInfo();
        /// <summary>
        /// json文件
        /// </summary>
        public static ResoureInfo Jsons { get; set; } = new ResoureInfo();

        /// <summary>
        /// png文件
        /// </summary>
        public static ResoureInfo Pngs { get; set; } = new ResoureInfo();
    }
}
