using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Fate.Common.Mapper;
using Fate.Common.Mapper.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// automapper 依赖注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册实体映射关系
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typesParams"></param>
        /// <returns></returns>
        public static IServiceCollection AddRegisterMapper(this IServiceCollection services, params Type[] typesParams)
        {

            services.AddTransient(typeof(IAutoInjectFactory), typeof(DefaultAutoInjectFactory));
            services.AddTransient(typeof(IEntityMapper), typeof(DefaultEntityMapper));
            return services.AddAutoMapper(item =>
            {
                using (var itemServices = services.BuildServiceProvider().CreateScope())
                {
                    //获取服务
                    var autoInjectFactory = itemServices.ServiceProvider.GetRequiredService<IAutoInjectFactory>();
                    //获取所有需要映射的类型对象
                    var list = autoInjectFactory.GetFromAssemblys(typesParams);
                    if (list != null && list.Count() > 0)
                    {
                        foreach (var itemOptions in list)
                        {
                            item.CreateMap(itemOptions.SoureType, itemOptions.TargetType).IgnoreAllExisting(itemOptions.IgnoreName).ReverseMap(itemOptions.ReverseMap);
                        }
                    }
                }
            }
            , typesParams);
        }
    }
}
