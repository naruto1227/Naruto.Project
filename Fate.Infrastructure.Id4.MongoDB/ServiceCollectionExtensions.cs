using Fate.Infrastructure.Id4.MongoDB.Options;
using Fate.Infrastructure.Id4.MongoDB.Services;
using Fate.Infrastructure.Id4.MongoDB.Stores;
using Fate.Infrastructure.Id4.MongoDB.Tokens;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Id4.MongoDB
{
    public static class ServiceCollectionExtensions
    {  
        /// <summary>
        /// Configures implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder)
        {
            //注入替换的服务
            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddCorsPolicyService<CorsPolicyService>();
            builder.AddPersistedGrantStore<PersistedGrantStore>();
            builder.AddDeviceFlowStore<DeviceFlowStore>();

            return builder;
        }

        /// <summary>
        /// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();
            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();
            builder.AddCorsPolicyCache<CorsPolicyService>();
            return builder;
        }

        /// <summary>
        /// Configures implementation of IPersistedGrantStore with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            Action<TokenCleanupOptions> tokenCleanUpOptionsAction = null)
        {
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptionsAction?.Invoke(tokenCleanupOptions);
            builder.Services.AddSingleton(tokenCleanupOptions);

            return builder;
        }

        /// <summary>
        /// Adds an implementation of the IOperationalStoreNotification to IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddOperationalStoreNotification<T>(
            this IIdentityServerBuilder builder)
            where T : class, IOperationalStoreNotification
        {
            builder.Services.AddTransient<IOperationalStoreNotification, T>();
            return builder;
        }
    }
}
