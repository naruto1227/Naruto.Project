﻿using Fate.Infrastructure.Redis.IRedisManage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Redis.RedisManage
{
    public class DefaultRedisSubscribe : IRedisSubscribe
    {
        private readonly IRedisBase redisBase;


        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisSubscribe(IRedisBase _redisBase)
        {
            redisBase = _redisBase;
        }
        #region 同步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public void Subscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.Subscribe(chanel, handler);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        public long Publish(RedisChannel channel, RedisValue message)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            return subscriber.Publish(channel, message);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public void Unsubscribe(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.Unsubscribe(chanel, handler);
        }
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            subscriber.UnsubscribeAll();
        }
        #endregion
        #region 异步
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public async Task SubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.SubscribeAsync(chanel, handler).ConfigureAwait(false);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">被订阅的name</param>
        /// <param name="message">需要传递的参数</param>
        public async Task<long> PublishAsync(RedisChannel channel, RedisValue message)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            return await subscriber.PublishAsync(channel, message).ConfigureAwait(false);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="chanel">订阅的名称</param>
        /// <param name="handler">需要处理的事件</param>
        public async Task UnsubscribeAsync(RedisChannel chanel, Action<RedisChannel, RedisValue> handler = null)
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.UnsubscribeAsync(chanel, handler).ConfigureAwait(false);
        }
        /// <summary>
        /// 取消所有的订阅
        /// </summary>
        public async Task UnsubscribeAllAsync()
        {
            var subscriber = redisBase.RedisConnection.GetSubscriber();
            await subscriber.UnsubscribeAllAsync().ConfigureAwait(false);
        }
        #endregion
    }
}