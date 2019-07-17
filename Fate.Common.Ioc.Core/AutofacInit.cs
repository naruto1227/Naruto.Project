using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using System.Linq;

namespace Fate.Common.Ioc.Core
{
    public class AutofacInit
    {
        private static IContainer container;
        private static ContainerBuilder builder;
        public static IContainer Injection(IServiceCollection services)
        {
            builder = new ContainerBuilder();
            //InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例
            //SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
            //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //获取所有需要依赖注入的程序集
            //.Domain是服务所在程序集命名空间  
            Assembly assembliesDomain = Assembly.Load("Fate.Domain");
            //appcation 服务所在的程序
            Assembly assemblyApp = Assembly.Load("Fate.Application");
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(assembliesDomain);
            assemblies.Add(assemblyApp);

            //获取公共层的程序信息
            Assembly assemblyCommon = Assembly.Load("Fate.Common");

            //注入实体层
            Assembly assemblyModel = Assembly.Load("Fate.Domain.Model");
            //注入领域事件
            Assembly assemblyEvent = Assembly.Load("Fate.Domain.Event");

            //自动注册接口
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(b => b.GetInterface("IAppServicesDependency") != null || b.GetInterface("IDomainServicesDependency") != null)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); //见上方说明
            //注册公共层的接口
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonDependency") != null).AsImplementedInterfaces().InstancePerLifetimeScope();

            //注册公共层(通过类)
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonClassDependency") != null);

            //注册公共层(单例模式)
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonClassSigleDependency") != null).SingleInstance();
            //注入领域实体
            builder.RegisterAssemblyTypes(assemblyModel).Where(a => a.GetInterface("IModelDependency") != null).InstancePerDependency();
            //注入领域事件
            builder.RegisterAssemblyTypes(assemblyEvent).Where(a => a.GetInterface("IEventDependency") != null).InstancePerLifetimeScope();
            builder.Populate(services);
            container = builder.Build();
            return container;
        }
        #region 当前方法注册需要类与接口的一一对应
        /// <summary>
        /// 注册类到容器
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <typeparam name="IT">当前类所继承的接口</typeparam>
        public static void RegisterType<T, IT>(LiftTimeEnum liftTimeEnum = LiftTimeEnum.InstancePerDependency) where T : class where IT : class
        {
            if (liftTimeEnum == LiftTimeEnum.SingleInstance)
            {
                builder.RegisterType<T>().As<IT>().SingleInstance();
            }
            else if (liftTimeEnum == LiftTimeEnum.InstancePerLifetimeScope)
            {
                builder.RegisterType<T>().As<IT>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<T>().As<IT>();
            }
        }
        /// <summary>
        /// 控制反转
        /// </summary>
        /// <typeparam name="T">类所继承的接口</typeparam>
        /// <returns></returns>

        public static IT Resolve<IT>() where IT : class
        {
            return container.Resolve<IT>();
        }
        #endregion

        #region 通过name注册
        /// <summary>
        /// 注册类到容器 通过key
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <typeparam name="IT">当前类所继承的接口</typeparam>
        public static void RegisterTypeByKeyName<T, IT>(AutoFacInitByKeyEnum autoFacInitByKeyEnum, LiftTimeEnum liftTimeEnum = LiftTimeEnum.InstancePerDependency) where T : class where IT : class
        {
            if (liftTimeEnum == LiftTimeEnum.SingleInstance)
            {
                builder.RegisterType<T>().Keyed<IT>(autoFacInitByKeyEnum).SingleInstance();
            }
            else if (liftTimeEnum == LiftTimeEnum.InstancePerLifetimeScope)
            {
                builder.RegisterType<T>().Keyed<IT>(autoFacInitByKeyEnum).InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<T>().Keyed<IT>(autoFacInitByKeyEnum);
            }
        }

        public static IT ResolveByKeyName<IT>(AutoFacInitByKeyEnum autoFacInitByKeyEnum) where IT : class
        {
            return container.ResolveKeyed<IT>(autoFacInitByKeyEnum);
        }

        #endregion

    }
}
