
using Fate.Infrastructure;
using Fate.Infrastructure.MongoDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fate.XUnitTest.Mongodb
{
    public class UserInfo : BsonValue
    {
        public string UserName { get; set; }

        public int Age { get; set; }

        public override BsonType BsonType => throw new NotImplementedException();

        public override int CompareTo(BsonValue other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public class MongoGridFSTest
    {
        private IServiceCollection services = new ServiceCollection();
        private readonly IMongoRepository<TestMongoContext> mongoRepository;
        public MongoGridFSTest()
        {
            services.AddMongoServices();
            services.AddMongoContext<TestMongoContext>(a =>
            {
                a.ConnectionString = "mongodb://192.168.0.104:27017"; a.DataBase = "test1";
            });
            services.AddMongoContext<Test2MongoContext>(a =>
            {
                a.ConnectionString = "mongodb://192.168.0.104:27017"; a.DataBase = "test2";
            });
            mongoRepository = services.BuildServiceProvider().GetRequiredService<IMongoRepository<TestMongoContext>>();
        }

        #region  上传
        /// <summary>
        /// 文件上传的测试
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadFromBytesAsync()
        {
            //获取需要上传的资源
            var bytes = await File.ReadAllBytesAsync(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"));
            var fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);
            await FindByIdAsync(fileId);
            FindById(fileId);
        }
        /// <summary>
        /// 文件上传的测试
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void UploadFromBytes()
        {
            //获取需要上传的资源
            var bytes = File.ReadAllBytes(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"));
            var fileId = mongoRepository.GridFS().UploadFromBytes(nameof(MongoGridFSTest) + ".cs", bytes);

        }
        /// <summary>
        /// 通过流上传
        /// </summary>
        [Fact]
        public void UploadFromStream()
        {
            using var fileStream = new FileStream(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"), FileMode.Open, FileAccess.Read);
            var fileId = mongoRepository.GridFS().UploadFromStream(nameof(MongoGridFSTest) + ".cs", fileStream, new Dictionary<string, object>()
            {
                {"userinfo",new UserInfo{
                     UserName="张三",
                      Age=18
                } }
            });
        }
        /// <summary>
        /// 通过流上传
        /// </summary>
        [Fact]
        public async Task UploadFromStreamAsync()
        {
            using var fileStream = new FileStream(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"), FileMode.Open, FileAccess.Read);
            var fileId = await mongoRepository.GridFS().UploadFromStramAsync(nameof(MongoGridFSTest) + ".cs", fileStream);
        }
        #endregion

        #region 下载

        /// <summary>
        /// 获取需要下载资源的二进制数据  通过名称
        /// </summary>
        [Fact]
        public void DownloadAsBytesByName()
        {
            var bytes = mongoRepository.GridFS().DownloadAsBytesByName("MongoGridFSTest.cs");

            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.cs"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 获取需要下载资源的二进制数据 通过名称
        /// </summary>
        [Fact]
        public async Task DownloadAsBytesByNameAsync()
        {
            var bytes = await mongoRepository.GridFS().DownloadAsBytesByNameAsync("MongoGridFSTest.cs");

            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.cs"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 获取需要下载资源的二进制数据  通过id
        /// </summary>
        [Fact]
        public void DownloadAsBytesById()
        {
            var bytes = mongoRepository.GridFS().DownloadAsBytesById("5e60b9969c4c0000df003bf4");

            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.cs"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 获取需要下载资源的二进制数据 通过id
        /// </summary>
        [Fact]
        public async Task DownloadAsBytesByIdAsync()
        {
            var bytes = await mongoRepository.GridFS().DownloadAsBytesByIdAsync("5e60b9969c4c0000df003bf4");

            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.cs"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 通过文件名将资源下载到对应的流
        /// </summary>
        [Fact]
        public void DownloadToStreamByName()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00002.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                mongoRepository.GridFS().DownloadToStreamByName("ATT00002.htm", fileStream);
            }
        }
        /// <summary>
        /// 通过文件名将资源下载到对应的流
        /// </summary>
        [Fact]
        public async Task DownloadToStreamByNameAsync()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00003.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                await mongoRepository.GridFS().DownloadToStreamByNameAsync("ATT00002.htm", fileStream);
            }
        }

        /// <summary>
        /// 通过文件id将资源下载到对应的流
        /// </summary>
        [Fact]
        public void DownloadToStreamById()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00002.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                mongoRepository.GridFS().DownloadToStreamById("5e60b9969c4c0000df003bf4", fileStream);
            }
        }
        /// <summary>
        /// 通过文件id将资源下载到对应的流
        /// </summary>
        [Fact]
        public async Task DownloadToStreamByIdAsync()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00002.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                await mongoRepository.GridFS().DownloadToStreamByIdAsync("5e60b9969c4c0000df003bf4", fileStream);
            }
        }
        /// <summary>
        /// 通过文件名获取流信息  并且写入指定的位置
        /// </summary>
        [Fact]
        public async Task GetDownloadStreamByNameAsync()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00004.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var gridfsStream = await mongoRepository.GridFS().GetDownloadStreamByNameAsync("ATT00002.htm");
                await gridfsStream.Stream.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// 通过文件名获取流信息  并且写入指定的位置
        /// </summary>
        [Fact]
        public void GetDownloadStreamByName()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00005.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var gridfsStream = mongoRepository.GridFS().GetDownloadStreamByName("ATT00002.htm");
                gridfsStream.Stream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// 通过id获取流信息  并且写入指定的位置
        /// </summary>
        [Fact]
        public void GetDownloadStreamById()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00006.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var gridfsStream = mongoRepository.GridFS().GetDownloadStreamById("5e60b9969c4c0000df003bf4");
                gridfsStream.Stream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// 通过id获取流信息  并且写入指定的位置
        /// </summary>
        [Fact]
        public async Task GetDownloadStreamByIdAsync()
        {
            using (FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ATT00007.htm"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var gridfsStream = await mongoRepository.GridFS().GetDownloadStreamByIdAsync("5e60b9969c4c0000df003bf4");
                await gridfsStream.Stream.CopyToAsync(fileStream);
            }
        }

        #endregion

        #region 删除
        /// <summary>
        /// 根据id删除
        /// </summary>

        [Fact]
        public void Delete()
        {
            mongoRepository.GridFS().DeleteById("5e6111369ccf5733046f512a");
        }

        /// <summary>
        /// 根据id删除
        /// </summary>

        [Fact]
        public async Task DeleteAsync()
        {
            await mongoRepository.GridFS().DeleteByIdAsync("5e611a28f2f45266dc48411f");
        }

        /// <summary>
        /// 删除所有的文件
        /// </summary>
        [Fact]
        public void Drop()
        {
            mongoRepository.GridFS().Drop();
        }


        /// <summary>
        /// 删除所有的文件
        /// </summary>
        [Fact]
        public async Task DropAsync()
        {
            await mongoRepository.GridFS().DropAsync();
        }

        #endregion

        #region 查询文件信息

        [Fact]
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        public void FindByIdTest()
        {
            FindById("5e6111369ccf5733046f512a");
        }

        [Fact]
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        public async Task FindByIdAsyncTest()
        {
            await FindByIdAsync("5e6111369ccf5733046f512a");
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        public void FindById(string id)
        {
            var info = mongoRepository.GridFS().FindById(id);
        }

        public async Task FindByIdAsync(string id)
        {
            var info = await mongoRepository.GridFS().FindByIdAsync(id);
        }

        [Fact]
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="id"></param>
        public void FindByName()
        {
            var info = mongoRepository.GridFS().FindByName("dFST", true);
        }
        [Fact]
        /// <summary>
        /// 根据名称查询
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task FindByNameAsync()
        {
            var info = await mongoRepository.GridFS().FindByNameAsync("dFST", false);
        }

        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        [Fact]
        public void Exists()
        {
            var res = mongoRepository.GridFS().Exists("5e6111369ccf5733046f512a");
        }


        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        [Fact]
        public async Task ExistsAsync()
        {
            var res = await mongoRepository.GridFS().ExistsAsync("5e611a28f2f45266dc48411f");
        }


        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        [Fact]
        public async Task ExistsByNameAsync()
        {
            var res = await mongoRepository.GridFS().ExistsByNameAsync("dFST");
        }

        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        [Fact]
        public void ExistsByName()
        {
            var res = mongoRepository.GridFS().ExistsByName("MongoGridFSTest.cs");
        }
        #endregion

        #region changge


        [Fact]
        public async Task ChangeDatabase()
        {
            //获取需要上传的资源
            var bytes = await File.ReadAllBytesAsync(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"));
            var fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);

            await mongoRepository.ChangeDataBase("gridfs");
            fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);

            await FindByIdAsync(fileId);
            FindById(fileId);
        }

        [Fact]
        public async Task ChangeBuckName()
        {
            //获取需要上传的资源
            var bytes = await File.ReadAllBytesAsync(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"));
            await mongoRepository.ChangeDataBase("gridfs");
            await mongoRepository.GridFS().ChangeBucketNameAsync("gridfs");
            var fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);


            fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);

            await FindByIdAsync(fileId);
            FindById(fileId);
        }

        [Fact]
        public async Task Change()
        {
            //获取需要上传的资源
            var bytes = await File.ReadAllBytesAsync(Path.Combine(@"D:\自有项目\Fate.DDD\Fate.XUnitTest", "Mongodb", "MongoCommandTest.cs"));
            var fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);

            await mongoRepository.GridFS().ChangeBucketNameAsync("gridfs");
            fileId = await mongoRepository.GridFS().UploadFromBytesAsync(nameof(MongoGridFSTest) + ".cs", bytes);

            await FindByIdAsync(fileId);
            FindById(fileId);
        }
        #endregion

        [Fact]
        public void TestObjectId()
        {
            MongoDB.Bson.ObjectId.Parse("123456789012345678901234");
            var o = MongoDB.Bson.ObjectId.GenerateNewId();
            var o2 = MongoDB.Bson.ObjectId.Empty;
        }
    }
}
