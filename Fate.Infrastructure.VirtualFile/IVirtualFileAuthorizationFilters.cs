using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.VirtualFile
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 虚拟文件系统的授权接口过滤实现
    /// </summary>
    public interface IVirtualFileAuthorizationFilters
    {
        Task<bool> AuthorizationAsync(VirtualFileContext context);
    }
}
