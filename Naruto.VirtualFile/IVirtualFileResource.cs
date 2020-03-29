using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-11-24
    /// 虚拟文件的资源名称信息的操作
    /// </summary>
    public interface IVirtualFileResource
    {
        /// <summary>
        /// 获取文件的名字
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        string GetFileName(string requestPath, string folderName);

        /// <summary>
        /// 获取资源的名称
        /// </summary>
        /// <param name="contentFolder"></param>
        /// <param name="resourceName">相对于 ResouresDirectoryPrefix 值的目录名称</param>
        /// <returns></returns>
        string GetContentResourceName(string contentFolder, string resourceName);
    }
}
