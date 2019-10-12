using System;
using Xunit;
using System.Collections.Generic;
using Fate.Domain.Model.Entities;
using System.Diagnostics;
using Fate.Common.AutofacDependencyInjection;
using Fate.Common.Repository;
using System.Data;
using Fate.Common.NPOI;
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
            dt.Columns.Add("列一");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["列一"] = 1;
                dt.Rows.Add(dr);
            }

            ExportHelper.ToExcel(dt, new string[] { "列一" }, "result", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls"));
        }

        /// <summary>
        /// list转datatable
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
