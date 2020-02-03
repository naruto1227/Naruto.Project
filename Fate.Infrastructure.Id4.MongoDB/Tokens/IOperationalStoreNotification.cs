using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = Fate.Infrastructure.Id4.Entities;
namespace Fate.Infrastructure.Id4.MongoDB.Tokens
{
    /// <summary>
    /// Interface to model notifications from the TokenCleanup feature.
    /// </summary>
    public interface IOperationalStoreNotification
    {
        /// <summary>
        /// Notification for persisted grants being removed.
        /// </summary>
        /// <param name="persistedGrants"></param>
        /// <returns></returns>
        Task PersistedGrantsRemovedAsync(IEnumerable<Entities.PersistedGrant> persistedGrants);
    }
}