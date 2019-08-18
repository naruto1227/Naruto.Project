using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Fate.XUnitTest
{
    public class TestLambda
    {

        [Fact]
        public void test()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<TestMethod>();
            Dictionary<string, Action<string>> keyValuePairs = new Dictionary<string, Action<string>>();

            var types = typeof(TestMethod);
            var str = types.GetConstructors()[0];
            var s = str.Invoke(null) as TestMethod;
            keyValuePairs.Add("test", s.test1);
            keyValuePairs["test"]("1");
        }


    }
}
