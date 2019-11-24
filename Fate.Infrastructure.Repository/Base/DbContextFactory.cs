using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fate.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Fate.Infrastructure.Repository.Base
{

    public class DbContextFactory : IDbContextFactory, IDisposable
    {
        public ConcurrentDictionary<Type, DbContext> _dbContexts { get; set; }

        public DbContextFactory()
        {
            _dbContexts = new ConcurrentDictionary<Type, DbContext>();
        }
        /// <summary>
        /// 设置上下文
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <param name="dbContext"></param>
        public void Set(Type DbContextType, DbContext dbContext)
        {
            if (DbContextType == null)
                throw new ArgumentNullException(nameof(DbContextType));
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbContexts.AddOrUpdate(DbContextType, dbContext, (k, oldvalue) => dbContext);
        }
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="DbContextType"></param>
        /// <returns></returns>
        public DbContext Get<TDbContext>() where TDbContext : DbContext
        {
            return _dbContexts[typeof(TDbContext)];
        }

        void IDisposable.Dispose()
        {
            _dbContexts?.Clear();
            _dbContexts = null;
            GC.SuppressFinalize(this);
        }
    }
}
