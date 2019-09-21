using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.Mappers;
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 实体映射
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// 注入服务 实体映射
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper();
            return services;
        }
    }
}
