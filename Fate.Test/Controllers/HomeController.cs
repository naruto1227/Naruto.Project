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

namespace Fate.Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class Home1Controller : ControllerBase
    {
        Fate.Common.Infrastructure.MyJsonResult jsonResult = new Common.Infrastructure.MyJsonResult();
        ISettingApp setting;
        private IUnitOfWork unitOfWork;
        private IRedisOperationHelp redis;
        public Home1Controller(ISettingApp _setting, IUnitOfWork _unitOfWork, IRedisOperationHelp _redis)
        {

            setting = _setting;
            unitOfWork = _unitOfWork;
            redis = _redis;
        }
        [HttpGet]
        public async Task test()
        {
            redis.StringSet("zhang", "haibo");
            await setting.add(new Domain.Model.Entities.setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
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
        public async Task<IActionResult> addmvc()
        {
            return new JsonResult("111111");
        }

        public void test22() {
            Fate.Common.NLog.NLogHelper.Default.Info("11");
            Fate.Common.NLog.NLogHelper.Default.Error("11");
        }
    }
}