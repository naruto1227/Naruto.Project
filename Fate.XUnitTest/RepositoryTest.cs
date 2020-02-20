using Consul;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Fate.Infrastructure.Consul;
using System.Net;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Fate.Infrastructure.Repository;
using Fate.Infrastructure.Repository.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Fate.Domain.Model;
using Fate.Domain.Model.Entities;
using System.Threading.Tasks;
#if NETCOREAPP
using Fate.Infrastructure.Repository.Interceptor;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore.Query;
#else
using Fate.Infrastructure.Repository.Interceptor;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore.Query;
#endif
namespace Fate.XUnitTest
{
    public class RepositoryTest
    {
        IServiceCollection services = new ServiceCollection();

        private DbContext dbContex;

        public RepositoryTest()
        {
            // services.AddScoped(typeof(IRepositoryFactory), typeof(RepositoryFactory));
            //注入mysql仓储   //注入多个ef配置信息
            services.AddRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;").AddInterceptors(new EFDbCommandInterceptor());
                options.ReadOnlyConnectionString = new string[] { "Database=test1;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;" };
                //
                options.UseEntityFramework<MysqlDbContent, SlaveMysqlDbContent>(true, 100);
                options.IsOpenMasterSlave = true;
            }, Test =>
            {
                Test.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;").AddInterceptors(new EFDbCommandInterceptor());
                Test.ReadOnlyConnectionString = new string[] { "Database=test1;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;" };
                //
                Test.UseEntityFramework<TestDbContent, SlaveTestDbContent>(true, 100);
                Test.IsOpenMasterSlave = true;
            });

