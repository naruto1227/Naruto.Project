

namespace Naruto.Id4.Entities
{
    
    /// <summary>
    /// 客户端跨域配置
    /// </summary>
    public class ClientCorsOrigin : BaseMongo.Model.IMongoEntity
    {
        /// <summary>
        /// 跨域地址
        /// </summary>
        public string Origin { get; set; }

    }
}