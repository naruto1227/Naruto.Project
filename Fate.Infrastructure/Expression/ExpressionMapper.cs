using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Fate.Test.ExpressTree
{
    /// <summary>
    /// 对象关系映射
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class ExpressionMapper<TIn, TOut> where TIn : class where TOut : class
    {

        private static Func<TIn, TOut> func;

        /// <summary>
        /// 传值
        /// </summary>
        /// <param name="in"></param>
        /// <returns></returns>
        public static TOut To(TIn @in)
        {
            if (@in == null)
                return default;

            if (func != null)
                return func(@in);
            //定义一个输入参数
            var paramter = Expression.Parameter(typeof(TIn), "a");

            List<MemberBinding> memberBindings = new List<MemberBinding>();
            //遍历属性
            foreach (var item in typeof(TOut).GetProperties())
            {
                memberBindings.Add(Expression.Bind(item, Expression.Property(paramter, typeof(TIn).GetProperty(item.Name))));
            }
            //遍历字段
            foreach (var item in typeof(TOut).GetFields())
            {
                memberBindings.Add(Expression.Bind(item, Expression.Field(paramter, typeof(TIn).GetField(item.Name))));
            }
            //初始化对象
            var init = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindings);

            var lambda = Expression.Lambda<Func<TIn, TOut>>(init, new ParameterExpression[] {
                 paramter
            });

            func = lambda.Compile();

            return func(@in);
        }

    }
}
