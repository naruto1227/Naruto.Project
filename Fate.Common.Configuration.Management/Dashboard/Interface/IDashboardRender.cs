using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration.Management.Dashboard.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-10-27
    /// 面板的资源的渲染
    /// </summary>
    public interface IDashboardRender
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <returns></returns>
        Task LoadAsync(DashboardContext dashboardContext);
    }
}
