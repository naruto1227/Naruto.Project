using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Domain.Interface;
using Fate.Application.Interface;
using Fate.Common.Repository.Mysql.UnitOfWork;
using Fate.Common.Redis.IRedisManage;
using Microsoft.AspNetCore.Authorization;
using Fate.Domain.Model.Entities;
using Fate.Common.Ioc.Core;
using Fate.Common.Infrastructure;
using Fate.Common.Repository.Mysql;
using StackExchange.Redis;
using Fate.Application.Services;
using Microsoft.EntityFrameworkCore;

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
        RSAHelper rSA;
        public Home1Controller(SettingApp _setting, IUnitOfWork<MysqlDbContent> _unitOfWork, IRedisOperationHelp _redis, MyJsonResult myJson, RSAHelper _rSA)
        {
            setting = _setting;
            unitOfWork = _unitOfWork;
            redis = _redis;
            jsonResult = myJson;
            rSA = _rSA;
        }
        [HttpGet]
        public async Task test()
        {

            redis.StringSet("zhang", "haibo");
            await setting.add(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
            jsonResult.msg = "helloword";
            //throw new Fate.Common.Exceptions.NoAuthorizationException("111111111111111");
        }

        [HttpGet]
        public async Task tran()
        {
           unitOfWork.BeginTransaction();
            //await unitOfWork.Respositiy<setting>().AsQueryable().ToListAsync();
            await unitOfWork.Respositiy<test1>().AddAsync(new test1() { Id = Convert.ToInt32(DateTime.Now.ToString("ffffff")) });
            await unitOfWork.SaveChangeAsync();
            unitOfWork.CommitTransaction();

        }

        public async Task testredis2()
        {
            await redis.ListRightPushAsync("1", new Random().Next(1000, 9999).ToString());
        }

        [HttpGet]
        public async Task test33()
        {
            List<setting> list = Common.Ioc.Core.AutofacInit.Resolve<List<setting>>();
            List<string> li = Common.Ioc.Core.AutofacInit.Resolve<List<string>>();
            if (list.Count() > 0)
                list.Remove(list[0]);
            await setting.add(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
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
            rSA.CreateRSACache();
        }

        public async Task sendEmail()
        {
            Common.MailKit.EmailKit emailKi = new Common.MailKit.EmailKit();
            await emailKi.SendEmailAsync("1635783721@qq.com", "测试", "", "<a src='www.baidu.com'>点击</a>", "D:\\360极速浏览器下载\\AdminLTE-2.4.5.zip");
        }

        public void model()
        {
            var str = AutofacInit.Resolve<setting>();
        }

        public string subTest()
        {
            Action<RedisChannel, RedisValue> handler = (channel, message) =>
            {
                Console.WriteLine(channel);
                Console.WriteLine(message);
            };
            redis.Subscribe("push", handler);

            ////发布
            redis.Publish("push", "你好");
            return "1";
        }
        public void testlog()
        {

            var log = AutofacInit.Resolve<Fate.Common.NLog.NLogHelper>();
            log.Info("1");
        }

        public void testConfig()
        {
            ConfigurationManage.GetValue("RedisConfig:Connection");
        }
    }
}