using System;
using Xunit;
using System.Collections.Generic;
using Fate.Domain.Model.Entities;
using System.Diagnostics;
using Fate.Infrastructure.AutofacDependencyInjection;
using Fate.Infrastructure.Repository;
using System.Data;
using Fate.Infrastructure.NPOI;
using System.IO;
using System.Linq;

namespace Fate.XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("test");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["test"] = 1;
                dt.Rows.Add(dr);
            }

            ExportHelper.ToExcel(dt, "result", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xlsx"), null, new Dictionary<string, string>
            {
                { "test","²âÊÔ"}
            });
        }

        /// <summary>
        /// list×ªdatatable
        /// </summary>
        [Fact]
        public void todatatable()
        {

            var list = new List<TestEntity>()
            {
                new TestEntity(){  Id=1, Name="1", Title="1" },
                 new TestEntity(){  Id=2, Name="2", Title="2" },
                  new TestEntity(){  Id=3, Name="3", Title="3" },
            };
            for (int i = 3; i <= 100000; i++)
            {
                list.Add(new TestEntity() { Id = i, Name = i.ToString(), Title = i.ToString() });
            }
           
            var res = list.ToDataTable();
        }
    }
}
