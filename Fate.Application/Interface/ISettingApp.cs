using Fate.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Fate.Application.Interface
{
    public interface ISettingApp : IAppServicesDependency
    {
        Task add(IEntity info);

        Task EventTest();
        Task testEF();
    }
}
