using Fate.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Fate.Application.Interface
{
    public interface ISettingApp : IAppServicesAutoInject
    {
        Task add(IEntity info);

        Task EventTest();
        Task testEF();
    }
}
