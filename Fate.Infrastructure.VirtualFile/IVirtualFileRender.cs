using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 面板的资源的渲染
    /// </summary>
    public interface IVirtualFileRender
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <returns></returns>
        Task LoadAsync(VirtualFileContext dashboardContext);
    }
}
