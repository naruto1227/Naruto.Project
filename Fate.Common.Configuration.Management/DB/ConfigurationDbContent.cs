using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration.Management.DB
{
    /// <summary>
    /// 张海波
    /// 2019-10-16
    /// 配置信息的数据上下文
    /// </summary>
    public class ConfigurationDbContent : DbContext
    {
        public ConfigurationDbContent(DbContextOptions<ConfigurationDbContent> options)
  : base(options)
        {

        }

        public DbSet<ConfigurationEndPoint> ConfigurationEndPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationEndPoint>(entity =>
            {
                entity.ToTable("ConfigurationEndPoint");
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.HasIndex(e => e.EnvironmentType).HasName("index_EnvironmentType");
                entity.HasIndex(e => e.Group).HasName("index_Group");
                entity.Property(e => e.Id).HasColumnType("varchar(100)");
                entity.Property(e => e.EnvironmentType).HasColumnType("int(2)").HasDefaultValue(0);
                entity.Property(e => e.Group).HasColumnType("varchar(255)").HasDefaultValue("");
                entity.Property(e => e.Key).HasColumnType("varchar(255)");
                entity.Property(e => e.Value).HasColumnType("longtext");
                entity.Property(e => e.Remark).HasColumnType("longtext").HasDefaultValue("");
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
            });
        }
    }
}
