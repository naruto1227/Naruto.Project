using Fate.Common.Configuration.Management.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fate.Common.Configuration.Management
{

    /// <summary>
    /// 张海波
    /// 2019-10-31
    /// 注入面板中间件
    /// </summary>
    public class ConfigurationStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                //注册面板中间件
                app.UseMiddleware<DashBoardMiddleware>();
                next(app);
            };
        }
    }
}
