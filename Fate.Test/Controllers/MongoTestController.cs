using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Domain.Interface;
using Fate.Application.Interface;
using Fate.Infrastructure.Repository.UnitOfWork;
using Fate.Infrastructure.Redis.IRedisManage;
using Microsoft.AspNetCore.Authorization;
using Fate.Domain.Model.Entities;
using Fate.Infrastructure.AutofacDependencyInjection;
using Fate.Infrastructure.Infrastructure;
using Fate.Infrastructure.Repository;
using StackExchange.Redis;
using Fate.Application.Services;
using Microsoft.EntityFrameworkCore;
using Fate.Domain.Model;
using Fate.Infrastructure.Email;
using Fate.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Test.TestClass;

namespace Fate.Test
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MongoTestController : ControllerBase
    {
        private readonly IMongoRepository<TestMongo> mongoRepository;
        public MongoTestController(IMongoRepository<TestMongo> _mongoRepository)
        {
            mongoRepository = _mongoRepository;
        }
        [HttpGet]
        public async Task insert()
        {
            var list = new List<TestDTO>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestDTO()
                {
                    Name = "张三" + i
                });
            }
            await mongoRepository.Command<TestDTO>().InsertManyAsync(list);
        }
    }
}
