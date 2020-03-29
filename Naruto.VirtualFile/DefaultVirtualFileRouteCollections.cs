using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

namespace Naruto.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-11-24
    /// 存放静态资源的路由集合
    /// </summary>
    public class DefaultVirtualFileRouteCollections : IVirtualFileRouteCollections
    {
        /// <summary>
        /// 存放路由的集合 
        /// 第一个参数为资源的请求地址
        /// 第二个参数为所处目录的名称 （相互与 文件所处的根路径的名称 多个目录之间用/分割  ，例 js/test ）为了请求的时候用于过滤拼接资源名称
        /// 第三个参数为contentType
        /// </summary>
        private readonly ConcurrentDictionary<string, Tuple<string, string>> routes = new ConcurrentDictionary<string, Tuple<string, string>>();

        /// <summary>
        /// 获取资源的路由信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<string, string> Get(string key)
        {
            routes.TryGetValue(key, out var res);
            return res;
        }

        /// <summary>
        /// 添加一个路由规则
        /// </summary>
        /// <param name="pathTemplate">请求地址模板</param>
        /// <param name="mimeType">类型</param>
        /// <param name="folderName">目录的名称</param>
        public void Add(string pathTemplate, string folderName, string mimeType)
        {
            routes.TryAdd(pathTemplate, (folderName, mimeType).ToTuple());
        }
    }
}
