using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-10-31
    /// 当前集合为了操作静态资源的信息
    /// </summary>
    public interface IVirtualFileRouteCollections
    {
        /// <summary>
        /// 获取资源的信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Tuple<string, string> Get(string key);
        /// <summary>
        /// 添加一个路由规则
        /// </summary>
        /// <param name="pathTemplate">请求的地址</param>
        /// <param name="folderName">相对于 ResouresDirectoryPrefix 值的目录名称</param>
        /// <param name="contentType"></param>
        void Add(string pathTemplate, string folderName, string contentType);
    }
}
