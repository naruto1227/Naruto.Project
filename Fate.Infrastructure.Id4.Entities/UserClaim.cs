
namespace Fate.Infrastructure.Id4.Entities
{

    [NoCollection]
    public abstract class UserClaim : BaseMongo.Model.IMongoEntity
    {
        public string Type { get; set; }
    }
}