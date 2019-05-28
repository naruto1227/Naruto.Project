using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace Fate.Common.AutoMapper
{
    /// <summary>
    /// automapper 扩展
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 传输一个对象
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="soure">数据源</param>
        /// <returns></returns>
        public static T MapperTo<T>(this object soure) where T : class
        {
            if (soure == null)
                return null;
            Mapper.Reset();
            Mapper.Initialize(a => a.CreateMap(soure.GetType(), typeof(T)));
            return Mapper.Map<T>(soure);
        }
        /// <summary>
        /// 为已经存在的对象进行automapper
        /// </summary>
        /// <returns></returns>
        public static T MapperTo<T>(this object obj, T result) where T : class
        {
            if (obj == null)
                return default(T);
            Mapper.Reset();
            Mapper.Initialize(a => a.CreateMap(obj.GetType().UnderlyingSystemType, typeof(T)));
            return (T)Mapper.Map(obj, result, obj.GetType().UnderlyingSystemType, typeof(T));
        }
        /// <summary>
        /// 传输一个集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="soure">数据源</param>
        /// <returns></returns>
        public static List<T> MapperTo<T>(this IEnumerable soure) where T : class
        {
            if (soure == null)
                return null;
            Mapper.Reset();
            foreach (var item in soure)
            {
                Mapper.Initialize(a => a.CreateMap(item.GetType(), typeof(T)));
                break;
            }
            return Mapper.Map<List<T>>(soure);
        }
        /// <summary>  
        /// 将 DataTable 转为实体对象  
        /// </summary>  
        /// <typeparam name="T">目标对象类型</typeparam>  
        /// <param name="dt">数据源</param>  
        /// <returns></returns>  
        public static List<T> MapTo<T>(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return default(List<T>);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<IDataReader, List<T>>());
            var mapper = config.CreateMapper();
            return mapper.Map<IDataReader, List<T>>(dt.CreateDataReader());
        }
        /// <summary>
        /// 将List转换为Datatable
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="list">数据源</param>
        /// <returns></returns>
        public static DataTable MapTo<T>(this IEnumerable<T> list)
        {
            if (list == null) return default(DataTable);

            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            System.Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
