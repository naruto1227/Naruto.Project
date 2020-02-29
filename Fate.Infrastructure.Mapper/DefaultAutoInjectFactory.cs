using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Fate.Infrastructure.Mapper.Attributes;

using System.Linq.Expressions;
namespace Fate.Infrastructure.Mapper
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
                var autoInjectType = assembly.GetTypes().Where(a => a.IsDefined(typeof(AutoInjectDtoAttribute))).ToList();
                if (autoInjectType != null && autoInjectType.Count() > 0)
                {
                    List<MapperOptions> list = new List<MapperOptions>();
                    autoInjectType.ForEach(itemType =>
                    {
                        //实例化对象 获取特性 的数据
                        MapperOptions mapperOptions = GetAutoInjectAttr(itemType);
                        if (mapperOptions != null)
                        {
                            mapperOptions.IgnoreName = GetFieldIgnoreAttrName(itemType);
                            list.Add(mapperOptions);
                        }
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
                return default;
            var soureType = GetAttrbutesValue(customAttrbutes, "SoureType") as Type;
            var targetType = GetAttrbutesValue(customAttrbutes, "TargetType") as Type;
            var reverseMap = GetAttrbutesValue(customAttrbutes, "ReverseMap");

            return new MapperOptions(soureType, targetType, reverseMap);
        }

        private object GetAttrbutesValue(CustomAttributeData customAttrbutes, string memberName)
        {
            return customAttrbutes.NamedArguments.Where(a => a.MemberName == memberName).Select(a => a.TypedValue.Value).FirstOrDefault();
        }
        /// <summary>
        /// 获取属性字段的标识为忽略的特性
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private IReadOnlyList<string> GetFieldIgnoreAttrName(Type itemType)
        {
            var list = new List<string>();

            //获取所有的属性
            var properties = itemType.GetProperties();
            if (properties != null && properties.Count() > 0)
            {
                //获取所有的标记了忽略的字段
                var propsList = properties.Where(a => a.IsDefined(typeof(AutoInjectIgnoreAttribute))).ToList();
                if (propsList != null && propsList.Count() > 0)
                    propsList.ForEach(item => list.Add(item.Name));
            }
            //获取字段
            var fields = itemType.GetFields();
            if (fields != null && fields.Count() > 0)
            {
                var fieldsList = fields.Where(a => a.IsDefined(typeof(AutoInjectIgnoreAttribute))).ToList();
                if (fieldsList != null && fieldsList.Count() > 0)
                    fieldsList.ForEach(item => list.Add(item.Name));
            }
            return list;
        }

    }
}
