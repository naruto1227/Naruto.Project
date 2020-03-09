

using System;
using System.Linq;
using System.Threading.Tasks;
using Fate.Infrastructure.Id4.MongoDB;
using Fate.Infrastructure.MongoDB.Base;
using Fate.Infrastructure.MongoDB.Interface;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Entities = Fate.Infrastructure.Id4.Entities;
using MongoDB.Driver.Linq;
using Fate.Infrastructure.Id4.Entities.Mappers;

namespace Fate.Infrastructure.Id4.MongoDB.Stores
{
    /// <summary>
    /// Implementation of IClientStore thats uses MongoDB.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IClientStore" />
    public class ClientStore : IClientStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        private readonly IMongoRepository<IdentityServerMongoContext> Context;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ClientStore> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ClientStore(IMongoRepository<IdentityServerMongoContext> context, ILogger<ClientStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>
        /// The client
        /// </returns>
        public virtual async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await Context.Query<Entities.Client>().AsQueryable().Where(a => a.ClientId == clientId).FirstOrDefaultAsync();
            var model = client.ToModel();
            Logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, model != null);
            return model;
        }
    }
}