            ////注入mysql仓储   //注入多个ef配置信息
            //services.AddRepositoryServer().AddRepositoryEFOptionServer(options =>
            //{
            //    options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=192.168.18.227;Port=3308;UserId=hai;Password=123456;Charset=utf8mb4;").AddInterceptors(new EFDbCommandInterceptor());
            //    options.ReadOnlyConnectionString = new string[] { "Database=test;DataSource=192.168.18.227;Port=3309;UserId=hairead;Password=123456;Charset=utf8mb4;"
            //    ,"Database=test;DataSource=192.168.18.227;Port=3310;UserId=hairead;Password=123456;Charset=utf8mb4;"
            //    };
            //    //
            //    options.UseEntityFramework<MysqlDbContent, SlaveMysqlDbContent>(true, 100);
            //    options.IsOpenMasterSlave = true;
            //}, Test =>
            //{
            //    Test.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=192.168.18.227;Port=3308;UserId=hai;Password=123456;Charset=utf8mb4;").AddInterceptors(new EFDbCommandInterceptor());
            //    Test.ReadOnlyConnectionString = new string[] { "Database=test;DataSource=192.168.18.227;Port=3309;UserId=hairead;Password=123456;Charset=utf8mb4;"
            //    ,"Database=test;DataSource=192.168.18.227;Port=3310;UserId=hairead;Password=123456;Charset=utf8mb4;"
            //    };
            //    //
            //    Test.UseEntityFramework<TestDbContent, SlaveTestDbContent>(true, 100);
            //    Test.IsOpenMasterSlave = true;
            //});
        }
        [Fact]
        public void Test()
        {
            CancellationToken cancellationToken;
            cancellationToken.ThrowIfCancellationRequested();
            var iserverPri = services.BuildServiceProvider();

            var mysqlDbContent = iserverPri.GetRequiredService<MysqlDbContent>();
            DbContextOptions<MysqlDbContent> options = new DbContextOptions<MysqlDbContent>();

            var db2 = (DbContext)Activator.CreateInstance(typeof(MysqlDbContent), options);
            var db3 = mysqlDbContent.Clone();
            //var re = iserverPri.GetRequiredService<IRepositoryFactory>();
            var list22 = mysqlDbContent.test1.Where(a => 1 == 1).ToList();
            mysqlDbContent.Dispose();

            list22 = db3.test1.Where(a => 1 == 1).ToList();
            db3.Dispose();
            list22 = db3.test1.Where(a => 1 == 1).ToList();
            var list222 = mysqlDbContent.test1.Where(a => 1 == 1).ToList();

            mysqlDbContent.setting.Add(new setting()
            {
                Contact = "",
                Description = "",
                DuringTime = "",
                Integral = 1,
                Rule = ""
            });
            db3.Database.GetDbConnection().Close();
            db3.Database.GetDbConnection().ConnectionString = "Database=test1;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;";
            db3.Database.GetDbConnection().Open();
            db3.setting.Add(new setting()
            {
                Contact = "",
                Description = "",
                DuringTime = "",
                Integral = 1,
                Rule = ""
            });

            mysqlDbContent.SaveChanges();
            //re.dbContext = u0k;
            //dbContex = u0k;
            //u0k.Dispose();
            //var re2 = iserverPri.GetRequiredService<IRepositoryFactory>();

            var u0k = iserverPri.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            u0k.CommandTimeout = 40;
            //u0k.Query<setting>().AsQueryable().OrderBy("Rule").ToList();
            for (int i = 0; i < 100; i++)
            {
                u0k.Command<setting>().Add(new setting() { Id = new Random().Next(100000, 999999) });
            }

            u0k.SaveChanges();
            // var res2 = u0k.Query<setting>().AsQueryable().OrderBy("Rule").ToList();
            var res = u0k.Query<setting>().AsQueryable().FirstOrDefault();
        }
        [Fact]
        public async Task bulkAdd()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            var unit = services.BuildServiceProvider().GetService(typeof(IUnitOfWork<>).MakeGenericType(typeof(MysqlDbContent))) as IUnitOfWork;

            ConcurrentQueue<setting> settings1 = new ConcurrentQueue<setting>();

            Parallel.For(0, 100, (item) =>
            {
                settings1.Enqueue(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            });
            await unit.Command<setting>().BulkAddAsync(settings1, cancellationToken.Token);
            await unit.SaveChangeAsync(cancellationToken.Token);
        }
        [Fact]
        public async Task Query()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            //
            // await unit.ChangeReadOrWriteConnection(Common.Repository.Object.ReadWriteEnum.Read);
            // await unit.ChangeDataBase("test1");
            //var sql =  unit.Query<setting>().AsQueryable().ToSql(services.BuildServiceProvider().GetRequiredService<MysqlDbContent>());
            // var sql = unit.Query<setting>().AsQueryable().ToSql();
            var str2 = "";
            var str = unit.Query<setting>().AsQueryable().Where(a => a.Description == str2).ToSqlWithParams();
            var res = await unit.Query<setting>().AsQueryable().Where(a => a.Description == str2).ToListAsync();
        }

        [Fact]
        public async Task ChangeDataBase()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            unit.CommandTimeout = 40;
            await unit.ChangeDataBaseAsync("test1");

            var str = await unit.Query<setting>().AsQueryable().ToListAsync();
            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await unit.Query<setting>().AsQueryable().ToListAsync();
            await unit.ChangeDataBaseAsync("test");
            str = await unit.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task ManyContextWriteRead()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var unit2 = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<TestDbContent>>();
            var str = await unit.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            str = await unit2.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();

            str = await unit.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await unit.Query<setting>(true).AsQueryable().AsNoTracking().ToListAsync();
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
            str = await unit.Query<setting>(true).AsQueryable().AsNoTracking().ToListAsync();
            str = await unit.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task Tran()
        {
            using (var servicesScope = services.BuildServiceProvider().CreateScope())
            {
                var unit = servicesScope.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
                var str = await unit.Query<setting>().AsQueryable().ToListAsync();
                await unit.ChangeDataBaseAsync("test1");
                await unit.BeginTransactionAsync();
                unit.CommandTimeout = 40;
                str = await unit.Query<setting>().AsQueryable().ToListAsync();
                await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await unit.SaveChangeAsync();
                //str = await unit.Query<setting>().AsQueryable().ToListAsync();
                //await unit.ChangeDataBase("test");
                str = await unit.Query<setting>().AsQueryable().ToListAsync();
                await unit.CommitTransactionAsync();
                str = await unit.Query<setting>().AsQueryable().ToListAsync();
            }
        }

        [Fact]
        public async Task Tran2()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();

            await unit.BeginTransactionAsync();

            await unit.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });

            await unit.SaveChangeAsync();
            await unit.CommitTransactionAsync();
        }

        [Fact]
        public void DataTableTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            unit.CommandTimeout = 40;
            var dt = unit.SqlQuery().ExecuteSqlQuery("select  * from setting");
            unit.ChangeDataBaseAsync("test1");
            dt = unit.SqlQuery().ExecuteSqlQuery("select  * from setting");
        }
        [Fact]
        public async Task DataTableAsyncTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var dt = await unit.SqlQuery().ExecuteSqlQueryAsync("    1select  * from setting");

        }

        [Fact]
        public async Task ExecSqlTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            //await unit.ChangeDataBaseAsync("test1");
            await unit.BeginTransactionAsync();

            unit.CommandTimeout = 180;
            var res = await unit.SqlCommand().ExecuteNonQueryAsync("delete from setting");
            unit.CommitTransaction();
        }
        [Fact]
        public async Task ExecuteScalarAsync()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var query = unit.SqlQuery();
            //  await unit.ChangeDataBaseAsync("test1");
            unit.CommandTimeout = 180;
            var res = await query.ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
            unit.CommandTimeout = 110;
            res = await unit.SqlQuery(true).ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
            await query.ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
        }

        /// <summary>
        /// 测试多工作单元的事务批量提交方式
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MoreUok()
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var IUnitOfWork2 = scope.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
                var IUnitOfWork3 = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContent>>();
                var unitOfWorkTran = scope.ServiceProvider.GetRequiredService<IUnitOfWorkTran>();
                //统一开启事务
                await unitOfWorkTran.BeginTransactionAsync();
                await IUnitOfWork2.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await IUnitOfWork2.SaveChangeAsync();
                await IUnitOfWork3.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await IUnitOfWork3.SaveChangeAsync();
                //统一提交事务
                await unitOfWorkTran.CommitTransactionAsync();
            }
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
