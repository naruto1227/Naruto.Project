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

namespace Fate.XUnitTest
{
    public class ThreadTest
    {
        IServiceCollection services = new ServiceCollection();
        [Fact]
        public void test()
        {
            ConcurrentQueue<IConsulClient> consulClients = new ConcurrentQueue<IConsulClient>();

            services.AddConsul(options =>
            {
                options.Port = 8521;
                options.Scheme = SchemeEnum.Http;
            });
            //services.UseServiceRegister(new Commom.Consul.Object.RegisterConfiguration() { ServerName = " ceshi", TcpHealthCheck = new IPEndPoint(IPAddress.Parse("192.168.18.171"), 5000), Address = new IPEndPoint(IPAddress.Parse("192.168.18.171"), 5000) });
            var consulClientFactory = services.BuildServiceProvider().GetRequiredService<IConsulClientFactory>();
            var option = services.BuildServiceProvider().GetRequiredService<IOptions<ConsulClientOptions>>();
            var client = consulClientFactory.Get(option.Value);
            var res = client.KV.Get("test2").GetAwaiter().GetResult();
            consulClients.Enqueue(client);


            for (int i = 0; i < 1000; i++)
            {

                Thread.Sleep(100000);
                Thread thread = new Thread(() =>
                {
                    var client2 = consulClients.FirstOrDefault();
                    res = client2.KV.Get("test2").GetAwaiter().GetResult();
                    Assert.NotNull(res.Response);
                });
                thread.IsBackground = true;
                thread.Start();
            }


        }


        [Fact]
        public async Task unitofWork()
        {

            //注入mysql仓储   //注入多个ef配置信息
            services.AddRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;");
                options.ReadOnlyConnectionString = "Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;".Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                //
                options.UseEntityFramework<MysqlDbContent>();
                options.IsOpenMasterSlave = true;
            });
            var iserverPri = services.BuildServiceProvider();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, 100, async (item) =>
            {
                using (var server = iserverPri.CreateScope())
                {
                    var unitOfWork = server.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
                     //  await unitOfWork.ChangeReadOrWriteConnection(Fate.Infrastructure.Repository.Object.ReadWriteEnum.ReadWrite);
                     await unitOfWork.Query<setting>().AsQueryable().ToListAsync();
                    await unitOfWork.Command<setting>().AddAsync(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
                    await unitOfWork.SaveChangeAsync();
                }
            });

            //for (int i = 0; i < 100; i++)
            //{
            //    Thread thread = new Thread(async () =>
            //    {
            //        using (var server = iserverPri.CreateScope())
            //        {
            //            var unitOfWork = server.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            //            await unitOfWork.ChangeReadOrWriteConnection(Common.Repository.Base.ReadWriteEnum.ReadWrite);
            //            await unitOfWork.Respositiy<setting>().AsQueryable().ToListAsync();
            //            await unitOfWork.Respositiy<setting>().AddAsync(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
            //            await unitOfWork.SaveChangeAsync();
            //        }
            //    });
            //    thread.IsBackground = true;
            //    thread.Start();
            //}
            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);

        }

    }
}
