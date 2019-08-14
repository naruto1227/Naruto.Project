
using Fate.Common.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.OcelotStore.Redis
{
    public class CacheOptions
    {
        /// <summary>
        /// 存储的redis中的key （默认为 ocelot）
        /// </summary>
        public string CacheKey { get; set; } = "ocelot";

        /// <summary>
        /// ef的参数配置
        /// </summary>
        public Action<EFOptions> EFOptions { get; set; } = null;
    }
}
