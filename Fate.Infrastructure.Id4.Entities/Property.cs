

namespace Fate.Infrastructure.Id4.Entities
{
    /// <summary>
    /// 属性
    /// </summary>
    [NoCollection]
    public abstract class Property : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}