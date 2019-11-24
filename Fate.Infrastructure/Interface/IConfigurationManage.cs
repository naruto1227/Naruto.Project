using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Interface
{
    public interface IConfigurationManage : ICommonScopeDependency
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isAppsetting"></param>
        /// <returns></returns>
        T GetValue<T>(string key, bool isAppsetting = true);
    }
}
