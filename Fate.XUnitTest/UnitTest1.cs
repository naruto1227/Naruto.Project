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
    }
}
