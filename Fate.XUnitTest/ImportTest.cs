using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Fate.Common.NPOI;
using System.IO;

namespace Fate.XUnitTest
{
   public class ImportTest
    {
        [Fact]
        public void test() {
          var dt=  ImportHelper.ToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls"),"result",0);
        }
    }
}
