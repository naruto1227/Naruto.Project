using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fate.Common.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Fate.Common.Repository.Base
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
        public bool Set(Type DbContextType, DbContext dbContext)
        {
            if (DbContextType == null)
                throw new ArgumentNullException(nameof(DbContextType));
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            return _dbContexts.TryAdd(DbContextType, dbContext);
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
