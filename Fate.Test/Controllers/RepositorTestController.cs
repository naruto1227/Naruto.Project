using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fate.Common.Ioc.Core;
using Fate.Domain.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fate.Common.Repository.Mysql.UnitOfWork;
using System.Data;
using Fate.Common.Extensions;
namespace Fate.Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RepositorTestController : ControllerBase
    {
        IUnitOfWork unit;
        public RepositorTestController(IUnitOfWork ofWork)
        {
            unit = ofWork;
        }
        public IActionResult test()
        {
            List<setting> settings = new List<setting>();
            for (int i = 0; i < 1000; i++)
            {
                settings.Add(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            unit.Respositiy<setting>().BulkAddAsync(settings);
            unit.SaveChanges();
            stopwatch.Stop();
            return new JsonResult(new { stopwatch.ElapsedMilliseconds });
        }

        public async Task<IActionResult> testbulk()
        {
            DataTable dt = new DataTable
            {
                TableName = "setting"
            };
            dt.Columns.Add("Contact");
            dt.Columns.Add("Description");
            dt.Columns.Add("DuringTime");
            dt.Columns.Add("Integral");
            dt.Columns.Add("Rule");
            for (int i = 0; i < 100000; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Contact"] = "1";
                dr["Description"] = "1";
                dr["DuringTime"] = "1";
                dr["Integral"] = "1";
                dr["Rule"] = "1";
                dt.Rows.Add(dr);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            await dt.BulkLoadAsync();
            stopwatch.Stop();
            return new JsonResult(new { stopwatch.ElapsedMilliseconds });
        }
    }
}