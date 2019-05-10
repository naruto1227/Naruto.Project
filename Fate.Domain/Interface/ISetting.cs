using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Interface;
namespace Fate.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISetting: IDomainServicesAutoInject
    {
        Task add(IEntity info);
        Task EventTest();
        Task testEF();
    }
}
