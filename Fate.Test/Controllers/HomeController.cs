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

namespace Fate.Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class Home1Controller : ControllerBase
    {
        MyJsonResult jsonResult;
        SettingApp setting;
        private IUnitOfWork<MysqlDbContent> unitOfWork;
        private IRedisOperationHelp redis;

        private readonly IServiceProvider serviceProvider;
        RSAHelper rSA;
        public Home1Controller(SettingApp _setting, IUnitOfWork<MysqlDbContent> _unitOfWork, IRedisOperationHelp _redis, MyJsonResult myJson, RSAHelper _rSA, IServiceProvider _serviceProvider)
        {
            setting = _setting;
            unitOfWork = _unitOfWork;
            redis = _redis;
            jsonResult = myJson;
            rSA = _rSA;
            serviceProvider = _serviceProvider;
        }
        [HttpGet]
        public async Task test()
        {
            var redis1 = serviceProvider.GetRequiredService<IRedisOperationHelp>();
            redis1.RedisString().Add("zhang", "haibo");
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var redis2 = serviceScope.ServiceProvider.GetRequiredService<IRedisOperationHelp>();
                redis2.RedisString().Add("zhang", "haibo");

            }
            jsonResult.msg = "helloword";
            await setting.add(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
            //throw new Fate.Infrastructure.Exceptions.NoAuthorizationException("111111111111111");
        }

        [HttpGet]
        public async Task tran()
        {
            unitOfWork.BeginTransaction();
            //await unitOfWork.Respositiy<setting>().AsQueryable().ToListAsync();
            await unitOfWork.Command<test1>().AddAsync(new test1() { Id = Convert.ToInt32(DateTime.Now.ToString("ffffff")) });
            await unitOfWork.SaveChangeAsync();
            unitOfWork.CommitTransaction();

        }

        [HttpGet]
        public async Task tran2()
        {
            //unitOfWork.BeginTransaction();
            await unitOfWork.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await unitOfWork.Command<test1>().AddAsync(new test1() { Id = Convert.ToInt32(DateTime.Now.ToString("ffffff")) });
            await unitOfWork.SaveChangeAsync();
            var str = await unitOfWork.Query<test1>().AsQueryable().AsNoTracking().ToListAsync();
            await unitOfWork.Command<test1>().AddAsync(new test1() { Id = Convert.ToInt32(DateTime.Now.ToString("ffffff")) });
            await unitOfWork.SaveChangeAsync();
            //unitOfWork.RollBackTransaction();
        }
        public async Task tran3()
        {

            unitOfWork.BeginTransaction();

            await unitOfWork.Query<setting>().AsQueryable().ToListAsync();

            var str = await unitOfWork.Query<setting>().AsQueryable().ToListAsync();
            unitOfWork.RollBackTransaction();
        }
        public async Task test5()
        {
            await unitOfWork.ChangeDataBase("test2");
            var str = await unitOfWork.Query<test1>().AsQueryable().ToListAsync();
        }
        public async Task testredis2()
        {
            await redis.RedisList().RightPushAsync("1", new Random().Next(1000, 9999).ToString());
        }

        [HttpGet]
        public async Task test33()
        {
            //List<setting> list = Common.Ioc.Core.AutofacDI.Resolve<List<setting>>();
            //List<string> li = Common.Ioc.Core.AutofacDI.Resolve<List<string>>();
            //if (list.Count() > 0)
            //    list.Remove(list[0]);
            //await setting.add(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });


        }

        /// <summary>
        /// 测试事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task EeventTest()
        {
            await setting.EventTest();
        }
        /// <summary>
        /// 测试redis
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task RedisTest()
        {
            await redis.RedisString().AddAsync("1", new Random().Next(1000, 9999).ToString());
        }

        public async Task testEF()
        {
            await setting.testEF();
        }
        [Authorize]
        [HttpGet]
        public IActionResult addmvc()
        {
            return new JsonResult("111111");
        }

        public void test22()
        {
            rSA.CreateRSACacheAsync();
        }


        public string subTest()
        {
            Action<RedisChannel, RedisValue> handler = (channel, message) =>
            {
                Console.WriteLine(channel);
                Console.WriteLine(message);
            };
            redis.RedisSubscribe().Subscribe("push", handler);

            ////发布
            //redis.Publish("push", "你好");
            return "1";
        }

        public void testConfig()
        {
            ConfigurationManage.GetValue("RedisConfig:Connection");
        }
    }
}