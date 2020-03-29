using Naruto.Redis.IRedisManage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Naruto.Configuration.RedisProvider
{
    /// <summary>
    /// 张海波
    /// 2020-02-23
    /// 使用redis的订阅服务
    /// </summary>
    public class RedisSubscribeReloadData : ISubscribeReloadData
    {
        private readonly IFateConfigurationLoadAbstract fateConfigurationLoad;
        private readonly IRedisOperationHelp redis;
        private readonly IConfiguration configuration;

        public RedisSubscribeReloadData(IRedisOperationHelp _redis, IConfiguration _configuration, IFateConfigurationLoadAbstract _fateConfigurationLoad)
        {
            redis = _redis;
            configuration = _configuration;
            fateConfigurationLoad = _fateConfigurationLoad;
        }
        /// <summary>
        /// 订阅加载事件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task ReloadAsync(object obj)
        {
            await redis.RedisSubscribe().SubscribeAsync(FateConfigurationInfrastructure.SubscribeKey, async (channel, value) =>
              {
                  //重新获取数据
                  var data = await fateConfigurationLoad.LoadConfiguration().ConfigureAwait(false);
                  data.ToList().ForEach(item =>
                  {
                      configuration[item.Key] = item.Value;
                  });
              });
        }
    }
}
