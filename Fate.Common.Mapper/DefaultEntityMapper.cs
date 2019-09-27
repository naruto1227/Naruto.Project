using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Fate.Common.Mapper
{
    public class DefaultEntityMapper : IEntityMapper
    {
        private readonly IMapper mapper;

        public DefaultEntityMapper(IMapper _mapper)
        {
            mapper = _mapper;
        }

        /// <summary>
        /// 传输一个对象
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="soure">数据源</param>
        /// <returns></returns>
        public T MapperTo<T>(object soure) where T : class
        {
            if (soure == null)
                return default;
            return mapper.Map<T>(soure);
        }
        /// <summary>
        /// 为已经存在的对象进行automapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public T MapperTo<T>(object soure, T result) where T : class
        {
            if (soure == null)
                return default;
            return (T)mapper.Map(soure, result, soure.GetType().UnderlyingSystemType, result.GetType());
        }
        /// <summary>
        /// 传输一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <returns></returns>
        public Task<List<T>> MapperToList<T>(object soure) where T : class
        {
            if (soure == null)
                return default;
            return Task.Run(() => mapper.Map<List<T>>(soure));
        }
    }
}
