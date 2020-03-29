using Naruto.Domain.Model;
using Naruto.Repository;
using Naruto.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Naruto.Repository.UnitOfWork;
using Naruto.Domain.Event;
using Naruto.Domain.Event.Infrastructure;
using Naruto.Domain.Event.Events;
using Naruto.Domain.Event.EventHandlers;
using Naruto.Domain.Event.Infrastructure.Redis;
using Naruto.BaseRepository.Model;

namespace Naruto.Domain.Services
{
    public class Setting : ISetting
    {
        private IUnitOfWork<MysqlDbContent> unitOfWork;
        private RedisStoreEventBus redisStoreEventBus;
        public Setting(IUnitOfWork<MysqlDbContent> _unitOfWork, RedisStoreEventBus _redisStoreEventBus)
        {
            redisStoreEventBus = _redisStoreEventBus;
            unitOfWork = _unitOfWork;
        }

        public async Task add(IEntity info)
        {
            await unitOfWork.Command<Naruto.Domain.Model.Entities.setting>().AddAsync(new Naruto.Domain.Model.Entities.setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });

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
            //Naruto.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Register(new EventHandlerTest());
            //Naruto.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Trigger(new EventTest() { Mesage = "触发的事件" });
            //Naruto.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.RegisterAllFromAssembly();
            //Naruto.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.Trigger(new EventTest() { Mesage = "触发的事件" });
            //Naruto.Domain.Event.Infrastructure.Redis.RedisStoreEventBus.Default.HandleFailEvent();
            await redisStoreEventBus.Register(new EventHandlerTest());
            await redisStoreEventBus.Trigger(new EventTest() { Mesage = "触发的事件" });
        }

        public async Task testEF()
        {
            //await unitOfWork.Respositiy<Naruto.Domain.Model.Entities.setting>().DeleteAsync(a=>a.Id==1);

            var info = await unitOfWork.Query<Naruto.Domain.Model.Entities.setting>().FindAsync(a => a.Id == 6);
            info.Contact = "sssssssssss";
            await unitOfWork.Command<Naruto.Domain.Model.Entities.setting>().UpdateAsync(info);
            await unitOfWork.SaveChangeAsync();
        }
    }
}
