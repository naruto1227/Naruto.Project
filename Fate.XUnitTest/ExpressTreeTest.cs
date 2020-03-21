using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Fate.Infrastructure.Repository.Interceptor;
using Microsoft.EntityFrameworkCore;
using Fate.Domain.Model;
using System.Diagnostics;
using Fate.Infrastructure.Repository.UnitOfWork;
using Fate.Domain.Model.Entities;
using ExpressionToString;
using System.Reflection;

namespace Fate.XUnitTest
{
    /// <summary>
    /// 表达式树 创建动态查询的sql
    /// </summary>
    public class ExpressTreeTest
    {
        private IServiceCollection serviceDescriptors = new ServiceCollection();
        IUnitOfWork<MysqlDbContent> unit = null;
        public ExpressTreeTest()
        {
            //注入mysql仓储 
            serviceDescriptors.AddRepository().AddEFOption(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hks360;Charset=utf8;");

                //
                options.UseEntityFramework<MysqlDbContent>();
            });
            unit = serviceDescriptors.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
        }

        /// <summary>
        /// 测试排序的扩展
        /// </summary>
        [Fact]
        public void OrderBy()
        {
            //扩展排序
            var res = unit.Query<setting>().AsQueryable().OrderBy("Id", false).ThenBy("DuringTime", false).ToList();
            //自带的排序
            var res2 = unit.Query<setting>().AsQueryable().OrderByDescending<setting, object>(a => a.Id).ThenBy(a => a.Contact).ToList();
        }

        [Fact]
        public void GroupBy()
        {

            var res = unit.Query<setting>().AsQueryable().GroupBy(a => new { a.Contact, a.Description }).ToList();
        }

        [Fact]
        public void TestNew()
        {
            var str = new { };
            var str2 = Expression.New(str.GetType());
            var res = str2.ToString("C#");
        }

    }
}
