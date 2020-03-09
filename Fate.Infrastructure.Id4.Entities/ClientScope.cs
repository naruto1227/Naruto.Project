

namespace Fate.Infrastructure.Id4.Entities
{
    [NoCollection]
    /// <summary>
    /// 客户端范围
    /// </summary>
    public class ClientScope : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 范围名
        /// </summary>
        public string Scope { get; set; }
    }
}