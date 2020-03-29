
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Naruto.VirtualFile
{

    /// <summary>
    /// 张海波
    /// 2019-10-31
    /// 注入静态中间件
    /// </summary>
    public class VirtualFileStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                //注册面板中间件
                app.UseMiddleware<VirtualFileMiddleware>();
                next(app);
            };
        }
    }
}
