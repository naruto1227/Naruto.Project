using Fate.Domain.Model;
using Fate.Common.Repository.Mysql;
using Fate.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fate.Common.Repository.Mysql.UnitOfWork;
using Fate.Domain.Event;
using Fate.Domain.Event.Infrastructure;
using Fate.Domain.Event.Events;
using Fate.Domain.Event.EventHandlers;
using Fate.Domain.Event.Infrastructure.Redis;

namespace Fate.Domain.Services
{
    public class Setting : ISetting
    {
        private IUnitOfWork unitOfWork;
        private RedisStoreEventBus redisStoreEventBus;
        public Setting(IUnitOfWork _unitOfWork, RedisStoreEventBus _redisStoreEventBus)
        {
            redisStoreEventBus = _redisStoreEventBus;
            unitOfWork = _unitOfWork;
        }

        public async Task add(IEntity info)
        {
            await unitOfWork.Respositiy<Fate.Domain.Model.Entities.setting>().AddAsync(new Fate.Domain.Model.Entities.setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unitOfWork.Respositiy<Model.Entities.test1>().AddAsync(new Model.Entities.test1() { Id = 1 });
            await unitOfWork.SaveChangeAsync();
        }
        /// <summary>
        /// 事件测试
        /// </summary>
        /// <returns></returns>
        public async Task EventTest()
        {
            //注册
            //EventBus.Default.Register(new EventHandlerTest());
            //EventBus.Default.RegisterAllFromAssembly();
            //EventBus.Default.Trigger(new EventTest() { Mesage = "触发的事件" });
            //Fate.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Register(new EventHandlerTest());
            //Fate.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Trigger(new EventTest() { Mesage = "触发的事件" });
            //Fate.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.RegisterAllFromAssembly();
            //Fate.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Trigger(new EventTest() { Mesage = "触发的事件" });
            //Fate.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.HandleFailEvent();
            await redisStoreEventBus.Register(new EventHandlerTest());
            await redisStoreEventBus.Trigger(new EventTest() { Mesage = "触发的事件" });
        }

        public async Task testEF()
        {
            //await unitOfWork.Respositiy<Fate.Domain.Model.Entities.setting>().DeleteAsync(a=>a.Id==1);

            var info = await unitOfWork.Respositiy<Fate.Domain.Model.Entities.setting>().FindAsync(a => a.Id == 6);
            info.Contact = "sssssssssss";
            await unitOfWork.Respositiy<Fate.Domain.Model.Entities.setting>().UpdateAsync(info);
            await unitOfWork.SaveChangeAsync();
        }
    }
}
