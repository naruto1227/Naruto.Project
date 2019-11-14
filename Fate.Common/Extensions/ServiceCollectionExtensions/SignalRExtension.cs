//using Microsoft.AspNetCore.SignalR;

//using System;

//using System.Linq;
//using Fate.Common.Redis.RedisConfig;
//namespace Microsoft.Extensions.DependencyInjection
//{

//    public static class SignalRExtension
//    {
//        /// <summary>
//        /// 添加一个redis的分布式
//        /// </summary>
//        /// <param name="builder"></param>
//        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder)
//        {
//            builder.AddStackExchangeRedis(options =>
//             {
//                 //获取配置信息
//                 var redisConnectionHelp = builder.Services.BuildServiceProvider().GetService<IRedisConnectionHelp>();
//                 options.Configuration.AllowAdmin = true;
//                 options.Configuration.Password = redisConnectionHelp.RedisPassword;
//                 options.Configuration.DefaultDatabase = redisConnectionHelp.RedisDefaultDataBase;
//                 options.Configuration.ConnectTimeout = 300;
//                 var redisConnectionConfigs = redisConnectionHelp.RedisConnectionConfig.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
//                 if (redisConnectionConfigs == null && redisConnectionConfigs.Count() <= 0)
//                 {
//                     throw new ArgumentException("Redis配置错误");
//                 }
//                 redisConnectionConfigs.ToList().ForEach(a =>
//                 {
//                     options.Configuration.EndPoints.Add(a);
//                 });
//             });
//            return builder;
//        }
//    }
//}
