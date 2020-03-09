

namespace Fate.Infrastructure.Id4.Entities
{
    [NoCollection]
    /// <summary>
    /// 客户端声明
    /// </summary>
    public class ClientClaim : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 声明的类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 声明的值
        /// </summary>
        public string Value { get; set; }

    }
}