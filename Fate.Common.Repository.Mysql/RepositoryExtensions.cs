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
        /// 注入mysql仓储服务(依赖注入)
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

        /// <summary>
        /// 注入上下文的实例类型
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>

        /// <typeparam name="TEFOptions">上下文参数的实例类型</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryEFOptionServer<TContext, TEFOptions>(this IServiceCollection services, Action<TEFOptions> action) where TEFOptions : EFOptions, new() where TContext : DbContext
        {
            //获取参数
            TEFOptions options = new TEFOptions();
            action?.Invoke(options);

            services.AddDbContext<TContext>(options?.ConfigureDbContext);
            services.Configure<TEFOptions>(option =>
            {
                option.ConfigureDbContext = options?.ConfigureDbContext;
                option.DbContextType = options?.DbContextType ?? typeof(TContext);
                option.ReadOnlyConnectionName = options?.ReadOnlyConnectionName;
                option.WriteReadConnectionName = options?.WriteReadConnectionName;
            });
            return services;
        }
    }
}
