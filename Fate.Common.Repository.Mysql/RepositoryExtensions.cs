using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fate.Common.Repository.Mysql.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Fate.Common.Repository.Mysql.Interceptor;
using System.Diagnostics;
using Fate.Common.Repository.Mysql.HostServer;

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
            var repositoryDependencyTypes = types.Where(a => a.GetInterface("IRepositoryDependency") != null);
            if (repositoryDependencyTypes != null && repositoryDependencyTypes.Count() > 0)
            {
                repositoryDependencyTypes.Where(a => a.IsInterface).ToList().ForEach(item =>
                {
                    //获取当前接口对应的 实现类
                    var classType = repositoryDependencyTypes.Where(a => a.IsClass && a.GetInterface(item.Name) != null).FirstOrDefault();
                    if (classType != null)
                    {
                        services.AddScoped(item, classType);
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
        //public static IServiceCollection AddRepositoryEFOptionServer<TContext, TEFOptions>(this IServiceCollection services, Action<TEFOptions> action) where TEFOptions : EFOptions, new() where TContext : DbContext
        //{
        //    //获取参数
        //    TEFOptions options = new TEFOptions();
        //    action?.Invoke(options);

        //    services.AddDbContext<TContext>(options?.ConfigureDbContext);
        //    services.Configure<TEFOptions>(option =>
        //    {
        //        option.ConfigureDbContext = options?.ConfigureDbContext;
        //        option.DbContextType = options?.DbContextType ?? typeof(TContext);
        //        option.ReadOnlyConnectionName = options?.ReadOnlyConnectionName;
        //        option.WriteReadConnectionName = options?.WriteReadConnectionName;
        //    });
        //    return services;
        //}

        /// <summary>
        /// 注入上下文的实例类型
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryEFOptionServer(this IServiceCollection services, params Action<EFOptions>[] action)
        {
            if (action == null || action.Count() <= 0)
            {
                throw new ArgumentNullException("值不能为空!");
            }
            //获取参数
            List<EFOptions> options = new List<EFOptions>();

            foreach (var item in action)
            {
                EFOptions eFOptions = new EFOptions();
                item?.Invoke(eFOptions);
                options.Add(eFOptions);
                //验证
                if (eFOptions.ReadOnlyConnectionString == null || eFOptions.ReadOnlyConnectionString.Count() <= 0)
                {
                    eFOptions.ReadOnlyConnectionString = new string[] { eFOptions.WriteReadConnectionString };
                }
                //写如连接字符串的线程安全集合
                List<SlaveDbConnection> slaveDbConnections = new List<SlaveDbConnection>();
                eFOptions.ReadOnlyConnectionString.ToList().ForEach(readItem =>
                {
                    var items = readItem.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    slaveDbConnections.Add(new SlaveDbConnection() { ConnectionString = readItem, IsAvailable = true, HostName = items.Where(a => a.Contains("datasource")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1], Port = Convert.ToInt32(items.Where(a => a.Contains("port")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1]) });
                });
                SlavePools.slaveConnec.TryAdd(eFOptions.DbContextType, slaveDbConnections);
            }
            services.Configure<List<EFOptions>>(a =>
            {
                foreach (var item in options)
                {
                    a.Add(item);
                }
            });
            //注入拦截器
            services.AddScoped<EFCommandInterceptor>();
            services.AddScoped<EFDiagnosticListener>();
            //DiagnosticListener.AllListeners.Subscribe(services.BuildServiceProvider().GetRequiredService<EFDiagnosticListener>());
            //注入后台服务
            services.AddHostedService<MasterSlaveHostServer>();
            return services;
        }

        /// <summary>
        /// 注入EFCOre 上下文 (此参数配置需要放置在ConfigureDbContext后)
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="eFOptions"></param>
        public static void UseEntityFramework<TDbContext>(this EFOptions eFOptions, IServiceCollection services) where TDbContext : DbContext
        {
            if (services == null)
                throw new ArgumentNullException("services 不能为空!");
            if (eFOptions == null)
                throw new ArgumentNullException("值不能为空!");
            if (eFOptions.ConfigureDbContext == null)
                throw new ArgumentNullException("请先配置上下文的类型ConfigureDbContext!");
            //添加master 主库的上下文
            services.AddDbContext<TDbContext>(eFOptions.ConfigureDbContext);
            if (eFOptions.DbContextType == null)
                eFOptions.DbContextType = typeof(TDbContext); //获取上下文的实例
            //获取master主库的连接字符串
            if (string.IsNullOrWhiteSpace(eFOptions.WriteReadConnectionString))
            {
                var dbContent = services.BuildServiceProvider().GetRequiredService<TDbContext>();
                eFOptions.WriteReadConnectionString = dbContent.Database.GetDbConnection().ConnectionString;
            }
        }
    }
}
