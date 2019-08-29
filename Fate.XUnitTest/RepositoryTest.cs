using Consul;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Fate.Commom.Consul;
using System.Net;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Fate.Common.Repository;
using Fate.Common.Repository.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Fate.Domain.Model;
using Fate.Domain.Model.Entities;
using System.Threading.Tasks;

namespace Fate.XUnitTest
{
    public class RepositoryTest
    {
        IServiceCollection services = new ServiceCollection();

        private DbContext dbContex;
        [Fact]
        public async Task Test()
        {
            services.AddScoped(typeof(IRepositoryFactory), typeof(RepositoryFactory));
            //注入mysql仓储   //注入多个ef配置信息
            services.AddMysqlRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;");
                options.ReadOnlyConnectionString = "Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;".Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                //
                options.UseEntityFramework<MysqlDbContent>(services);
                options.IsOpenMasterSlave = true;
            });
            var iserverPri = services.BuildServiceProvider();

            //var u0k = iserverPri.GetRequiredService<MysqlDbContent>();


            //var re = iserverPri.GetRequiredService<IRepositoryFactory>();

            //re.dbContext = u0k;
            //dbContex = u0k;
            //u0k.Dispose();
            //var re2 = iserverPri.GetRequiredService<IRepositoryFactory>();

            var u0k = iserverPri.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var res = await u0k.Query<setting>().AsQueryable().FirstOrDefaultAsync();
        }

    }


    public interface IRepositoryFactory
    {
        DbContext dbContext { get; set; }
    }
    public class RepositoryFactory : IRepositoryFactory
    {
        public DbContext dbContext { get; set; }
    }

}
