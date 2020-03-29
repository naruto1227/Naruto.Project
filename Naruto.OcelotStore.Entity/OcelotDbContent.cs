using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Naruto.OcelotStore.Entity;

namespace Naruto.OcelotStore.Entity
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

        public DbSet<OcelotAggregateReRoute> OcelotAggregateReRoute { get; set; }

        public DbSet<OcelotAggregateReRouteConfig> OcelotAggregateReRouteConfig { get; set; }
        public DbSet<OcelotAuthenticationOption> OcelotAuthenticationOption { get; set; }

        public DbSet<OcelotCacheOptions> OcelotCacheOptions { get; set; }
        public DbSet<OcelotGlobalConfiguration> OcelotGlobalConfiguration { get; set; }
        public DbSet<OcelotDictionaryClaims> OcelotDictionaryClaims { get; set; }
        public DbSet<OcelotHeaderTransform> OcelotHeaderTransform { get; set; }
        public DbSet<OcelotHostAndPort> OcelotHostAndPort { get; set; }
        public DbSet<OcelotHttpHandlerOptions> OcelotHttpHandlerOptions { get; set; }
        public DbSet<OcelotLoadBalancer> OcelotLoadBalancer { get; set; }
        public DbSet<OcelotQoSOptions> OcelotQoSOptions { get; set; }
        public DbSet<OcelotRateLimitRule> OcelotRateLimitRule { get; set; }
        public DbSet<OcelotReRoute> OcelotReRoute { get; set; }
        public DbSet<OcelotSecurityOptions> OcelotSecurityOptions { get; set; }
        public DbSet<OcelotServiceDiscoveryProvider> OcelotServiceDiscoveryProvider { get; set; }

        public DbSet<OcelotRateLimitOptions> OcelotRateLimitOptions { get; set; }
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

            modelBuilder.Entity<OcelotAggregateReRoute>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.Aggregator).HasColumnType("varchar(255)");
                entity.Property(e => e.Priority).HasColumnType("int(5)");
                entity.Property(e => e.ReRouteIsCaseSensitive).HasColumnType("bit(1)");
                entity.Property(e => e.ReRouteKeys).HasColumnType("varchar(255)");
                entity.Property(e => e.UpstreamHost).HasColumnType("varchar(255)");
                entity.Property(e => e.UpstreamHttpMethod).HasColumnType("varchar(100)");
                entity.Property(e => e.UpstreamPathTemplate).HasColumnType("varchar(100)");

                entity.Ignore("OcelotAggregateReRouteConfigs");
            });

            modelBuilder.Entity<OcelotAggregateReRouteConfig>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.JsonPath).HasColumnType("varchar(255)");
                entity.Property(e => e.Parameter).HasColumnType("varchar(255)");
                entity.Property(e => e.ReRouteKey).HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<OcelotAuthenticationOption>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.AllowedScopes).HasColumnType("varchar(255)");
                entity.Property(e => e.AuthenticationProviderKey).HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<OcelotCacheOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Region).HasColumnType("varchar(150)");
                entity.Property(e => e.TtlSeconds).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotDictionaryClaims>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Key).HasColumnType("varchar(255)");
                entity.Property(e => e.Value).HasColumnType("varchar(300)");
                entity.Property(e => e.Type).HasColumnType("int(2)");
            });

            modelBuilder.Entity<OcelotGlobalConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.BaseUrl).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamScheme).HasColumnType("varchar(20)");
                entity.Property(e => e.RequestIdKey).HasColumnType("varchar(50)");
                entity.Ignore("OcelotServiceDiscoveryProvider").Ignore("OcelotLoadBalancer").Ignore("OcelotQoSOptions").Ignore("OcelotRateLimitOptions").Ignore("OcelotHttpHandlerOptions");
            });


            modelBuilder.Entity<OcelotHeaderTransform>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Header).HasColumnType("varchar(255)");
                entity.Property(e => e.IsUpOrDown).HasColumnType("int(2)");
                entity.Property(e => e.Value).HasColumnType("varchar(300)");
            });

            modelBuilder.Entity<OcelotHostAndPort>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Host).HasColumnType("varchar(50)");
                entity.Property(e => e.Port).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotHttpHandlerOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.AllowAutoRedirect).HasColumnType("bit(1)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.UseCookieContainer).HasColumnType("bit(1)");
                entity.Property(e => e.UseProxy).HasColumnType("bit(1)");
                entity.Property(e => e.UseTracing).HasColumnType("bit(1)");
            });

            modelBuilder.Entity<OcelotLoadBalancer>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Expiry).HasColumnType("int(11)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.Key).HasColumnType("varchar(255)");
                entity.Property(e => e.Type).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<OcelotQoSOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.DurationOfBreak).HasColumnType("int(11)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.ExceptionsAllowedBeforeBreaking).HasColumnType("int(11)");
                entity.Property(e => e.TimeoutValue).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotRateLimitRule>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ClientWhitelist).HasColumnType("varchar(255)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.EnableRateLimiting).HasColumnType("bit(1)");
                entity.Property(e => e.Limit).HasColumnType("double");
                entity.Property(e => e.Period).HasColumnType("varchar(150)");
                entity.Property(e => e.PeriodTimespan).HasColumnType("double");
            });
            modelBuilder.Entity<OcelotReRoute>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.DangerousAcceptAnyServerCertificateValidator).HasColumnType("bit(1)");
                entity.Property(e => e.DelegatingHandlers).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamPathTemplate).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamScheme).HasColumnType("varchar(50)");
                entity.Property(e => e.IsServiceDiscovery).HasColumnType("bit(1)");
                entity.Property(e => e.Key).HasColumnType("varchar(150)");

                entity.Property(e => e.Priority).HasColumnType("int(11)");
                entity.Property(e => e.RequestIdKey).HasColumnType("varchar(255)");
                entity.Property(e => e.ReRouteIsCaseSensitive).HasColumnType("bit(1)");
                entity.Property(e => e.ServiceName).HasColumnType("varchar(150)");
                entity.Property(e => e.ServiceNamespace).HasColumnType("varchar(150)");
                entity.Property(e => e.Timeout).HasColumnType("int(11)");

                entity.Property(e => e.UpstreamHost).HasColumnType("varchar(150)");
                entity.Property(e => e.UpstreamHttpMethod).HasColumnType("varchar(150)");
                entity.Property(e => e.UpstreamPathTemplate).HasColumnType("varchar(150)");

                entity.Ignore("OcelotHostAndPorts");
                entity.Ignore("OcelotLoadBalancer");
                entity.Ignore("OcelotAuthenticationOption");
                entity.Ignore("OcelotDictionaryClaims");
                entity.Ignore("OcelotHeaderTransform");
                entity.Ignore("OcelotHttpHandlerOptions");

                entity.Ignore("OcelotQoSOptions");
                entity.Ignore("OcelotRateLimitRule");
                entity.Ignore("OcelotSecurityOptions");
            });

            modelBuilder.Entity<OcelotSecurityOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.IPAllowedList).HasColumnType("longtext");
                entity.Property(e => e.IPBlockedList).HasColumnType("longtext");

            });


            modelBuilder.Entity<OcelotServiceDiscoveryProvider>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ConfigurationKey).HasColumnType("varchar(255)");
                entity.Property(e => e.Host).HasColumnType("varchar(50)");

                entity.Property(e => e.Namespace).HasColumnType("varchar(100)");
                entity.Property(e => e.PollingInterval).HasColumnType("int(11)");
                entity.Property(e => e.Port).HasColumnType("int(11)");
                entity.Property(e => e.Token).HasColumnType("varchar(300)");
                entity.Property(e => e.Type).HasColumnType("varchar(50)");
            });
            modelBuilder.Entity<OcelotRateLimitOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ClientIdHeader).HasColumnType("varchar(255)");
                entity.Property(e => e.DisableRateLimitHeaders).HasColumnType("bit(1)");

                entity.Property(e => e.HttpStatusCode).HasColumnType("int(11)");
                entity.Property(e => e.QuotaExceededMessage).HasColumnType("varchar(255)");
                entity.Property(e => e.RateLimitCounterPrefix).HasColumnType("varchar(255)");
            });

        }
    }



    /// <summary>
    /// 张海波
    /// 2019-08-14
    /// ocelot的从库数据上下文
    /// </summary>
    public class SlaveOcelotDbContent : DbContext
    {
        public SlaveOcelotDbContent(DbContextOptions<SlaveOcelotDbContent> options)
   : base(options)
        {

        }

        public DbSet<OcelotConfiguration> OcelotConfiguration { get; set; }

        public DbSet<OcelotAggregateReRoute> OcelotAggregateReRoute { get; set; }

        public DbSet<OcelotAggregateReRouteConfig> OcelotAggregateReRouteConfig { get; set; }
        public DbSet<OcelotAuthenticationOption> OcelotAuthenticationOption { get; set; }

        public DbSet<OcelotCacheOptions> OcelotCacheOptions { get; set; }
        public DbSet<OcelotGlobalConfiguration> OcelotGlobalConfiguration { get; set; }
        public DbSet<OcelotDictionaryClaims> OcelotDictionaryClaims { get; set; }
        public DbSet<OcelotHeaderTransform> OcelotHeaderTransform { get; set; }
        public DbSet<OcelotHostAndPort> OcelotHostAndPort { get; set; }
        public DbSet<OcelotHttpHandlerOptions> OcelotHttpHandlerOptions { get; set; }
        public DbSet<OcelotLoadBalancer> OcelotLoadBalancer { get; set; }
        public DbSet<OcelotQoSOptions> OcelotQoSOptions { get; set; }
        public DbSet<OcelotRateLimitRule> OcelotRateLimitRule { get; set; }
        public DbSet<OcelotReRoute> OcelotReRoute { get; set; }
        public DbSet<OcelotSecurityOptions> OcelotSecurityOptions { get; set; }
        public DbSet<OcelotServiceDiscoveryProvider> OcelotServiceDiscoveryProvider { get; set; }

        public DbSet<OcelotRateLimitOptions> OcelotRateLimitOptions { get; set; }
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

            modelBuilder.Entity<OcelotAggregateReRoute>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.Aggregator).HasColumnType("varchar(255)");
                entity.Property(e => e.Priority).HasColumnType("int(5)");
                entity.Property(e => e.ReRouteIsCaseSensitive).HasColumnType("bit(1)");
                entity.Property(e => e.ReRouteKeys).HasColumnType("varchar(255)");
                entity.Property(e => e.UpstreamHost).HasColumnType("varchar(255)");
                entity.Property(e => e.UpstreamHttpMethod).HasColumnType("varchar(100)");
                entity.Property(e => e.UpstreamPathTemplate).HasColumnType("varchar(100)");

                entity.Ignore("OcelotAggregateReRouteConfigs");
            });

            modelBuilder.Entity<OcelotAggregateReRouteConfig>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.JsonPath).HasColumnType("varchar(255)");
                entity.Property(e => e.Parameter).HasColumnType("varchar(255)");
                entity.Property(e => e.ReRouteKey).HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<OcelotAuthenticationOption>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.AllowedScopes).HasColumnType("varchar(255)");
                entity.Property(e => e.AuthenticationProviderKey).HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<OcelotCacheOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Region).HasColumnType("varchar(150)");
                entity.Property(e => e.TtlSeconds).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotDictionaryClaims>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Key).HasColumnType("varchar(255)");
                entity.Property(e => e.Value).HasColumnType("varchar(300)");
                entity.Property(e => e.Type).HasColumnType("int(2)");
            });

            modelBuilder.Entity<OcelotGlobalConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.BaseUrl).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamScheme).HasColumnType("varchar(20)");
                entity.Property(e => e.RequestIdKey).HasColumnType("varchar(50)");
                entity.Ignore("OcelotServiceDiscoveryProvider").Ignore("OcelotLoadBalancer").Ignore("OcelotQoSOptions").Ignore("OcelotRateLimitOptions").Ignore("OcelotHttpHandlerOptions");
            });


            modelBuilder.Entity<OcelotHeaderTransform>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Header).HasColumnType("varchar(255)");
                entity.Property(e => e.IsUpOrDown).HasColumnType("int(2)");
                entity.Property(e => e.Value).HasColumnType("varchar(300)");
            });

            modelBuilder.Entity<OcelotHostAndPort>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Host).HasColumnType("varchar(50)");
                entity.Property(e => e.Port).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotHttpHandlerOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.AllowAutoRedirect).HasColumnType("bit(1)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.UseCookieContainer).HasColumnType("bit(1)");
                entity.Property(e => e.UseProxy).HasColumnType("bit(1)");
                entity.Property(e => e.UseTracing).HasColumnType("bit(1)");
            });

            modelBuilder.Entity<OcelotLoadBalancer>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.Expiry).HasColumnType("int(11)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.Key).HasColumnType("varchar(255)");
                entity.Property(e => e.Type).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<OcelotQoSOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.DurationOfBreak).HasColumnType("int(11)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.ExceptionsAllowedBeforeBreaking).HasColumnType("int(11)");
                entity.Property(e => e.TimeoutValue).HasColumnType("int(11)");
            });

            modelBuilder.Entity<OcelotRateLimitRule>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ClientWhitelist).HasColumnType("varchar(255)");
                entity.Property(e => e.IsReRouteOrGlobal).HasColumnType("int(2)");
                entity.Property(e => e.EnableRateLimiting).HasColumnType("bit(1)");
                entity.Property(e => e.Limit).HasColumnType("double");
                entity.Property(e => e.Period).HasColumnType("varchar(150)");
                entity.Property(e => e.PeriodTimespan).HasColumnType("double");
            });
            modelBuilder.Entity<OcelotReRoute>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.DangerousAcceptAnyServerCertificateValidator).HasColumnType("bit(1)");
                entity.Property(e => e.DelegatingHandlers).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamPathTemplate).HasColumnType("varchar(255)");
                entity.Property(e => e.DownstreamScheme).HasColumnType("varchar(50)");
                entity.Property(e => e.IsServiceDiscovery).HasColumnType("bit(1)");
                entity.Property(e => e.Key).HasColumnType("varchar(150)");

                entity.Property(e => e.Priority).HasColumnType("int(11)");
                entity.Property(e => e.RequestIdKey).HasColumnType("varchar(255)");
                entity.Property(e => e.ReRouteIsCaseSensitive).HasColumnType("bit(1)");
                entity.Property(e => e.ServiceName).HasColumnType("varchar(150)");
                entity.Property(e => e.ServiceNamespace).HasColumnType("varchar(150)");
                entity.Property(e => e.Timeout).HasColumnType("int(11)");

                entity.Property(e => e.UpstreamHost).HasColumnType("varchar(150)");
                entity.Property(e => e.UpstreamHttpMethod).HasColumnType("varchar(150)");
                entity.Property(e => e.UpstreamPathTemplate).HasColumnType("varchar(150)");

                entity.Ignore("OcelotHostAndPorts");
                entity.Ignore("OcelotLoadBalancer");
                entity.Ignore("OcelotAuthenticationOption");
                entity.Ignore("OcelotDictionaryClaims");
                entity.Ignore("OcelotHeaderTransform");
                entity.Ignore("OcelotHttpHandlerOptions");

                entity.Ignore("OcelotQoSOptions");
                entity.Ignore("OcelotRateLimitRule");
                entity.Ignore("OcelotSecurityOptions");
            });

            modelBuilder.Entity<OcelotSecurityOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.IPAllowedList).HasColumnType("longtext");
                entity.Property(e => e.IPBlockedList).HasColumnType("longtext");

            });


            modelBuilder.Entity<OcelotServiceDiscoveryProvider>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ConfigurationKey).HasColumnType("varchar(255)");
                entity.Property(e => e.Host).HasColumnType("varchar(50)");

                entity.Property(e => e.Namespace).HasColumnType("varchar(100)");
                entity.Property(e => e.PollingInterval).HasColumnType("int(11)");
                entity.Property(e => e.Port).HasColumnType("int(11)");
                entity.Property(e => e.Token).HasColumnType("varchar(300)");
                entity.Property(e => e.Type).HasColumnType("varchar(50)");
            });
            modelBuilder.Entity<OcelotRateLimitOptions>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PRIMARY");
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int(11)");
                entity.Property(e => e.ParentId).HasColumnType("int(11)");
                entity.Property(e => e.ClientIdHeader).HasColumnType("varchar(255)");
                entity.Property(e => e.DisableRateLimitHeaders).HasColumnType("bit(1)");

                entity.Property(e => e.HttpStatusCode).HasColumnType("int(11)");
                entity.Property(e => e.QuotaExceededMessage).HasColumnType("varchar(255)");
                entity.Property(e => e.RateLimitCounterPrefix).HasColumnType("varchar(255)");
            });

        }
    }
}
