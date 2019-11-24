using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace System.Linq
{
    public static class LINQToDataTable
    {
        /// <summary>
        /// 将List转换为Datatable
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="list">数据源</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            if (list == null) return default;

            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            System.Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
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