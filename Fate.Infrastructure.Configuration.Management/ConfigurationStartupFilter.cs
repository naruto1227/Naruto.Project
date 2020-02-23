using Fate.Infrastructure.Configuration.Management.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fate.Infrastructure.Configuration.Management
{
    /// <summary>
    /// 张海波
    /// 2019-11-18
    /// 启用获取配置数据中间件
    /// </summary>
    public class ConfigurationDataStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                //注册配置数据获取中间件
                app.UseMiddleware<ConfigurationDataMiddleware>();
                next(app);
            };
        }
    }
}
