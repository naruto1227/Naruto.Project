using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Naruto.BaseRepository.Model;
using Naruto.Domain.Model;
namespace Naruto.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISetting: IDomainServicesDependency
    {
        Task add(IEntity info);
        Task EventTest();
        Task testEF();
    }
}
