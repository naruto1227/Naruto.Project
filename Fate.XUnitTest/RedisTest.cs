using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fate.XUnitTest
{
   public class RedisTest
    {
        [Fact]
        public void test()
        {
            var redis = ConnectionMultiplexer.Connect("127.0.0.1");
            ISubscriber subscriber = redis.GetSubscriber();
            //订阅
            subscriber.Subscribe("push", (chanel, msg) => {
                Console.WriteLine(chanel);
                Console.WriteLine(msg);
            });
            //发布
            subscriber.Publish("push", "你好");
            Console.WriteLine("1");
        }
    }
}
