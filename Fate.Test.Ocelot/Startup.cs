using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.Common.OcelotStore.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.File;

namespace Fate.Test.Ocelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddEFCache(options =>
            {
                options.EFOptions = ef => ef.ConfigureDbContext = context => context.UseMySql(Configuration.GetConnectionString("OcelotMysqlConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.MapWhen(content => content.Request.Path.StartsWithSegments("/test"), build =>
            {
                var ocelotCache = build.ApplicationServices.GetService<IOcelotCache<FileConfiguration>>();
                ocelotCache.ClearRegion("ocelotef");
                EFConfigurationProvider.Get(build);
                //build.UseMiddleware<testmidware>();
            });
            await app.UseOcelot();

        }
    }
}
