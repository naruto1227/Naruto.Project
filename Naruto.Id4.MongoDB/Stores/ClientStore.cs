

using System;
using System.Linq;
using System.Threading.Tasks;
using Naruto.Id4.MongoDB;
using Naruto.MongoDB.Base;
using Naruto.MongoDB.Interface;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Entities = Naruto.Id4.Entities;
using MongoDB.Driver.Linq;
using Naruto.Id4.Entities.Mappers;

namespace Naruto.Id4.MongoDB.Stores
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