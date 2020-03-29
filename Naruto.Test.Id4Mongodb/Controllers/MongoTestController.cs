using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Naruto.Id4.Entities;
using Naruto.Id4.MongoDB;
using Naruto;
using Naruto.MongoDB.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Naruto.Test.Id4Mongodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoTestController : ControllerBase
    {
        private readonly IMongoRepository<IdentityServerMongoContext> mongoRepository;
        public MongoTestController(IMongoRepository<IdentityServerMongoContext> _mongoRepository)
        {
            mongoRepository = _mongoRepository;
        }

        [HttpGet]
        public async Task<string> Test()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await mongoRepository.Command<Client>().AddAsync(new Client
            {
                //d = SnowFlakeHelper.NewID()
            });
            await mongoRepository.ChangeDataBase("tests");
            var res = await mongoRepository.Query<Client>().ToListAsync(a => true);

            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds.ToString();
        }
        [RequestFormLimits(BufferBodyLengthLimit = long.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost]
        public async Task<string> Upload(IFormFile file)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await mongoRepository.GridFS().ChangeBucketNameAsync("testupload");
            await mongoRepository.GridFS().UploadFromStramAsync(file.FileName, file.OpenReadStream());
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }
    }
}