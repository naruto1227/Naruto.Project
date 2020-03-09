
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fate.Infrastructure.Id4.MongoDB;
using Fate.Infrastructure.MongoDB.Interface;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Entities = Fate.Infrastructure.Id4.Entities;
using Fate.Infrastructure.Id4.Entities.Mappers;
namespace Fate.Infrastructure.Id4.MongoDB.Stores
{
    /// <summary>
    /// Implementation of IResourceStore thats uses MongoDB.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IResourceStore" />
    public class ResourceStore : IResourceStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly IMongoRepository<IdentityServerMongoContext> Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ResourceStore> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ResourceStore(IMongoRepository<IdentityServerMongoContext> context, ILogger<ResourceStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var api = await Context.Query<Entities.ApiResource>().FirstOrDefaultAsync(x => x.Name == name);
            return api.ToModel();
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var results = await Context.Query<Entities.ApiResource>().ToListAsync(x => scopeNames.Contains(x.Name));
            var models = results.Select(x => x.ToModel()).ToArray();

            Logger.LogDebug("Found {scopes} API scopes in database", models.SelectMany(x => x.Scopes).Select(x => x.Name));

            return models;
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();

            var results = await Context.Query<Entities.IdentityResource>().ToListAsync(x => scopeNames.Contains(x.Name));

            Logger.LogDebug("Found {scopes} identity scopes in database", results.Select(x => x.Name));

            return results.Select(x => x.ToModel()).ToArray();
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(
                (await Context.Query<Entities.IdentityResource>().ToListAsync(x => true)).Select(x => x.ToModel()),
                (await Context.Query<Entities.ApiResource>().ToListAsync(x => true)).Select(x => x.ToModel())
            );

            Logger.LogDebug("Found {scopes} as all scopes in database", result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name)));

            return result;
        }
    }
}