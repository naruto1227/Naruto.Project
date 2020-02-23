using Fate.Infrastructure.Redis.IRedisManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Configuration.RedisProvider
{
    /// <summary>
    /// 张海波
    /// 2020-02-23
    /// 基于redis的发布的默认实现
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
