using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Fate.Common.Redis.IRedisManage;
using Fate.Common.Redis.RedisManage;
using Fate.Common.Redis.RedisConfig;

namespace Fate.Common.Redis
{
    /// <summary>
    ///缓存的依赖注入
    /// </summary>
    public static class RedisDependencyExtension
    {
        /// <summary>
        /// 注入redis仓储
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddRedisRepository(this IServiceCollection server, Action<RedisOptions> options)
        {
            //注入服务
            server.AddSingleton(typeof(IRedisBase), typeof(RedisBase));
            server.AddSingleton(typeof(IRedisOperationHelp), typeof(RedisOperationHelp));
            server.AddSingleton<RedisConnectionHelp>();
            //配置参数
            server.Configure(options);
            return server;
        }
    }
}
