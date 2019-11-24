using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fate.Infrastructure.Repository.Object;

using Microsoft.EntityFrameworkCore;
using Fate.Infrastructure.Repository.Interceptor;
using System.Diagnostics;
using Fate.Infrastructure.Repository.HostServer;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 张海波
    /// 2019-08.13
    /// 仓储层的扩展服务
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 扩展服务
        /// </summary>
        private static List<Action<IServiceCollection>> Extension = new List<Action<IServiceCollection>>();

        /// <summary>
        /// 注入仓储服务(依赖注入)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryServer(this IServiceCollection services)
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
            services.AddScoped<UnitOfWorkOptions>();
            return services;
        }

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

            //获取参数 并执行委托
            List<EFOptions> efOptionsList = new List<EFOptions>();
            for (int i = 0; i < action.Count(); i++)
            {
                EFOptions eFOptions = new EFOptions();
                action[i]?.Invoke(eFOptions);
                if (eFOptions == null)
                    continue;
                //注入上下文服务扩展
                Extension[i](services);
                //加入集合配置
                efOptionsList.Add(eFOptions);
                //验证
                if (eFOptions.IsOpenMasterSlave && (eFOptions.ReadOnlyConnectionString == null || eFOptions.ReadOnlyConnectionString.Count() <= 0))
                {
                    throw new ArgumentException("检测到开启了读写分离但是未指定只读的连接字符串!");
                }
                //写入连接字符串的线程安全集合
                if (eFOptions.IsOpenMasterSlave)
                {
                    List<SlaveDbConnection> slaveDbConnections = new List<SlaveDbConnection>();
                    eFOptions.ReadOnlyConnectionString.ToList().ForEach(readItem =>
                    {
                        var items = readItem.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        slaveDbConnections.Add(new SlaveDbConnection() { ConnectionString = readItem, IsAvailable = true, HostName = items.Where(a => a.Contains("datasource")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1], Port = Convert.ToInt32(items.Where(a => a.Contains("port")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1]) });
                    });
                    SlavePools.slaveConnec.TryAdd(eFOptions.DbContextType, slaveDbConnections);
                }
            }

            //注入配置
            services.Configure<List<EFOptions>>(a =>
            {
                foreach (var item in efOptionsList)
                {
                    a.Add(item);
                }
            });

            //注入拦截器
            services.AddScoped<EFCommandInterceptor>();
            services.AddScoped<EFDiagnosticListener>();
            DiagnosticListener.AllListeners.Subscribe(services.BuildServiceProvider().GetRequiredService<EFDiagnosticListener>());

            //当从库有信息则执行定时服务
            if (SlavePools.slaveConnec.Count > 0)
            {
                //注入后台服务
                services.AddHostedService<MasterSlaveHostServer>();
            }
            Extension.Clear();
            return services;
        }

        /// <summary>
        /// 注入EFCOre 上下文 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="eFOptions"></param>
        /// <param name="isDbContextPool">是否设置上下文池</param>
        /// <param name="pollSize">连接池数量</param>
        public static void UseEntityFramework<TDbContext>(this EFOptions eFOptions, bool isDbContextPool = true, int pollSize = 128) where TDbContext : DbContext
        {
            if (eFOptions == null)
                throw new ArgumentNullException("值不能为空!");
            //获取上下文的实例
            if (eFOptions.DbContextType == null)
                eFOptions.DbContextType = typeof(TDbContext);
            void DbContextExtension(IServiceCollection serviceDescriptors)
            {
                //添加master 主库的上下文
                if (isDbContextPool)
                    serviceDescriptors.AddDbContextPool<TDbContext>(eFOptions.ConfigureDbContext, pollSize);
                else
                    serviceDescriptors.AddDbContext<TDbContext>(eFOptions.ConfigureDbContext);
                using (var server = serviceDescriptors.BuildServiceProvider().CreateScope())
                {
                    //获取master主库的连接字符串
                    var dbContent = server.ServiceProvider.GetRequiredService<TDbContext>();
                    eFOptions.WriteReadConnectionString = dbContent.Database.GetDbConnection().ConnectionString;
                }
            }
            Extension.Add(DbContextExtension);
        }
    }
}
