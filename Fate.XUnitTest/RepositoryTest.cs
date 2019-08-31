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

        public RepositoryTest()
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
        }
        [Fact]
        public async Task Test()
        {

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
        [Fact]
        public async Task bulkAdd()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            ConcurrentQueue<setting> settings1 = new ConcurrentQueue<setting>();

            Parallel.For(0, 10000, (item) =>
            {
                settings1.Enqueue(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            });
            await unit.Command<setting>().BulkAddAsync(settings1);
            await unit.SaveChangeAsync();
        }
        [Fact]
        public async Task Query()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            await unit.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
        }

        [Fact]
        public async Task ChangeDataBase()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            await unit.ChangeDataBase("test1");

            var str = await unit.Query<setting>().AsQueryable().ToListAsync();
            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
           await unit.SaveChangeAsync();
            str = await unit.Query<setting>().AsQueryable().ToListAsync();
            await unit.ChangeDataBase("test");
            str = await unit.Query<setting>().AsQueryable().ToListAsync();

        }

        [Fact]
        public async Task WriteRead()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var str = await unit.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            str = await unit.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await unit.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task Tran()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
           
            unit.BeginTransaction();
            await unit.ChangeDataBase("test1");
            var str = await unit.Query<setting>().AsQueryable().ToListAsync();
            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            //str = await unit.Query<setting>().AsQueryable().ToListAsync();
            //await unit.ChangeDataBase("test");
            //str = await unit.Query<setting>().AsQueryable().ToListAsync();
            unit.CommitTransaction();
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
