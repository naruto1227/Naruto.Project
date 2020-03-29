using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.Mapper.Extensions
{
    /// <summary>
    /// 创建mapper的时候 扩展的类
    /// 张海波
    /// 2019-09-26
    /// </summary>
    internal static class ConfigurationExtension
    {
        /// <summary>
        /// 忽略 目标对象 所有 存在于ignores中的字段
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="ignores">需要忽略的字段</param>
        /// <returns></returns>
        internal static IMappingExpression IgnoreAllExisting(this IMappingExpression mapping, IReadOnlyList<string> ignores)
        {
            if (mapping == null)
                return mapping;
            //获取需要忽略的字段
            if (ignores != null && ignores.Count() > 0)
            {
                foreach (var item in ignores)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        mapping.ForMember(item, opton => opton.Ignore());
                    }
                }
            }
            return mapping;
        }
        /// <summary>
        /// 反转 映射
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="reverseMap">true 代表反转 false 代表不反转</param>
        /// <returns></returns>
        internal static IMappingExpression ReverseMap(this IMappingExpression mapping, bool reverseMap)
        {
            if (reverseMap == false)
                return mapping;
            return mapping.ReverseMap();
        }
    }
}
