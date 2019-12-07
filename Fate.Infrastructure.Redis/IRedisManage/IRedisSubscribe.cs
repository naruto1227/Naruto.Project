using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.IRedisManage
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// 发布订阅
    /// </summary>
    public interface IRedisSubscribe:IRedisDependency
    {
        #region 同步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        void Subscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        long Publish(RedisChannel channel, RedisValue message = default, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        void Unsubscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        void UnsubscribeAll(CommandFlags flags = CommandFlags.None);
        #endregion
        #region 异步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        Task SubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        Task<long> PublishAsync(RedisChannel channel, RedisValue message = default, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        Task UnsubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None);
        #endregion
    }
}
