
using System;
using Naruto.Redis.RedisConfig;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis缓存的依赖注入
    /// </summary>
    public static class RedisDependencyExtension
    {

        /// <summary>
        /// 注入redis服务
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal static IServiceCollection AddRedisService(this IServiceCollection server)
        {
            //获取当前层的所有的类型
            var types = Assembly.Load(Assembly.GetAssembly(typeof(RedisDependencyExtension)).GetName()).GetTypes();
            //获取需要通过接口的实现来依赖注入的类型
            var redisDependencyTypes = types.Where(a => a.GetInterface("IRedisDependency") != null);
            if (redisDependencyTypes != null && redisDependencyTypes.Count() > 0)
            {
                //获取接口类型
                redisDependencyTypes.Where(a => a.IsInterface).ToList().ForEach(item =>
                {
                    //获取当前接口对应的 实现类
                    var classType = redisDependencyTypes.Where(a => a.IsClass && a.GetInterface(item.Name) != null).FirstOrDefault();
                    if (classType != null)
                    {
                        server.AddSingleton(item, classType);
                    }
                });
            }
            return server;
        }

        /// <summary>
        /// 注入redis仓储
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddRedisRepository(this IServiceCollection server, Action<RedisOptions> options)
        {
            //注入服务
            server.AddRedisService();
            //配置参数
            server.Configure(options);
            return server;
        }

        /// <summary>
        /// 注入redis仓储
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddRedisRepository(this IServiceCollection server, IConfiguration config)
        {
            //注入服务
            server.AddRedisService();
            //配置参数
            server.Configure<RedisOptions>(config);
            return server;
        }
    }
}
