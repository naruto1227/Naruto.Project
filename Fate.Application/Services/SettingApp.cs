using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Application.Interface;
using Fate.Domain.Model;
using Fate.Domain.Interface;
using Fate.Common.Base.Model;

namespace Fate.Application.Services
{
    public class SettingApp : IAppServicesDependency
    {
        private ISetting setting;
        public SettingApp(ISetting _setting)
        {
            setting = _setting;
        }
        public async Task add(IEntity info)
        {
            await setting.add(info);
        }

        public async Task EventTest()
        {
            await setting.EventTest();
        }

        public async Task testEF()
        {
            await setting.testEF();
        }
    }
}
