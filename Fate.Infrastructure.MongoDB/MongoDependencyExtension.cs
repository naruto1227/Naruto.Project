
using Fate.Infrastructure.MongoDB.Base;
using Fate.Infrastructure.MongoDB.Interface;
using Fate.Infrastructure.MongoDB.Object;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddMongoServices(this IServiceCollection services, Action<List<MongoContext>> options)
        {
            services.Configure(options);
            services.AddServices();
            return services;
        }
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoServices(this IServiceCollection services, IConfiguration options)
        {
            services.Configure<List<MongoContext>>(options);
            services.AddServices();
            return services;
        }
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClientFactory, DefaultMongoClientFactory>();
            services.AddScoped<MongoContextOptions>();
            services.AddScoped(typeof(IMongoQuery<,>), typeof(DefaultMongoQuery<,>));
            services.AddScoped(typeof(IMongoCommand<,>), typeof(DefaultMongoCommand<,>));
            services.AddScoped(typeof(IMongoRepository<>), typeof(DefaultMongoRepository<>));
            services.AddScoped(typeof(IMongoInfrastructureBase<>), typeof(MongoInfrastructureBase<>));
            services.AddScoped(typeof(IMongoDataBaseInfrastructure<>), typeof(DefaultMongoDataBaseInfrastructure<>));
            services.AddScoped(typeof(IMongoIndexInfrastructure<,>), typeof(DefaultMongoIndexInfrastructure<,>));
            return services;
        }
    }
}
