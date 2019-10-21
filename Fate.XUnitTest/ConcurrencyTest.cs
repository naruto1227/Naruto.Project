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
            services.AddRepositoryServer().AddRepositoryEFOptionServer(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hks360;Charset=utf8;");
                //
                options.UseEntityFramework<MysqlDbContent>(services);
                options.IsOpenMasterSlave = false;
            });
        }
        /// <summary>
        /// 测试保存的并发
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task test()
        {
            MysqlDbContent mysqlDbContent = services.BuildServiceProvider().GetRequiredService<MysqlDbContent>();
            var info = mysqlDbContent.setting.FirstOrDefault(a => a.Id == 18);

            info.Contact = "142";
            //伪造并发
            mysqlDbContent.Database.ExecuteSqlCommand("update setting set Contact='37a1222111' where Id=" + info.Id);
            try
            {
                var s = mysqlDbContent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) //配置并发令牌之后 触发的事件
            {
                foreach (var entry in ex.Entries)
                {
                    //第一种
                    //foreach (var property in entry.Metadata.GetProperties())
                    //{
                    //    ////当前值
                    //    //var proposedValue = entry.Property(property.Name).CurrentValue;

                    //    ////原始值
                    //    //var originalValue = entry.Property(property.Name).OriginalValue;

                    //    //数据库值
                    //    var databaseValue = entry.GetDatabaseValues().GetValue<object>(property.Name);
                    //    //原始值等于数据库 保存数据库的值和原始值一直 防止提交的时候再次报错
                    //    entry.Property(property.Name).OriginalValue = databaseValue;
                    //}
                    ////重新提交
                    //entry.Context.SaveChanges();



                    //第二种

                    // 保存数据库的值和原始值一直 防止提交的时候再次报错
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    entry.Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
