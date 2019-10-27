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
    public class ConfigurationStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<DashBoardMiddleware>();
            };
        }
    }
}
