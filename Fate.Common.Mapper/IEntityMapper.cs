using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Common.Mapper
{
    /// <summary>
    /// 实体映射 对外提供的接口
    /// </summary>
    public interface IEntityMapper
    {
        /// <summary>
        /// 传输一个对象
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="soure">数据源</param>
        /// <returns></returns>
        T MapperTo<T>(object soure) where T : class;
        /// <summary>
        /// 为已经存在的对象进行automapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        T MapperTo<T>(object soure, T result) where T : class;

        /// <summary>
        /// 传输一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <returns></returns>
        Task<List<T>> MapperToListAsync<T>(object soure) where T : class;

        /// <summary>
        /// 传输一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <returns></returns>
        List<T> MapperToList<T>(object soure) where T : class;
    }
}
