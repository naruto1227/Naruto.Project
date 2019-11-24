
//using StackExchange.Redis;
//using System;
//using System.Linq;
//using Fate.Infrastructure.Redis.RedisConfig;

//namespace Microsoft.Extensions.DependencyInjection
//{
//    /// <summary>
//    /// 分布式缓存的扩展
//    /// </summary>
//    public static class RedisCacheExtension
//    {
//        public static IServiceCollection AddDistributedRedisCache(this IServiceCollection service)
//        {
//            return service.AddStackExchangeRedisCache(options =>
//             {
//                 //获取配置信息
//                 var redisConnectionHelp = service.BuildServiceProvider().GetService<IRedisConnectionHelp>();
//                 options.ConfigurationOptions = new ConfigurationOptions();
//                 options.ConfigurationOptions.AllowAdmin = true;
//                 options.ConfigurationOptions.Password = redisConnectionHelp.RedisPassword;
//                 options.ConfigurationOptions.DefaultDatabase = redisConnectionHelp.RedisDefaultDataBase;
//                 options.ConfigurationOptions.ConnectTimeout = 300;
//                 var redisConnectionConfigs = redisConnectionHelp.RedisConnectionConfig.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
//                 if (redisConnectionConfigs == null && redisConnectionConfigs.Count() <= 0)
//                 {
//                     throw new ArgumentException("Redis配置错误");
//                 }
//                 redisConnectionConfigs.ToList().ForEach(a =>
//                 {
//                     options.ConfigurationOptions.EndPoints.Add(a);
//                 });
//             });
//        }
//    }
//}
