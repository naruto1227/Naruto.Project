using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fate.Common.Repository.Mysql.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace Fate.Common.Repository.Mysql
{

    public static class RepositoryExtensions
    {
        /// <summary>
        /// 注入mysql仓储服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMysqlRepositoryServer<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            //获取当前层的所有的类型
            var types = Assembly.Load(Assembly.GetAssembly(typeof(RepositoryExtensions)).GetName()).GetTypes();
            //获取需要通过接口的实现来依赖注入的类型
            var iTypes = types.Where(a => a.GetInterface("IRepositoryDependency") != null);
            if (iTypes != null && iTypes.Count() > 0)
            {
                iTypes.Where(a => a.IsClass).ToList().ForEach(item =>
                {
                    var interfaceClassName = "I" + item.Name;
                    //获取接口的类型
                    var interfaceClass = iTypes.Where(a => a.Name == interfaceClassName).FirstOrDefault();
                    if (interfaceClass != null)
                    {
                        services.AddScoped(interfaceClass, item);
                    }
                });
            }

            //配置ef上下文
            services.Configure(UseEntityFramework<TContext>());
            return services;
        }

        /// <summary>
        /// 注入ef上下文的类型
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static Action<EFOptions> UseEntityFramework<TContext>() where TContext : DbContext
        {
            Action<EFOptions> action = (options) =>
            {
                options.DbContextType = typeof(TContext);
            };
            return action;
        }
    }
}
