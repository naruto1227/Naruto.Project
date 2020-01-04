using Fate.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;


namespace Fate.Infrastructure.Repository.UnitOfWork
{
    
    public interface IUnitOfWork<TDbContext> : IDisposable, IUnitOfWork, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
