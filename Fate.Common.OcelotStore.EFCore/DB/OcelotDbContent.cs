using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fate.Common.OcelotStore.EFCore
{
    /// <summary>
    /// 张海波
    /// 2019-08-14
    /// ocelot的数据上下文
    /// </summary>
    public class OcelotDbContent : DbContext
    {
        public OcelotDbContent(DbContextOptions<OcelotDbContent> options)
   : base(options)
        {

        }

        public DbSet<OcelotConfiguration> OcelotConfiguration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OcelotConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).HasColumnType("varchar(255)");

                entity.Property(e => e.ReRoutes).HasColumnType("longtext");
                entity.Property(e => e.DynamicReRoutes).HasColumnType("longtext");
                entity.Property(e => e.Aggregates).HasColumnType("longtext");
                entity.Property(e => e.GlobalConfiguration).HasColumnType("longtext");
            });
        }
    }
}
