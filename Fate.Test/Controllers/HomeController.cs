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

namespace Fate.Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class Home1Controller : ControllerBase
    {
        MyJsonResult jsonResult;
        ISettingApp setting;
        private IUnitOfWork unitOfWork;
        private IRedisOperationHelp redis;
        RSAHelper rSA;
        IRepository<setting> repository;
        public Home1Controller(ISettingApp _setting, IUnitOfWork _unitOfWork, IRedisOperationHelp _redis, MyJsonResult myJson, RSAHelper _rSA, IRepository<setting> _repository)
        {
            setting = _setting;
            unitOfWork = _unitOfWork;
            redis = _redis;
            jsonResult = myJson;
            rSA = _rSA;
            repository = _repository;
        }
        [HttpGet]
        public async Task test()
        {
          await  repository.AddAsync(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
            redis.StringSet("zhang", "haibo");
            await setting.add(new setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
            jsonResult.msg = "helloword";
            throw new Fate.Common.Exceptions.NoAuthorizationException("111111111111111");
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
            await Fate.Common.Redis.RedisConfig.RedisConnectionHelp.RedisConnection.GetDatabase().HashSetAsync("1", "1", "1");
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
            var str =AutofacInit.Resolve<setting>();
        }
    }
}