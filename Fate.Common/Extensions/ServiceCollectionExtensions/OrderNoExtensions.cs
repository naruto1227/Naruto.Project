using System;
using System.Collections.Generic;
using System.Text;

using Fate.Common.Options;
using Fate.Common.Repository.UnitOfWork;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrderNoExtensions
    {
        /// <summary>
        /// 配置单号生成的上下文参数信息
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection UseOrderNo<TUnitOfWork>(this IServiceCollection services) where TUnitOfWork : IUnitOfWork
        {
            return services.Configure<OrderNoOptions>(a => a.TUnitOfWorkType = typeof(TUnitOfWork));
        }
    }
}
