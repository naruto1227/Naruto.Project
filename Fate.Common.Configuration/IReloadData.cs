using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-21
    /// 数据重载的接口
    /// </summary>
    public interface IReloadData
    {
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <returns></returns>
        Task SubscribeReloadAsync(object obj);
    }
}
