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
using System;

namespace Fate.XUnitTest
{
    public class ConcurrencyTest
    {
        IServiceCollection services = new ServiceCollection();
        public ConcurrencyTest()
        {
            services.AddScoped(typeof(IRepositoryFactory), typeof(RepositoryFactory));
            //注入mysql仓储   //注入多个ef配置信息
            services.AddMysqlRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;");
                //
                options.UseEntityFramework<MysqlDbContent>(services);
                options.IsOpenMasterSlave = false;
            });
        }

        [Fact]
        public async Task test()
        {
            MysqlDbContent mysqlDbContent = services.BuildServiceProvider().GetRequiredService<MysqlDbContent>();
             var info = mysqlDbContent.setting.FirstOrDefault();

            info.Contact = "22";

            mysqlDbContent.Database.ExecuteSqlCommand("update setting set Contact='111' where Id=" + info.Id);
            try
            {
                var s = mysqlDbContent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Person)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
         
        }
    }
}
