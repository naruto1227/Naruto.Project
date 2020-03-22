using Fate.Infrastructure.MongoDB.Interface;
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
           
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOneIndex()
        {
            mongoRepository.Index<TestDTO>().CreateOne(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Ascending(a => a.Hobbit)));
            await mongoRepository.Index<TestDTO>().CreateOneAsync(new CreateIndexModel<TestDTO>(Builders<TestDTO>.IndexKeys.Descending("Name")));
        }
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DropIndex()
        {
            mongoRepository.Index<TestDTO>().DropOne("Name_-1");
            await mongoRepository.Index<TestDTO>().DropOneAsync("Hobbit_1");
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
            await mongoRepository.Index<TestDTO>().CreateManyAsync(idnexs);
        }

        /// <summary>
        /// 删除所有索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DropAll()
        {
            await mongoRepository.Index<TestDTO>().DropAllAsync();
        }
    }
}
