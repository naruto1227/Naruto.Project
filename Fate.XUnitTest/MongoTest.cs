using Fate.Infrastructure.BaseMongo.Model;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fate.XUnitTest
{
    public class TestMongoContext : MongoContext
    {
    }

    public class Test2MongoContext : MongoContext
    {
    }
    public class TestDTO : IMongoEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
    public class MongoTest
    {
        private IServiceCollection services = new ServiceCollection();

        public MongoTest()
        {
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.1.6:27017", ContextTypeName = "TestMongoContext", DataBase = "test" });
            });
        }

        [Fact]
        public void GetServices()
        {
            var repository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }

        [Fact]
        public async Task CountAsync()
        {
            var repository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
            var res = await repository.Query<TestDTO>().CountAsync(a => 1 == 1);
        }
        [Fact]
        public void Test()
        {
            services.Configure<List<MongoContext>>(a =>
            {
                a.Add(new TestMongoContext() { ConnectionString = "123", ContextTypeName = typeof(TestMongoContext).Name });
                a.Add(new Test2MongoContext() { ConnectionString = "123222", ContextTypeName = typeof(Test2MongoContext).Name });
            });
            services.AddSingleton(new TestMongoContext() { ConnectionString = "1212", ContextTypeName = typeof(TestMongoContext).Name });

            var res = services.BuildServiceProvider().GetRequiredService<IOptions<List<MongoContext>>>();
            var res2 = services.BuildServiceProvider().GetRequiredService<TestMongoContext>();
        }
    }
}
