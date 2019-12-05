using Fate.Infrastructure.Mongo.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;

namespace Fate.XUnitTest
{
    public class MongoIndexTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoIndexTest()
        {
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27021", ContextTypeName = "TestMongoContext", DataBase = "test" });
                //options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27017,192.168.18.227:27018,192.168.18.227:27019,192.168.18.227:27020?readPreference=secondaryPreferred", ContextTypeName = "TestMongoContext", DataBase = "test" });
            });
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOneIndex()
        {
            mongoRepository.IndexInfrastructure<TestDTO>().CreateOne(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Ascending(a => a.Hobbit)));
            await mongoRepository.IndexInfrastructure<TestDTO>().CreateOneAsync(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Descending("Name")));
        }
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DropIndex()
        {
            mongoRepository.IndexInfrastructure<TestDTO>().DropOne("Name_-1");
            await mongoRepository.IndexInfrastructure<TestDTO>().DropOneAsync("Hobbit_1");
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateManyIndex()
        {
            var idnexs = new List<CreateIndexModel<TestDTO>>();

            idnexs.Add(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Ascending(a => a.Hobbit)));
            idnexs.Add(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Ascending(a => a.Name)));
            await mongoRepository.IndexInfrastructure<TestDTO>().CreateManyAsync(idnexs);
        }

        /// <summary>
        /// 删除所有索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DropAll()
        {
            await mongoRepository.IndexInfrastructure<TestDTO>().DropAllAsync();
        }
    }
}
