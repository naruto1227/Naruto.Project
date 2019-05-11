using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Domain.Model;
namespace Fate.Domain.Interface
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
