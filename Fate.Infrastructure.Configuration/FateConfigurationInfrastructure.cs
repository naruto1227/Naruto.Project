using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Fate.Infrastructure.Configuration
{

    /// <summary>
    /// 张海波
    /// 2019-11-21 
    /// 存储配置的key 可用于重新加载配置的时候（暂时无用，因为一般来说热更新只是更新存在的字段）
    /// </summary>
    internal static class FateConfigurationInfrastructure
    {
        internal static ConcurrentBag<string> Keys = new ConcurrentBag<string>();

        internal static IChangeToken changeToken;
        /// <summary>
        /// 订阅的key
        /// </summary>
        internal static string SubscribeKey = "changeConfiguration";

    }
}
