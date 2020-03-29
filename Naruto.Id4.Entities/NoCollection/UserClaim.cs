
namespace Naruto.Id4.Entities
{

    
    public abstract class UserClaim : BaseMongo.Model.IMongoEntity
    {
        public string Type { get; set; }
    }
}