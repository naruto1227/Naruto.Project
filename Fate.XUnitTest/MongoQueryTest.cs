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
using MongoDB.Driver.Linq;
using System.Linq;

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

        public int? Age { get; set; }

        public int Hobbit { get; set; }
    }

    public class TestDTO2 : IMongoEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
    public class MongoQueryTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoQueryTest()
        {
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27017", ContextTypeName = "TestMongoContext", DataBase = "test" });
            });
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
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
            var res = await repository.Query<TestDTO>().CountAsync("test2", a => 1 == 1);
            var res2 = await repository.Query<TestDTO2>().CountAsync("test2", a => 1 == 1);
            res = await repository.Query<TestDTO>().CountAsync("test2", a => 1 == 1);
            res2 = await repository.Query<TestDTO2>().CountAsync("test2", a => 1 == 1);
        }

        [Fact]
        public async Task AsQueryable()
        {
            var list = await mongoRepository.Query<TestDTO>().AsQueryable("test2").ToListAsync();
            list = await mongoRepository.Query<TestDTO>().AsQueryable("test2").Where(a => a.Age == 1).ToListAsync();
        }

        [Fact]
        public async Task Find()
        {
            var res = await mongoRepository.Query<TestDTO>().FindAsync("test2", a => true);
            res = mongoRepository.Query<TestDTO>().Find("test2", a => true);
        }


        [Fact]
        public async Task FindByPage()
        {
            var res = await mongoRepository.Query<TestDTO>().FindByPageAsync(a => true, 1, 1, new FindOptions<TestDTO>()
            {
                Sort = Builders<TestDTO>.Sort.Ascending("Age")
            });
            res = mongoRepository.Query<TestDTO>().FindByPage("test2", a => true, 1, 1);
        }

        [Fact]
        public async Task FirstOrDefaultAsync()
        {
            var res = await mongoRepository.Query<TestDTO>().FirstOrDefaultAsync(a => a.Name == "张三");
            res = mongoRepository.Query<TestDTO>().FirstOrDefault("test2", a => true);

            res = await mongoRepository.Query<TestDTO>().FirstOrDefaultAsync(Builders<TestDTO>.Filter.Eq("Name", "李四"));

            res = await mongoRepository.Query<TestDTO>().FirstOrDefaultAsync(Builders<TestDTO>.Filter.And(Builders<TestDTO>.Filter.Eq("Name", "李四"), Builders<TestDTO>.Filter.Eq(a => a.Age, 2)));
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
