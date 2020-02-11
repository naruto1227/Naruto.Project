using Fate.Infrastructure.MongoDB.Interface;
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
    public partial class MongoCommandTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoCommandTest()
        {
            services.AddMongoServices(options =>
            {
                options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.0.107:27017", ContextTypeName = "TestMongoContext", DataBase = "test" });
                //options.Add(new TestMongoContext() { ConnectionString = "mongodb://192.168.18.227:27017,192.168.18.227:27018,192.168.18.227:27019,192.168.18.227:27020?readPreference=secondaryPreferred", ContextTypeName = "TestMongoContext", DataBase = "test" });
            });
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }
        [Fact]
        public async Task BsonDocument()
        {
            await mongoRepository.Command<BsonDocument>().AddAsync(new BsonDocument
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
        public async Task Insert2()
        {
            await mongoRepository.Command<Test3DTO>().AddAsync("test44", new Test3DTO()
            {
                testkey = Guid.NewGuid().ToString(),
                testDTO2 = new TestDTO2()
                {
                    Name = "长哈"
                }
            });
            var res = await mongoRepository.Query<Test3DTO>().AsQueryable("test44").ToListAsync();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Insert()
        {
            Parallel.For(0, 30000, async item =>
             {
                 // await mongoRepository.ChangeDataBase("test2");
                 mongoRepository.Command<TestDTO>().Add(new TestDTO()
                 {
                     Name = "王五"
                 });

                 await mongoRepository.Command<TestDTO>().AddAsync(new TestDTO()
                 {
                     Name = "王五"
                 });
                 var list = await mongoRepository.Query<TestDTO>().ToListAsync(a => a.Name == "王五");

                 mongoRepository.Command<TestDTO>().BulkAdd(new List<TestDTO>()
             {
               new TestDTO(){  Name="王五"},
               new TestDTO(){  Name="王五", Age=11}
             });
                 await mongoRepository.Command<TestDTO>().BulkAddAsync(new List<TestDTO>()
             {
               new TestDTO(){  Name="王五"},
               new TestDTO(){  Name="王五", Age=11}
             });
                 list = await mongoRepository.Query<TestDTO>().ToListAsync(Builders<TestDTO>.Filter.Eq(a => a.Name, "王五"));
             });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Fact]

        public async Task Delete()
        {
            mongoRepository.Command<TestDTO>().Delete(a => a.Age == null);

            await mongoRepository.Command<TestDTO>().DeleteAsync(a => a.Age == null);

            var list = await mongoRepository.Query<TestDTO>().ToListAsync(a => true);

            mongoRepository.Command<TestDTO>().BulkDelete(a => a.Age == null);

            await mongoRepository.Command<TestDTO>().BulkDeleteAsync(a => a.Age == 11);

            list = await mongoRepository.Query<TestDTO>().ToListAsync(a => true);
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

            await mongoRepository.Command<TestDTO>().UpdateAsync(a => a.Name == "张三", updateField);
        }
    }

    public partial class MongoCommandTest
    {
        [Fact]
        public async Task TestIdentity()
        {
            await mongoRepository.Command<Microsoft.AspNetCore.Identity.IdentityUser>().AddAsync(new Microsoft.AspNetCore.Identity.IdentityUser()
            {
                UserName = "1111"
            });
            var res = await mongoRepository.Query<Microsoft.AspNetCore.Identity.IdentityUser>().AsQueryable().ToListAsync();
        }
    }
}
