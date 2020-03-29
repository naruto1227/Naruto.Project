
using Naruto.MongoDB.Base;
using Naruto.MongoDB.Interface;
using Naruto.MongoDB.Object;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class MongoDependencyExtension
    {
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoServices(this IServiceCollection services)
        {
            if (services.BuildServiceProvider().GetService<IMongoClientFactory>() != null)
                return services;
            services.TryAddSingleton<IMongoClientFactory, DefaultMongoClientFactory>();
            services.TryAddScoped(typeof(MongoContextOptions<>));
            services.TryAddScoped(typeof(IMongoQuery<,>), typeof(DefaultMongoQuery<,>));
            services.TryAddScoped(typeof(IMongoCommand<,>), typeof(DefaultMongoCommand<,>));
            services.TryAddScoped(typeof(IMongoRepository<>), typeof(DefaultMongoRepository<>));
            services.TryAddScoped(typeof(IMongoInfrastructure<>), typeof(MongoInfrastructure<>));
            services.TryAddScoped(typeof(IMongoDataBaseInfrastructure<>), typeof(DefaultMongoDataBase<>));
            services.TryAddScoped(typeof(IMongoIndexInfrastructure<,>), typeof(DefaultMongoIndex<,>));
            services.TryAddScoped(typeof(IMongoGridFS<>), typeof(DefaultMongoGridFS<>));
            services.TryAddScoped(typeof(IMongoGridFSInfrastructure<>), typeof(DefaultMongoGridFSInfrastructure<>));
            return services;
        }


        /// <summary>
        /// 注入服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoContext<TMongoContext>(this IServiceCollection services, Action<TMongoContext> options) where TMongoContext : MongoContext, new()
        {
            TMongoContext mongoContext = new TMongoContext();
            options?.Invoke(mongoContext);

            //注入配置服务
            services.Add(new ServiceDescriptor(MergeNamedType.Merge(typeof(TMongoContext).Name, typeof(TMongoContext)), serviceProvider => mongoContext, ServiceLifetime.Singleton));

            return services;
        }
    }
}
