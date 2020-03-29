using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Redis.IRedisManage
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// Store
    /// </summary>

    public interface IRedisStore : IRedisDependency
    {
        #region Store

        /// <summary>
        /// 保存一个集合 （事务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        bool StoreAll<T>(List<T> list);

        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        bool Store<T>(T info);
        /// <summary>
        /// 删除所有的
        /// </summary>
        bool DeleteAll<T>();

        /// <summary>
        /// 移除 单个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteById<T>(string id);
        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteByIds<T>(List<string> ids);
        /// <summary>
        /// 获取所有的集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAll<T>();

        /// <summary>
        /// 获取单个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(int id);
        /// <summary>
        /// 获取多个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        List<T> GetByIds<T>(List<int> ids);

        #endregion
    }
}
