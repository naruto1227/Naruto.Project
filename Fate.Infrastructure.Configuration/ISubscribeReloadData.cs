using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-21
    /// 订阅数据重载的接口
    /// </summary>
    public interface ISubscribeReloadData
    {
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync(object obj);
    }
}
