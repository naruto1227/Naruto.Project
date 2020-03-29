using System;
using Xunit;
using System.IO;
using Naruto.Infrastructure.NPOI;

namespace Naruto.XUnitTest
{
    public class ImportTest
    {
        [Fact]
        public void test() {
          var dt=  ImportHelper.ToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.xls"),"result",0);
        }
    }
}
