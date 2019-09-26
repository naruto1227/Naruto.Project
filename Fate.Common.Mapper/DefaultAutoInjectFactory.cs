using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Fate.Common.Mapper.Attributes;

using System.Linq.Expressions;
namespace Fate.Common.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultAutoInjectFactory : IAutoInjectFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public IReadOnlyList<MapperOptions> GetFromAssemblys(params Type[] types)
        {
            if (types == null || types.Count() <= 0)
                return default;

            IReadOnlyList<MapperOptions> readOnlyList = default;

            foreach (var item in types)
            {
                Assembly assembly = Assembly.Load(item.Assembly.FullName);
                //获取所有的标记了特性的类型
                var autoInjectType = assembly.GetTypes().Where(a => a.GetCustomAttribute(typeof(AutoInjectDtoAttribute)) != null).ToList();
                if (autoInjectType != null && autoInjectType.Count() > 0)
                {
                    List<MapperOptions> list = new List<MapperOptions>();
                    autoInjectType.ForEach(itemType =>
                    {
                        //实例化对象 获取特性 的数据
                        MapperOptions mapperOptions = GetAutoInjectAttr(itemType);
                        mapperOptions.IgnoreName = GetFieldIgnoreAttrName(itemType);
                        list.Add(mapperOptions);
                    });
                    readOnlyList = list;
                }
            }

            return readOnlyList;
        }
        /// <summary>
        /// 获取自动注册的特性
        /// </summary>
        /// <returns></returns>
        private MapperOptions GetAutoInjectAttr(Type itemType)
        {
            //获取特性
            var customAttrbutes = itemType.CustomAttributes.Where(a => a.AttributeType == typeof(AutoInjectDtoAttribute)).FirstOrDefault();
            if (customAttrbutes == null)
                throw new ArgumentNullException("DefaultAutoInjectFactory:参数异常");
            var soureType = customAttrbutes.NamedArguments.Where(a => a.MemberName == "SoureType").Select(a => a.TypedValue.Value).FirstOrDefault() as Type;
            var targetType = customAttrbutes.NamedArguments.Where(a => a.MemberName == "TargetType").Select(a => a.TypedValue.Value).FirstOrDefault() as Type;

            var reverseMap = customAttrbutes.NamedArguments.Where(a => a.MemberName == "ReverseMap").Select(a => a.TypedValue.Value).FirstOrDefault();

            return new MapperOptions(soureType, targetType, reverseMap);
        }

        /// <summary>
        /// 获取字段的标识为忽略的特性
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private IReadOnlyList<string> GetFieldIgnoreAttrName(Type itemType)
        {
            //获取所有的字段
            var properties = itemType.GetProperties();
            if (properties == null)
                throw new ArgumentNullException("DefaultAutoInjectFactory:无任何字段需要映射");
            //获取所有的标记了忽略的字段
            var fidles = properties.Where(a => a.GetCustomAttribute(typeof(AutoInjectIgnoreAttribute)) != null).ToList();
            if (fidles == null || fidles.Count() <= 0)
                return default;

            var list = new List<string>();
            fidles.ForEach(item => list.Add(item.Name));
            return list;
        }

    }
}
