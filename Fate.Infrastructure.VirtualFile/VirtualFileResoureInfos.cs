
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
        /// 资源的 mime类型
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// 资源的名称
        /// </summary>
        public string[] Names { get; set; } = new string[] { };
    }
    /// <summary>
    /// 资源的名称配置
    /// </summary>
    public static class VirtualFileResoureInfos
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        internal static List<ResoureInfo> ResoureInfo { get; set; } = new List<ResoureInfo>();

        public static void Add(ResoureInfo resoureInfo)
        {
            ResoureInfo.Add(resoureInfo);
        }
        public static void Add(IList<ResoureInfo> resoureInfos)
        {
            ResoureInfo.AddRange(resoureInfos);
        }
    }
}
