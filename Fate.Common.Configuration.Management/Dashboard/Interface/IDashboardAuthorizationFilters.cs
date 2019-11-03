using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management.Dashboard.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-11-3
    /// 面板的授权接口过滤实现
    /// </summary>
    public interface IDashboardAuthorizationFilters
    {
        Task<bool> AuthorizationAsync(DashboardContext context);
    } 
}
