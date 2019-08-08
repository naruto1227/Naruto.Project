using Fate.Common.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Repository.UnitOfWork
{
    
    public interface IUnitOfWork<TDbContext> : IDisposable, IUnitOfWork, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
