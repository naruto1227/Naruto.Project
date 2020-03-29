

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naruto.Id4.MongoDB;
using Naruto.MongoDB.Interface;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Entities = Naruto.Id4.Entities;
using Naruto.Id4.Entities.Mappers;
using System;

namespace Naruto.Id4.MongoDB.Stores
{
    /// <summary>
    /// Implementation of IPersistedGrantStore thats uses MongoDB.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IPersistedGrantStore" />
    public class PersistedGrantStore : IPersistedGrantStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly IMongoRepository<IdentityServerMongoContext> Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedGrantStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public PersistedGrantStore(IMongoRepository<IdentityServerMongoContext> context, ILogger<PersistedGrantStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        /// <summary>
        /// 新增授权信息
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual async Task StoreAsync(PersistedGrant token)
        {
            var existing = await Context.Query<Entities.PersistedGrant>().FirstOrDefaultAsync(x => x.Key == token.Key);
            if (existing == null)
            {
                Logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                var persistedGrant = token.ToEntity();
                await Context.Command<Entities.PersistedGrant>().AddAsync(persistedGrant);
            }
            else
            {
                Logger.LogDebug("{persistedGrantKey} found in database", token.Key);
                //更新实体传输
                token.UpdateEntity(existing);
                //执行命令
                await Context.Command<Entities.PersistedGrant>().UpdateAsync(x => x.Key == token.Key, new Dictionary<string, object>()
                {
                    { "Type",token.Type},
                    { "SubjectId",token.SubjectId},
                    { "ClientId",token.ClientId},
                    { "CreationTime",token.CreationTime},
                    { "Expiration",token.Expiration},
                    { "Data",token.Data}
                });
            }
        }

        /// <summary>
        /// Gets the grant.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await Context.Query<Entities.PersistedGrant>().FirstOrDefaultAsync(x => x.Key == key);
            var model = persistedGrant?.ToModel();

            Logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

            return model;
        }

        /// <summary>
        /// Gets all grants for a given subject id.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = await Context.Query<Entities.PersistedGrant>().ToListAsync(x => x.SubjectId == subjectId);
            var model = persistedGrants.Select(x => x.ToModel());

            Logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", persistedGrants.Count, subjectId);

            return model;
        }

        /// <summary>
        /// Removes the grant by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task RemoveAsync(string key)
        {
            await Context.Command<Entities.PersistedGrant>().DeleteAsync(x => x.Key == key);
        }

        /// <summary>
        /// Removes all grants for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public virtual async Task RemoveAllAsync(string subjectId, string clientId)
        {
            await Context.Command<Entities.PersistedGrant>().BulkDeleteAsync(x => x.SubjectId == subjectId && x.ClientId == clientId);
        }

        /// <summary>
        /// Removes all grants of a give type for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            await Context.Command<Entities.PersistedGrant>().BulkDeleteAsync(x => x.SubjectId == subjectId && x.ClientId == clientId && x.Type == type);
        }
    }
}