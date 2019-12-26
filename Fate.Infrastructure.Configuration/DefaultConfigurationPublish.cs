using Fate.Infrastructure.Redis.IRedisManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration
{
    /// <summary>
    /// 发布的默认实现
    /// </summary>
    public class DefaultConfigurationPublish : IConfigurationPublish
    {

        private readonly IRedisOperationHelp redis;

        public DefaultConfigurationPublish(IRedisOperationHelp _redis)
        {
            redis = _redis;
        }
        public async Task PublishAsync()
        {
            await redis.RedisSubscribe().PublishAsync(FateConfigurationInfrastructure.SubscribeKey, "更新配置").ConfigureAwait(false);
        }
    }
}
