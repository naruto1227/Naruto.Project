using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Application.Interface;
using Fate.Common.Interface;
using Fate.Domain.Interface;
namespace Fate.Application.Services
{
    public class SettingApp : ISettingApp
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

        public async Task testEF() {
            await setting.testEF();
        }
    }
}
