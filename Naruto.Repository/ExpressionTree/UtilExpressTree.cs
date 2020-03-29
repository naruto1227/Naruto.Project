using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 表达式树的 扩展
    /// 张海波
    /// 2019-09-25
    /// </summary>
    public static class UtilExpressTree
    {

        /// <summary>
        /// 排序的扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="pre">排序的字段</param>
        /// <param name="isAscending">true默认升序 false 倒序</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> @this, TKey pre, bool isAscending = true) where T : class
        {
            //定义一个对象参数
            ParameterExpression parameter = Expression.Parameter(typeof(T), typeof(T).Name);
            ///获取对象的字段
            var fieid = Expression.Property(parameter, typeof(T).GetProperty(pre.ToString()));

            //执行升序的方法
            var asc = Expression.Call(
                //工厂对象
                typeof(Queryable),
               //方法名
               isAscending ? "OrderBy" : "OrderByDescending",
                //类型
                new Type[] { typeof(T), typeof(object) },
                //参数 一个当前元数据 一个排序条件的lambda
                @this.Expression,
                //Expression.Convert 进行类型的转换
                Expression.Lambda<Func<T, object>>(Expression.Convert(fieid, typeof(object)), new ParameterExpression[] { parameter }));
            //创建一个查询
            return (IOrderedQueryable<T>)@this.Provider.CreateQuery<T>(asc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="pre">排序的字段</param>
        /// <param name="isAscending">true默认升序 false 倒序</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> @this, TKey pre, bool isAscending = true) where T : class
        {
            //定义一个对象参数
            ParameterExpression parameter = Expression.Parameter(typeof(T), typeof(T).Name);
            ///获取对象的字段
            var fieid = Expression.Property(parameter, typeof(T).GetProperty(pre.ToString()));

            //执行升序的方法
            var asc = Expression.Call(
                //工厂对象
                typeof(Queryable),
               //方法名
               isAscending ? "ThenBy" : "ThenByDescending",
                //类型
                new Type[] { typeof(T), typeof(object) },
                //参数 一个当前元数据 一个排序条件的lambda
                @this.Expression,
                //Expression.Convert 进行类型的转换
                Expression.Lambda<Func<T, object>>(Expression.Convert(fieid, typeof(object)), new ParameterExpression[] { parameter }));
            //创建一个查询
            return (IOrderedQueryable<T>)@this.Provider.CreateQuery<T>(asc);
        }
    }
}
