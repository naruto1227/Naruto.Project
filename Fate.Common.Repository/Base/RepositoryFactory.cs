using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fate.Common.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Fate.Common.Repository.Base
{

    public class RepositoryFactory : IRepositoryFactory, IDisposable
    {
        public Lazy<DbContext> _dbContext { get; set; }

        public void Set(DbContext dbContext)
        {
            _dbContext = new Lazy<DbContext>(() => dbContext);
        }

        public DbContext Get()
        {
            return _dbContext?.Value;
        }

        void IDisposable.Dispose()
        {
            _dbContext.Value?.Dispose();
        }
    }
}
