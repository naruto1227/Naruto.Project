using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Fate.Common.Repository
{
    /// <summary>
    /// 表达式树的 扩展
    /// 张海波
    /// 2019-09-25
    /// </summary>
    public static class UtilExpressTree
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="pre">排序的字段</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByExtension<T, TKey>(this IQueryable<T> @this, TKey pre) where T : class
        {
            //定义一个对象参数
            ParameterExpression parameter = Expression.Parameter(typeof(T), typeof(T).Name);
            ///获取对象的字段
            var fieid = Expression.Property(parameter, typeof(T).GetProperty(pre.ToString()));
            //执行降序的方法
            var res = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(T), typeof(TKey) }, @this.Expression, Expression.Lambda<Func<T, TKey>>(fieid, new ParameterExpression[] { parameter }));
            //创建一个查询
            return @this.Provider.CreateQuery<T>(res);
        }
    }
}
