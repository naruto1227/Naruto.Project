using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Fate.Common.Repository.Mysql;
using Fate.Application.Interface;
using Fate.Common.Repository.Mysql.UnitOfWork;

namespace Fate.WebApi.Controllers
{
    public class HomeController : BaseController
    {
        ISettingApp setting;

        public HomeController(ISettingApp _setting)
        {
            setting = _setting;
         
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            return await Json("welcome come");
        }

        [HttpGet]
        public async Task test()
        {
            await setting.add(new Domain.Model.Entities.setting() { Contact = "111sdsd", DuringTime = "1", Description = "1", Integral = 1, Rule = "1" });
        }
    }
}