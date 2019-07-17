using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Fate.Common.Repository.Mysql
{

    public static class RepositoryExtensions
    {
        /// <summary>
        /// 注入mysql仓储服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMysqlRepositoryServer(this IServiceCollection services)
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
            return services;
        }
    }
}
