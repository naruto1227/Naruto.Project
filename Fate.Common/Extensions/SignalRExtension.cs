using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Linq;
using Fate.Common.Redis.RedisConfig;
namespace Fate.Common.Extensions
{

    public static class SignalRExtension
    {
        /// <summary>
        /// 添加一个redis的分布式
        /// </summary>
        /// <param name="builder"></param>
        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder)
        {
            builder.AddStackExchangeRedis(options =>
             {
                 options.Configuration.AllowAdmin = true;
                 options.Configuration.Password = RedisConnectionHelp.RedisPassword;
                 options.Configuration.DefaultDatabase = RedisConnectionHelp.RedisDefaultDataBase;
                 options.Configuration.ConnectTimeout = 300;
                 var redisConnectionConfigs = RedisConnectionHelp.RedisConnectionConfig.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                 if (redisConnectionConfigs == null && redisConnectionConfigs.Count() <= 0)
                 {
                     throw new ArgumentException("Redis配置错误");
                 }
                 redisConnectionConfigs.ToList().ForEach(a =>
                 {
                     options.Configuration.EndPoints.Add(a);
                 });
             });
            return builder;
        }
    }
}
