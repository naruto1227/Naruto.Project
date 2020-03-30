using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Naruto.Repository.Object;
using Microsoft.EntityFrameworkCore;
using Naruto.Repository.HostServer;
using Naruto.Repository;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Naruto.Repository.Interface;
using Naruto.Repository.Internal;

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
        private static Dictionary<Type, Action<IServiceCollection>> Extension = new Dictionary<Type, Action<IServiceCollection>>();

        #region 注入服务

        /// <summary>
        /// 注入仓储服务(依赖注入)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //验证仓储服务是否已经注入 注入了话就
            if (services.BuildServiceProvider().GetService<ISlaveDbConnectionFactory>() != null)
                return services;
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
                        services.TryAddScoped(item, classType);
                    }
                });
            }
            services.TryAddSingleton<ISlaveDbConnectionFactory, DefaultSlaveDbConnectionFactory>();
            services.TryAddScoped(typeof(UnitOfWorkOptions<>));
            services.TryAddSingleton<IEFOptionsFactory, EFOptionsFactory>();
            return services;
        }

        #endregion

        #region 注入仓储对应的EF信息

        /// <summary>
        /// 注入上下文的实例类型
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddEFOption(this IServiceCollection services, Action<EFOptions> action)
        {
            EFOptions eFOptions = new EFOptions();
            action?.Invoke(eFOptions);
            if (eFOptions == null)
                throw new ArgumentNullException(nameof(action));
            //注入上下文服务扩展
            Extension[eFOptions.DbContextType](services);

            //验证
            if (eFOptions.IsOpenMasterSlave && (eFOptions.ReadOnlyConnectionString == null || eFOptions.ReadOnlyConnectionString.Count() <= 0))
            {
                throw new ArgumentException("检测到开启了读写分离但是未指定只读的连接字符串!");
            }
            //写入连接字符串的线程安全集合
            if (eFOptions.IsOpenMasterSlave)
            {
                var slaveDbConnectionFactory = services.BuildServiceProvider().GetRequiredService<ISlaveDbConnectionFactory>();
                SlavePools.slaveConnec.TryAdd(eFOptions.DbContextType, slaveDbConnectionFactory.Get(eFOptions));
            }
            //注入配置服务
            services.Add(new ServiceDescriptor(MergeNamedType.Merge(eFOptions.DbContextType.Name, typeof(EFOptions)), serviceProvider => eFOptions, ServiceLifetime.Singleton));
            return services;
        }

        #endregion

        #region  注入EntityFramework实体信息

        /// <summary>
        /// 注入EFCOre 上下文 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="eFOptions"></param>
        /// <param name="isDbContextPool">是否设置上下文池</param>
        /// <param name="pollSize">连接池数量</param>
        public static void UseEntityFramework<TDbContext>(this EFOptions eFOptions, bool isDbContextPool = true, int pollSize = 128) where TDbContext : DbContext
        {
            eFOptions.UseEntityFramework<TDbContext, TDbContext>(isDbContextPool, pollSize);
        }
        /// <summary>
        /// 注入EFCOre 上下文 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="eFOptions"></param>
        /// <param name="isDbContextPool">是否设置上下文池</param>
        /// <param name="pollSize">连接池数量</param>
        public static void UseEntityFramework<TDbContext, TSlaveDbContext>(this EFOptions eFOptions, bool isDbContextPool = true, int pollSize = 128) where TDbContext : DbContext where TSlaveDbContext : DbContext
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

                //验证是否开启读写分离
                if (eFOptions.IsOpenMasterSlave)
                {
                    if (typeof(TDbContext) == typeof(TSlaveDbContext))
                    {
                        throw new ArgumentException($"{nameof(TSlaveDbContext)}参数配置错误");
                    }
                    //添加slave 从库的上下文
                    if (isDbContextPool)
                        serviceDescriptors.AddDbContextPool<TSlaveDbContext>(eFOptions.ConfigureDbContext, pollSize);
                    else
                        serviceDescriptors.AddDbContext<TSlaveDbContext>(eFOptions.ConfigureDbContext);

                    eFOptions.SlaveDbContextType = typeof(TSlaveDbContext);
                }
                using (var server = serviceDescriptors.BuildServiceProvider().CreateScope())
                {
                    //获取master主库的连接字符串
                    var dbContent = server.ServiceProvider.GetRequiredService<TDbContext>();
                    eFOptions.WriteReadConnectionString = dbContent.Database.GetDbConnection().ConnectionString;
                }
            }
            Extension.Add(eFOptions.DbContextType, DbContextExtension);
        }

        #endregion

        #region 注入仓储的心跳检查 

        /// <summary>
        /// 注入仓储的心跳检查
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryHealthCheck(this IServiceCollection services)
        {
            return services.AddHostedService<MasterSlaveHostServer>();
        }

        #endregion
    }
}
