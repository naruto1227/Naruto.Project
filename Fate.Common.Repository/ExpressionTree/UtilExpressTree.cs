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
        /// 升序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="pre">排序的字段</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T, TKey>(this IQueryable<T> @this, TKey pre) where T : class
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
                "OrderBy",
                //类型
                new Type[] { typeof(T), typeof(object) },
                //参数 一个当前元数据 一个排序条件的lambda
                @this.Expression,
                //Expression.Convert 进行类型的转换
                Expression.Lambda<Func<T, object>>(Expression.Convert(fieid, typeof(object)), new ParameterExpression[] { parameter }));

            //创建一个查询
            return @this.Provider.CreateQuery<T>(asc);
        }
        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="pre">排序的字段</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDescending<T, TKey>(this IQueryable<T> @this, TKey pre) where T : class
        {
            //定义一个对象参数
            ParameterExpression parameter = Expression.Parameter(typeof(T), typeof(T).Name);
            ///获取对象的字段
            var fieid = Expression.Property(parameter, typeof(T).GetProperty(pre.ToString()));
            //执行降序的方法
            var desc = Expression.Call(
                //工厂对象
                typeof(Queryable),
                //方法名
                "OrderByDescending",
                //类型
                new Type[] { typeof(T), typeof(object) },
                //参数 一个当前元数据 一个排序条件的lambda
                @this.Expression,
                Expression.Lambda<Func<T, object>>(Expression.Convert(fieid, typeof(object)), new ParameterExpression[] { parameter }));
            //创建一个查询
            return @this.Provider.CreateQuery<T>(desc);
        }
    }
}
