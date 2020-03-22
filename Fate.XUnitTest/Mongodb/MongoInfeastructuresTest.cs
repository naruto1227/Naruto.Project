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
    public class MongoInfeastructuresTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoInfeastructuresTest()
        {
            //services.AddMongoServices(options =>
            //{
            //    options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27021", DataBase = "test" });
            //    //options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27017,192.168.18.227:27018,192.168.18.227:27019,192.168.18.227:27020?readPreference=secondaryPreferred", ContextTypeName = "TestMongoContext", DataBase = "test" });
            //});
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }
        /// <summary>
        /// 创建文档
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateCollenction()
        {
            //创建文档并且固定文档的大小 当超出大小时，移除过时的文档
            mongoRepository.DataBase().CreateCollection("test22" + new Random().Next(1, 10000), new CreateCollectionOptions
            {
                Capped = true,
                MaxDocuments = 10,
                MaxSize = 1000
            });
            await mongoRepository.DataBase().CreateCollectionAsync("test111" + new Random().Next(1, 10000));
        }
        /// <summary>
        /// 删除文档
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DropCollenction()
        {
            mongoRepository.DataBase().DropCollection("test22");
            await mongoRepository.DataBase().DropCollectionAsync("test111");
        }
        [Fact]
        /// <summary>
        /// 重命名文档
        /// </summary>
        /// <returns></returns>
        public async Task RenameCollenction()
        {
            mongoRepository.DataBase().RenameCollection("test222108", "test222108_1");
            await mongoRepository.DataBase().RenameCollectionAsync("test1115839", "test1115839_1");
        }

        [Fact]
        public async Task RunCommand()
        {
            
        }
    }
}
