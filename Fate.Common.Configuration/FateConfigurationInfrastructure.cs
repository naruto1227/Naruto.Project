using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Configuration
{

    /// <summary>
    /// 张海波
    /// 2019-11-21 
    /// 存储配置的key 可用于重新加载配置的时候（暂时无用）
    /// </summary>
    internal static class FateConfigurationInfrastructure
    {
        internal static ConcurrentBag<string> Keys = new ConcurrentBag<string>();

        internal static IChangeToken changeToken;
    }
}
