using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fate.Infrastructure.AutofacDependencyInjection
{
    public class AutofacModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            //.Domain是服务所在程序集命名空间  
            Assembly assembliesDomain = Assembly.Load("Fate.Domain");
            //appcation 服务所在的程序
            Assembly assemblyApp = Assembly.Load("Fate.Application");

            //自动注册领域服务层接口
            builder.RegisterAssemblyTypes(assembliesDomain)
                .Where(b => b.GetInterface("IDomainServicesDependency") != null)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); //见上方说明

            //自动注册应用层接口
            builder.RegisterAssemblyTypes(assemblyApp)
                .Where(b => b.GetInterface("IAppServicesDependency") != null)
                .InstancePerLifetimeScope(); //见上方说明

            //注入实体层
            Assembly assemblyModel = Assembly.Load("Fate.Domain.Model");
            //注入领域事件
            Assembly assemblyEvent = Assembly.Load("Fate.Domain.Event");
            //注入领域实体
            builder.RegisterAssemblyTypes(assemblyModel).Where(a => a.GetInterface("IModelDependency") != null).InstancePerDependency();
            //注入领域事件
            builder.RegisterAssemblyTypes(assemblyEvent).Where(a => a.GetInterface("IEventDependency") != null).InstancePerLifetimeScope();

            //注入基础设施层
            RegisterCommmon(builder);
        }
        /// <summary>
        /// 基础设施层的注入
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterCommmon(ContainerBuilder builder)
        {
            //获取公共层的程序信息
            Assembly assemblyCommon = Assembly.Load("Fate.Infrastructure");

            //注册公共层的接口
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonDependency") != null).AsImplementedInterfaces().InstancePerLifetimeScope();

            //注册公共层(通过类)
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonClassDependency") != null);

            //注册公共层(单例模式)
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonClassSigleDependency") != null).SingleInstance();

            //注册公共层单例
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonSingleDependency") != null).AsImplementedInterfaces().SingleInstance();

            //注册公共层作用域
            builder.RegisterAssemblyTypes(assemblyCommon).Where(a => a.GetInterface("ICommonScopeDependency") != null).AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}
