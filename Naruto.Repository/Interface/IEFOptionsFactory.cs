using Microsoft.EntityFrameworkCore;
using Naruto.Repository.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Repository.Interface
{
    /// <summary>
    /// 张海波
    /// 2020-03-30
    /// EFOption工厂操作
    /// </summary>
    public interface IEFOptionsFactory
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        EFOptions Get<TDbContext>() where TDbContext : DbContext;

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        EFOptions Get(Type dbContext);
    }
}
