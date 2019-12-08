using Ocelot.Configuration;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using Fate.Infrastructure.Redis.IRedisManage;
using Newtonsoft.Json;
using Ocelot.Middleware.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Creator;

namespace Fate.Infrastructure.OcelotStore.RedisProvider
{
    /// <summary>
    /// 张海波
    /// 2019-08-16
    /// 重写内部的基本配置的仓储(网关的每次的调用都会触发当前类 来获取路由的信息)
    /// 用单例注册
    /// </summary>
    public class RedisInternalConfigurationRepository : IInternalConfigurationRepository
    {

        private static readonly object LockObject = new object();
        /// <summary>
        /// 获取redis对象
        /// </summary>
        private IRedisOperationHelp redis;

        /// <summary>
        /// 存储到redis的缓存key
        /// </summary>
        private readonly string RedisCacheKey = "Ocelot:InternalConfiguration";

        private IServiceProvider serviceProvider;

        public RedisInternalConfigurationRepository(IRedisOperationHelp _redis, IServiceProvider _serviceProvider)
        {
            redis = _redis;
            serviceProvider = _serviceProvider;
        }

        /// <summary>
        /// 将数据替换掉
        /// </summary>
        /// <param name="internalConfiguration"></param>
        /// <returns></returns>
        public Response AddOrReplace(IInternalConfiguration internalConfiguration)
        {
            lock (LockObject)
            {
                //存储基本的配置
                redis.RedisString().Set(RedisCacheKey, internalConfiguration);
            }
            return new OkResponse();
        }
        /// <summary>
        /// 获取配置的基本信息
        /// </summary>
        /// <returns></returns>
        public Response<IInternalConfiguration> Get()
        {
            //InternalConfiguration
            lock (LockObject)
            {
                //获取基本的配置
                var internalConfiguration = redis.RedisString().Get<InternalConfiguration>(RedisCacheKey);

                if (internalConfiguration == null)
                {
                    //获取所有配置服务的仓储
                    var fileConfigRepository = serviceProvider.GetService<IFileConfigurationRepository>();
                    //获取创建配置的服务(创建一个基本的配置数据 包含路由等数据)
                    var configCreator = serviceProvider.GetService<IInternalConfigurationCreator>();
                    //重新设置缓存
                    AddOrReplace(configCreator.Create(fileConfigRepository.Get().GetAwaiter().GetResult().Data).GetAwaiter().GetResult().Data);

                    //重新获取数据
                    internalConfiguration = redis.RedisString().Get<InternalConfiguration>(RedisCacheKey);
                }

                return new OkResponse<IInternalConfiguration>(internalConfiguration);
            }
        }
    }
}
