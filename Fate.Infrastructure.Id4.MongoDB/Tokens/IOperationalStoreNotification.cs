using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = Fate.Infrastructure.Id4.Entities;
namespace Fate.Infrastructure.Id4.MongoDB.Tokens
{
    /// <summary>
    /// 当授权信息移除的时候 发送通知接口
    /// </summary>
    public interface IOperationalStoreNotification
    {
        /// <summary>
        /// 通知数据已经被移除
        /// </summary>
        /// <param name="persistedGrants"></param>
        /// <returns></returns>
        Task PersistedGrantsRemovedAsync(IEnumerable<Entities.PersistedGrant> persistedGrants);
    }
}