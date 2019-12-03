
using Fate.Infrastructure.Mongo.Base;
using Fate.Infrastructure.Mongo.Interface;
using Fate.Infrastructure.Mongo.Object;
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
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClientFactory, DefaultMongoClientFactory>();
            services.AddScoped<MongoContextOptions>();
            services.AddScoped(typeof(IMongoQuery<,>), typeof(DefaultMongoQuery<,>));
            services.AddScoped(typeof(IMongoCommand<,>), typeof(DefaultMongoCommand<,>));
            services.AddScoped(typeof(IMongoRepository<>), typeof(DefaultMongoRepository<>));
            services.AddScoped(typeof(IMongoReadInfrastructure<>), typeof(MongoReadInfrastructure<>));
            services.AddScoped(typeof(IMongoWriteReadInfrastructure<>), typeof(MongoWriteReadInfrastructure<>));
            return services;
        }
    }
}
