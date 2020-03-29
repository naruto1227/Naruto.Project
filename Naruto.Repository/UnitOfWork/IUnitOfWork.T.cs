using Naruto.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;


namespace Naruto.Repository.UnitOfWork
{
    
    public interface IUnitOfWork<TDbContext> : IDisposable, IUnitOfWork, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
