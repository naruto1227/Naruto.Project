using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Naruto.Redis;
using Naruto.Redis.IRedisManage;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Naruto.Domain.Model.Entities;
using System.Linq;
using System.IO;

namespace Naruto.XUnitTest
{
    public class RedisTest
    {
        IServiceCollection services = new ServiceCollection();

        private readonly IRedisOperationHelp redis;
        public RedisTest()
        {
            //注入redis仓储服务
            services.AddRedisRepository(options =>
            {
                options.Connection = new string[] { "127.0.0.1:6379" };
                options.RedisPrefix = new Redis.RedisConfig.RedisPrefixKey();
            });
            redis = services.BuildServiceProvider().GetService<IRedisOperationHelp>();
        }
        [Fact]
        public void test()
        {
            var redis = ConnectionMultiplexer.Connect("127.0.0.1");
            var res = redis.GetDatabase().StringIncrement("test", 0d);
            for (int i = 0; i < 10; i++)
            {
                res = redis.GetDatabase().StringIncrement("test");
            }
            var rr = redis.GetDatabase().StringGet("test");

            res = redis.GetDatabase().StringDecrement("test");
            for (int i = 0; i < 5; i++)
            {
                res = redis.GetDatabase().StringDecrement("test");
            }

            var redisbase = services.BuildServiceProvider().GetService<IRedisOperationHelp>();
            res = redisbase.RedisString().Increment("test");
            for (int i = 0; i < 10; i++)
            {
                res = redisbase.RedisString().Increment("test");
            }
            for (int i = 0; i < 10; i++)
            {
                res = redisbase.RedisString().Decrement("test");
            }
            Console.WriteLine("1");
        }
        [Fact]
        public async Task Publish()
        {
            var path = Path.GetTempPath();
            var redis = ConnectionMultiplexer.Connect("127.0.0.1");
            ISubscriber subscriber = redis.GetSubscriber();
            Parallel.For(0, 1000, async item =>
            {
                //发布
                await subscriber.PublishAsync("push", item.ToString());
            });

        }
        [Fact]
        public async Task RedisTest1()
        {

            ConcurrentQueue<setting> settings1 = new ConcurrentQueue<setting>();

            Parallel.For(0, 1000000, (item) =>
            {
                settings1.Enqueue(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            });

            await redis.RedisList().AddAsync<setting>("test", settings1.ToList());
        }

        [Fact]
        public async Task Pub()
        {

            await redis.RedisSubscribe().SubscribeAsync("test", (msg, value) =>
             {
                 Console.WriteLine(value);
             });
        }

        [Fact]
        public async Task StringTest()
        {
            for (int i = 0; i < 5; i++)
            {
                using (var servicesscope = services.BuildServiceProvider().CreateScope())
                {
                    var redis = servicesscope.ServiceProvider.GetRequiredService<IRedisOperationHelp>();
                    await redis.RedisString().AddAsync("1", "1");
                    await redis.RedisString().GetAsync("1");
                }
            }


        }
        [Fact]
        public void Store()
        {
            redis.RedisStore().Store(new setting() { Description = "1" });
            //List<setting> list = new List<setting>();
            //for (int i = 0; i < 100000; i++)
            //{
            //    list.Add(new setting() { Id = i });
            //}
            //redis.StoreAll(list);
            // redis.DeleteAll<setting>();
        }
        [Fact]
        public void Remove()
        {
            redis.RedisKey().Remove(new List<string>() { "test2", "zhang" });
        }
    }
}
