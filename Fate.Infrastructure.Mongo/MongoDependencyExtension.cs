using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Mongo
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
            return services;
        }
    }
}
