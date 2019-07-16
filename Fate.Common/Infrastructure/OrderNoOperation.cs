using System;
using System.Collections.Generic;
using System.Text;
using Fate.Common.Redis.IRedisManage;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Fate.Common.Interface;
using Fate.Common.Config;
using Fate.Common.NLog;
using Fate.Common.Repository.Mysql.UnitOfWork;
using Fate.Domain.Model.Entities;

namespace Fate.Common.Infrastructure
{

    /// <summary>
    /// 单号的操作类
    /// </summary>
    public class OrderNoOperation : ICommonClassSigleDependency
    {
        private IRedisOperationHelp redis;
        //定义获取服务的实例
        private IServiceProvider serviceProvider;
        public OrderNoOperation(IRedisOperationHelp _reids, IServiceProvider _serviceProvider)
        {
            redis = _reids;
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// 获取单号的数量
        /// </summary>
        /// <returns></returns>
        private long DayLength
        {
            get
            {
                if (!redis.KeyExists(StaticFieldConfig.OrderNOByDayCacheList, Redis.KeyOperatorEnum.List))
                {
                    return 0;
                }
                return redis.ListLength(StaticFieldConfig.OrderNOByDayCacheList);
            }
        }
        /// <summary>
        /// 获取月单号的数量
        /// </summary>
        /// <returns></returns>
        private long MonthLength
        {
            get
            {
                if (!redis.KeyExists(StaticFieldConfig.OrderNOByMonthCacheList, Redis.KeyOperatorEnum.List))
                {
                    return 0;
                }
                return redis.ListLength(StaticFieldConfig.OrderNOByMonthCacheList);
            }
        }
        private readonly object _lock = new object();

        /// <summary>
        /// 验证单号的长度 是否低于规定的最小的长度, 低于的话 就自动追加数据
        /// </summary>
        public void FlagOrderNOLength()
        {
            lock (_lock)
            {
                try
                {
                    CreateOrderNOByDay();//按照天批量生成
                    CreateOrderNOByMonth();//按照月批量生成
                }
                catch (Exception ex) { NLogHelper.Default.Error(ex.Message); }
            }
        }
        /// <summary>
        /// 按照天来批量生成单号
        /// </summary>
        private void CreateOrderNOByDay()
        {
            //开启一个新的生命周期
            using (var service = serviceProvider.CreateScope())
            {
                //从容器中获取实例
                var unitOfWork = service.ServiceProvider.GetRequiredService<IUnitOfWork>();
                //获取当前日期
                var date = Convert.ToInt32(DateTime.Now.ToString("yyMMdd"));
                //验证当天的 单号是否生成 如果没有生成的话 就删除 缓存中的单号key
                if (!unitOfWork.Respositiy<OrderNo>().Where(a => a.Date == date).Any())
                    redis.KeyRemove(StaticFieldConfig.OrderNOByDayCacheList, Redis.KeyOperatorEnum.List);
                //验证缓存中的key的数量 是否达到最小约束
                else if (DayLength > StaticFieldConfig.OrderNOMinLength)
                    return;
                //从数据库中获取最后的一个单号
                var resNO = unitOfWork.Respositiy<OrderNo>().AsQueryable().OrderByDescending(a => a.Date).ThenByDescending(a => a.NO).Where(a => a.Date == date).Select(a => a.NO).FirstOrDefault();
                //获取集合实例
                List<OrderNo> addLi = serviceProvider.GetRequiredService<List<OrderNo>>();
                if (addLi.Count() > 0)
                    addLi.Remove(addLi[0]);
                //初始一个订单号
                if (resNO <= 0)
                    resNO = 1000;
                //追加新的单号
                for (int i = 0; i < StaticFieldConfig.OrderNOMaxLength; i++)
                {
                    resNO++;
                    //添加数据到数据库
                    addLi.Add(new OrderNo() { NO = resNO, Date = date });
                }
                if (addLi != null && addLi.Count() > 0)
                {
                    //追加到数据库
                    unitOfWork.Respositiy<OrderNo>().BulkAdd(addLi);
                    var res = unitOfWork.SaveChanges();
                    if (res > 0)
                    {
                        //追加到缓存
                        redis.ListRightPush(StaticFieldConfig.OrderNOByDayCacheList, addLi.Select(a => a.NO).ToList());
                    }
                }
            }
        }
        /// <summary>
        /// 按照月来批量生成单号
        /// </summary>
        private void CreateOrderNOByMonth()
        {
            //开启一个新的生命周期
            using (var service = serviceProvider.CreateScope())
            {
                //从容器中获取实例
                var unitOfWork = service.ServiceProvider.GetRequiredService<IUnitOfWork>();
                //获取当前日期
                var date = Convert.ToInt32(DateTime.Now.ToString("yyMM"));
                //在1号的时候 验证当月的 单号是否生成 如果没有生成的话 就删除 缓存中的单号key
                if (DateTime.Now.Day == 1 && !unitOfWork.Respositiy<OrderNo>().Where(a => a.Date == date).Any())
                    redis.KeyRemove(StaticFieldConfig.OrderNOByMonthCacheList, Redis.KeyOperatorEnum.List);
                //验证缓存中的key的数量 是否达到最小约束
                else if (MonthLength > StaticFieldConfig.OrderNOMinLength)
                    return;
                //从数据库中获取最后的一个单号
                var resNO = unitOfWork.Respositiy<OrderNo>().AsQueryable().OrderByDescending(a => a.Date).ThenByDescending(a => a.NO).Where(a => a.Date == date).Select(a => a.NO).FirstOrDefault();
                //获取集合实例
                List<OrderNo> addLi = service.ServiceProvider.GetRequiredService<List<OrderNo>>();
                if (addLi.Count() > 0)
                    addLi.Remove(addLi[0]);
                //初始一个订单号
                if (resNO <= 0)
                    resNO = 1000;
                //验证单号
                for (int i = 0; i < StaticFieldConfig.OrderNOMaxLength; i++)
                {
                    resNO++;
                    //添加数据到数据库
                    addLi.Add(new OrderNo() { NO = resNO, Date = date });
                }
                if (addLi != null && addLi.Count() > 0)
                {
                    //追加到数据库
                    unitOfWork.Respositiy<OrderNo>().BulkAdd(addLi);
                    var res = unitOfWork.SaveChanges();
                    if (res > 0)
                    {
                        //追加到缓存
                        redis.ListRightPush(StaticFieldConfig.OrderNOByMonthCacheList, addLi.Select(a => a.NO).ToList());
                    }
                }
            }
        }

        /// <summary>
        /// 根据天生成单号 (单号生成规则 日期+6位随机数 +序号) 返回单号
        /// </summary>
        public async Task<string> GenerateOrderNOByDay()
        {
            ////验证是否开启缓存 如果没有开启的话 就用雪花算法生成单号
            //if (!StaticFieldConfig.IsOpenReids)
            //    return SnowFlakeHelper.NewID().ToString();
            ////当缓存中的单号没有的话 就读取雪花算法的单号
            //if (!(await redis.KeyExistsAsync(StaticFieldConfig.OrderNOByDayCacheList, Redis.KeyOperatorEnum.LIST)) && (await redis.ListLengthAsync(StaticFieldConfig.OrderNOByDayCacheList)) <= 0)
            //    return SnowFlakeHelper.NewID().ToString();
            //获取单号
            var no = await redis.ListLeftPopAsync(StaticFieldConfig.OrderNOByDayCacheList);
            //拼接单号
            return DateTime.Now.ToString("yyMMdd") + new Random().Next(100000, 999999) + no;
        }

        /// <summary>
        /// 根据月生成单号 (单号生成规则 日期+6位随机数 +序号) 返回单号
        /// </summary>
        public async Task<string> GenerateOrderNOByMonth()
        {
            ////验证是否开启缓存 如果没有开启的话 就用雪花算法生成单号
            //if (!StaticFieldConfig.IsOpenReids)
            //    return SnowFlakeHelper.NewID().ToString();
            ////当缓存中的单号没有的话 就读取雪花算法的单号
            //if (!(await redis.KeyExistsAsync(StaticFieldConfig.OrderNOByDayCacheList, Redis.KeyOperatorEnum.LIST)) && (await redis.ListLengthAsync(StaticFieldConfig.OrderNOByMonthCacheList)) <= 0)
            //    return SnowFlakeHelper.NewID().ToString();
            //获取单号
            var no = await redis.ListLeftPopAsync(StaticFieldConfig.OrderNOByMonthCacheList);
            //拼接单号
            return DateTime.Now.ToString("yyMM") + new Random().Next(100000, 999999) + no;
        }


        /// <summary>
        /// 定时从数据库清理过时的订单数据
        /// </summary>
        public void ClearOrderNO()
        {
            try
            {
                using (var service = serviceProvider.CreateScope())
                {
                    //获取当前时间
                    var date = DateTime.Now;
                    //5号之前 清理非当月的数据
                    if (date.Day < 5 && date.Hour < 5)
                    {
                        //从容器中获取实例
                        var unitOfWork = service.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        //获取上月的日期
                        var upDate = Convert.ToInt32(date.AddMonths(-1).ToString("yyMM"));
                        unitOfWork.Respositiy<OrderNo>().Delete(a => a.Date == upDate);
                        unitOfWork.SaveChanges();
                    }
                    //凌晨的时候清理非今天的数据
                    if (date.Hour < 5)
                    {
                        //从容器中获取实例
                        var unitOfWork = service.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        //获取昨天的日期
                        var upDate = Convert.ToInt32(date.AddDays(-1).ToString("yyMMdd"));
                        unitOfWork.Respositiy<OrderNo>().Delete(a => a.Date == upDate);
                        unitOfWork.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { NLogHelper.Default.Error(ex.Message); }
        }
    }
}
