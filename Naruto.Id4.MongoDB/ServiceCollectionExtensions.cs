using Naruto.Id4.MongoDB.Options;
using Naruto.Id4.MongoDB.Services;
using Naruto.Id4.MongoDB.Stores;
using Naruto.Id4.MongoDB.Tokens;
using Naruto.MongoDB.Object;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Id4.MongoDB
{
    public static class MongoDBServiceCollectionExtensions
    {
        /// <summary>
        /// 注入对象存储   使用mongodb实现
        /// </summary>
        public static IIdentityServerBuilder AddMongoDBConfigurationStore(
            this IIdentityServerBuilder builder, Action<IdentityServerMongoContext> context)
        {
            //注入mongo存储
            builder.Services.AddMongoServices()
                .AddMongoContext(context);

            //注入替换的服务
            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddCorsPolicyService<CorsPolicyService>();
            builder.AddPersistedGrantStore<PersistedGrantStore>();
            builder.AddDeviceFlowStore<DeviceFlowStore>();

            return builder;
        }

        ///// <summary>
        ///// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        ///// </summary>
        //public static IIdentityServerBuilder AddMongoDBConfigurationStoreCache(
        //    this IIdentityServerBuilder builder)
        //{
        //    builder.AddInMemoryCaching();
        //    // add the caching decorators
        //    builder.AddClientStoreCache<ClientStore>();
        //    builder.AddResourceStoreCache<ResourceStore>();
        //    builder.AddCorsPolicyCache<CorsPolicyService>();
        //    return builder;
        //}

        /// <summary>
        /// 使用IdentityServer配置IPersistedGrantStore的实现。
        /// </summary>
        public static IIdentityServerBuilder AddMongoDBOperationalStore(
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
        /// 注入授权信息移除的通知接口
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
