using Microsoft.Extensions.DependencyInjection;
using System;
using Fate.Common.Redis.RedisConfig;
using System.Reflection;
using System.Linq;

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
            //获取当前层的所有的类型
            var types = Assembly.Load(Assembly.GetAssembly(typeof(RedisDependencyExtension)).GetName()).GetTypes();
            //获取需要通过接口的实现来依赖注入的类型
            var iTypes = types.Where(a => a.GetInterface("IRedisDependency") != null);
            if (iTypes != null && iTypes.Count() > 0)
            {
                iTypes.Where(a=>a.IsClass).ToList().ForEach(item =>
                {
                    var interfaceClassName = "I" + item.Name;
                    //获取接口的类型
                    var interfaceClass = iTypes.Where(a => a.Name == interfaceClassName).FirstOrDefault();
                    if (interfaceClass != null)
                    {
                        server.AddSingleton(interfaceClass, item);
                    }
                });
            }
            //注册类的实例
            types.Where(a => a.GetInterface("IRedisClassDependency") != null).ToList().ForEach(item =>
            {
                server.AddSingleton(item);
            });

            //配置参数
            server.Configure(options);
            return server;
        }
    }
}
