using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.Application.Services;
using Fate.Infrastructure.Redis.IRedisManage;
using Microsoft.AspNetCore.Mvc;

namespace Fate.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public SettingApp settingApp { get; set; }

        public IRedisOperationHelp Redis { get; set; }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Redis.RedisString().Add(Guid.NewGuid().ToString(), "1");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
