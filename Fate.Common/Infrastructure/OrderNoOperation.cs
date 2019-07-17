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
        private NLogHelper nLog;
        private IRedisOperationHelp redis;
        //定义获取服务的实例
        private IServiceProvider serviceProvider;
        public OrderNoOperation(IRedisOperationHelp _reids, IServiceProvider _serviceProvider, NLogHelper _nLog)
        {
            redis = _reids;
            serviceProvider = _serviceProvider;
            nLog = _nLog;
        }
        /// <summary>
        /// 获取单号的数量
        /// </summary>
        /// <returns></returns>
        private long DayLength(string tableName = default)
        {
            var key = StaticFieldConfig.OrderNOByDayCacheList + tableName;
            if (!redis.KeyExists(key, Redis.KeyOperatorEnum.List))
            {
                return 0;
            }
            return redis.ListLength(key);

        }
        /// <summary>
        /// 获取月单号的数量
        /// </summary>
        /// <returns></returns>
        private long MonthLength(string tableName = default)
        {
            var key = StaticFieldConfig.OrderNOByMonthCacheList + tableName;
            if (!redis.KeyExists(key, Redis.KeyOperatorEnum.List))
            {
                return 0;
            }
            return redis.ListLength(key);
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
                catch (Exception ex) { nLog.Error(ex.Message); }
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
                //遍历表名
                foreach (var item in OrderNOConfig.TableNameList)
                {
                    #region 生成单号
                    //缓存的key
                    var key = StaticFieldConfig.OrderNOByDayCacheList + item;

                    //验证缓存中的key的数量 是否达到最小约束
                    if (DayLength(item) > StaticFieldConfig.OrderNOMinLength)
                        return;
                    //从数据库中获取最后的一个单号
                    var resNO = unitOfWork.Respositiy<OrderNo>().AsQueryable().OrderByDescending(a => a.Date).ThenByDescending(a => a.NO).Where(a => a.Date == date && a.TableName.Equals(item)).Select(a => a.NO).FirstOrDefault();
                    //获取集合实例
                    List<OrderNo> addLi = service.ServiceProvider.GetRequiredService<List<OrderNo>>();
                    if (addLi.Count() > 0)
                        addLi.Remove(addLi[0]);
                    //初始一个订单号
                    if (resNO <= 0)
                        resNO = StaticFieldConfig.FirstOrderNO;
                    //追加新的单号
                    for (int i = 0; i < StaticFieldConfig.OrderNOMaxLength; i++)
                    {
                        resNO++;
                        var orderNO = service.ServiceProvider.GetRequiredService<OrderNo>();
                        orderNO.NO = resNO;
                        orderNO.Date = date;
                        orderNO.TableName = item;
                        //添加数据到数据库
                        addLi.Add(orderNO);
                    }
                    if (addLi != null && addLi.Count() > 0)
                    {
                        //追加到数据库
                        unitOfWork.Respositiy<OrderNo>().BulkAdd(addLi);
                        var res = unitOfWork.SaveChanges();
                        if (res > 0)
                        {
                            //追加到缓存
                            redis.ListRightPush(key, addLi.Select(a => a.NO).ToList());
                        }
                    }
                    #endregion
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
                //遍历表名
                foreach (var item in OrderNOConfig.TableNameList)
                {
                    //缓存的key
                    var key = StaticFieldConfig.OrderNOByMonthCacheList + item;
                    //验证缓存中的key的数量 是否达到最小约束
                    if (MonthLength(item) > StaticFieldConfig.OrderNOMinLength)
                        return;
                    //从数据库中获取最后的一个单号
                    var resNO = unitOfWork.Respositiy<OrderNo>().AsQueryable().OrderByDescending(a => a.Date).ThenByDescending(a => a.NO).Where(a => a.Date == date && a.TableName.Equals(item)).Select(a => a.NO).FirstOrDefault();
                    //获取集合实例
                    List<OrderNo> addLi = service.ServiceProvider.GetRequiredService<List<OrderNo>>();
                    if (addLi.Count() > 0)
                        addLi.Remove(addLi[0]);
                    //初始一个订单号
                    if (resNO <= 0)
                        resNO = StaticFieldConfig.FirstOrderNO;
                    //验证单号
                    for (int i = 0; i < StaticFieldConfig.OrderNOMaxLength; i++)
                    {
                        resNO++;
                        var orderNO = service.ServiceProvider.GetRequiredService<OrderNo>();
                        orderNO.NO = resNO;
                        orderNO.Date = date;
                        orderNO.TableName = item;
                        //添加数据到数据库
                        addLi.Add(orderNO);
                    }
                    if (addLi != null && addLi.Count() > 0)
                    {
                        //追加到数据库
                        unitOfWork.Respositiy<OrderNo>().BulkAdd(addLi);
                        var res = unitOfWork.SaveChanges();
                        if (res > 0)
                        {
                            //追加到缓存
                            redis.ListRightPush(key, addLi.Select(a => a.NO).ToList());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据天生成单号 (单号生成规则 日期 +序号) 返回单号
        /// </summary>
        public async Task<string> GenerateOrderNOByDay(string tableName = default)
        {
            //当缓存中的单号没有的话 就读取雪花算法的单号
            var key = StaticFieldConfig.OrderNOByDayCacheList + tableName;
            if (!(await redis.KeyExistsAsync(key, Redis.KeyOperatorEnum.List)) && (await redis.ListLengthAsync(key)) <= 0)
                return SnowFlakeHelper.NewID().ToString();
            //获取单号
            var no = await redis.ListLeftPopAsync(key);
            //拼接单号
            return DateTime.Now.ToString("yyMMdd") + no;
        }

        /// <summary>
        /// 根据月生成单号 (单号生成规则 日期 +序号) 返回单号
        /// </summary>
        public async Task<string> GenerateOrderNOByMonth(string tableName = default)
        {
            //当缓存中的单号没有的话 就读取雪花算法的单号
            var key = StaticFieldConfig.OrderNOByMonthCacheList + tableName;
            if (!(await redis.KeyExistsAsync(key, Redis.KeyOperatorEnum.List)) && (await redis.ListLengthAsync(key)) <= 0)
                return SnowFlakeHelper.NewID().ToString();
            //获取单号
            var no = await redis.ListLeftPopAsync(key);
            //拼接单号
            return DateTime.Now.ToString("yyMM") + no;
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
                        //获取
                        var nowDate = Convert.ToInt32(date.ToString("yyMM"));
                        unitOfWork.Respositiy<OrderNo>().Delete(a => a.Date != nowDate);
                        unitOfWork.SaveChanges();
                        //移除缓存
                        string key = StaticFieldConfig.OrderNOByMonthCacheList.Replace(nowDate.ToString(), date.AddMonths(-1).ToString("yyMM"));
                        redis.KeyRemove(key, Redis.KeyOperatorEnum.List);
                    }
                    //凌晨的时候清理非今天的数据
                    if (date.Hour < 5)
                    {
                        //从容器中获取实例
                        var unitOfWork = service.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        //获取
                        var nowDate = Convert.ToInt32(date.ToString("yyMMdd"));
                        unitOfWork.Respositiy<OrderNo>().Delete(a => a.Date != nowDate);
                        unitOfWork.SaveChanges();
                        //移除缓存
                        string key = StaticFieldConfig.OrderNOByDayCacheList.Replace(nowDate.ToString(), date.AddDays(-1).ToString("yyMMdd"));
                        redis.KeyRemove(key, Redis.KeyOperatorEnum.List);
                    }
                }
            }
            catch (Exception ex) { nLog.Error(ex.Message); }
        }
    }
}
