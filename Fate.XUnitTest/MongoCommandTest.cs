using Fate.Infrastructure.Mongo.Interface;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Archives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fate.XUnitTest
{
    public class MongoCommandTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoCommandTest()
        {
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27017,192.168.18.227:27018,192.168.18.227:27019,192.168.18.227:27020?readPreference=secondaryPreferred", ContextTypeName = "TestMongoContext", DataBase = "test" });
            });
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }
        [Fact]
        public async Task BsonDocument()
        {
            await mongoRepository.Command<BsonDocument>().InsertOneAsync(new BsonDocument
            {
                { "name","hai"},
                { "age","nian"}
            });
        }
        [Fact]
        public async Task BulkWrite()
        {
            for (int i = 0; i < 10; i++)
            {
                ConcurrentBag<InsertOneModel<TestDTO>> list = new ConcurrentBag<InsertOneModel<TestDTO>>();

                Parallel.For(0, 1000000, item =>
                {
                    list.Add(new InsertOneModel<TestDTO>(new TestDTO()
                    {
                        Hobbit = item,
                        Name = "张三" + item
                    }));
                });
                await mongoRepository.Command<TestDTO>().BulkWriteAsync(list);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Insert()
        {
            // await mongoRepository.ChangeDataBase("test2");
            mongoRepository.Command<TestDTO>().InsertOne(new TestDTO()
            {
                Name = "王五"
            });

            await mongoRepository.Command<TestDTO>().InsertOneAsync(new TestDTO()
            {
                Name = "王五"
            });
            var list = await mongoRepository.Query<TestDTO>().FindAsync(a => a.Name == "王五");

            mongoRepository.Command<TestDTO>().InsertMany(new List<TestDTO>()
            {
               new TestDTO(){  Name="王五"},
               new TestDTO(){  Name="王五", Age=11}
            });
            await mongoRepository.Command<TestDTO>().InsertManyAsync(new List<TestDTO>()
            {
               new TestDTO(){  Name="王五"},
               new TestDTO(){  Name="王五", Age=11}
            });
            list = await mongoRepository.Query<TestDTO>().FindAsync(Builders<TestDTO>.Filter.Eq(a => a.Name, "王五"));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Fact]

        public async Task Delete()
        {
            mongoRepository.Command<TestDTO>().DeleteOne(a => a.Age == null);

            await mongoRepository.Command<TestDTO>().DeleteOneAsync(a => a.Age == null);

            var list = await mongoRepository.Query<TestDTO>().FindAsync(a => true);

            mongoRepository.Command<TestDTO>().DeleteMany(a => a.Age == null);

            await mongoRepository.Command<TestDTO>().DeleteManyAsync(a => a.Age == 11);

            list = await mongoRepository.Query<TestDTO>().FindAsync(a => true);
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Replace()
        {
            var info = await mongoRepository.Query<TestDTO>().FirstOrDefaultAsync(a => a.Name == "王二麻子2");
            info.Name = "王二麻子2111";
            info.Hobbit = 11121111;

            //mongoRepository.Command<TestDTO>().ReplaceOne(a => a.Name == "张三", info);

            //await mongoRepository.Command<TestDTO>().ReplaceOneAsync(a => a.Name == "王二麻子", info);

            var oldInfo = mongoRepository.Command<TestDTO>().FindOneAndReplace(a => a.Name == "王二麻子2", info);
        }
        /// <summary>
        /// 修改测试
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Update()
        {
            var updateField = new Dictionary<string, object>()
            {
                { "Name","张三张三"},
                { "Age",1221}
            };
            // mongoRepository.Command<TestDTO>().UpdateOne(a => a.Name == "李四", updateField);

            await mongoRepository.Command<TestDTO>().UpdateOneAsync(a => a.Name == "张三", updateField);
        }
    }
}
