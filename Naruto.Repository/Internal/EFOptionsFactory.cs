using Microsoft.EntityFrameworkCore;
using Naruto.Repository.Interface;
using Naruto.Repository.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Repository.Internal
{
    /// <summary>
    /// 默认实现
    /// </summary>
    public class EFOptionsFactory : IEFOptionsFactory
    {
        private readonly IServiceProvider serviceProvider;
        public EFOptionsFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        public EFOptions Get<TDbContext>() where TDbContext : DbContext
        {
            return serviceProvider.GetService(MergeNamedType.Get(typeof(TDbContext).Name)) as EFOptions;
        }

        public EFOptions Get(Type dbContext)
        {
            return serviceProvider.GetService(MergeNamedType.Get(dbContext.Name)) as EFOptions;
        }
    }
}
