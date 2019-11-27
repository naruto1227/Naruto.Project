using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration
{
    /// <summary>
    /// 张海波
    /// 2019-11-28
    /// 发布消息通知订阅方
    /// </summary>
    public interface IConfigurationPublish
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        Task PublishAsync();

    }
}
