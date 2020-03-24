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

        private static Func<TIn, string[], TOut> func;

        /// <summary>
        /// 传值
        /// </summary>
        /// <param name="in"></param>
        /// <returns></returns>
        public static TOut To(TIn @in, string[] ignoreName = null)
        {
            if (@in == null)
                return default;
            if (ignoreName == null)
                ignoreName = Array.Empty<string>();
            if (func != null)
                return func(@in, ignoreName);
            //定义一个输入参数
            var inParamter = Expression.Parameter(typeof(TIn), "tIn");
            //忽略的名称
            var ignoreParamter = Expression.Parameter(typeof(string[]), "ignoreName");

            List<MemberBinding> memberBindings = new List<MemberBinding>();
            //遍历属性
            foreach (var item in typeof(TOut).GetProperties())
            {
                var contains = Expression.Call(typeof(Enumerable), "Contains", new Type[] {
                    typeof(string)
                }, new Expression[] {
                    ignoreParamter,
                    Expression.Constant(item.Name)
                });

                var memberBind = Expression.Bind(item,
                    Expression.Condition(//校检忽略的字段
                    contains,
                 CheckType(item.PropertyType),
                     Expression.Property(inParamter, typeof(TIn).GetProperty(item.Name))
                    ));

                memberBindings.Add(memberBind);
            }
            //遍历字段
            foreach (var item in typeof(TOut).GetFields())
            {
                var contains = Expression.Call(typeof(Enumerable), "Contains", new Type[] {
                    typeof(string)
                }, new Expression[] {
                    ignoreParamter,
                    Expression.Constant(item.Name)
                });

                var memberBind = Expression.Bind(
                    item,
                    Expression.Condition(//校检忽略的字段
                        contains,
                        CheckType(item.FieldType),
                        Expression.Field(inParamter, typeof(TIn).GetField(item.Name))
                        )
                    );

                memberBindings.Add(memberBind);
            }
            //初始化对象
            var init = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindings);

            var lambda = Expression.Lambda<Func<TIn, string[], TOut>>(init, new ParameterExpression[] {
                 inParamter,
                 ignoreParamter
            });

            func = lambda.Compile();

            return func(@in, ignoreName);
        }


        /// <summary>
        /// 校检类型 返回对应的表达式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Expression CheckType(Type type)
        {
            if (type == typeof(int))
                return Expression.Constant(0);
            else if (type == typeof(string))
                return Expression.Constant("");
            else if (type == typeof(long))
                return Expression.Constant((long)0);
            else if (type == typeof(float))
                return Expression.Constant((float)0);
            else if (type == typeof(double))
                return Expression.Constant((double)0);
            else if (type == typeof(decimal))
                return Expression.Constant((decimal)0);
            else if (type == typeof(DateTime))
                return Expression.Constant(DateTime.Parse("0001/1/1 0:00:00"));
            else if (type == typeof(bool))
                return Expression.Constant(false);
            else if (type == typeof(short))
                return Expression.Constant((short)0);
            else if (type == typeof(byte))
                return Expression.Constant((byte)0);
            else if (type == typeof(byte[]))
                return Expression.Constant(new byte[] { });
            else if (type == typeof(uint))
                return Expression.Constant((uint)0);
            else if (type == typeof(ulong))
                return Expression.Constant((ulong)0);
            else if (type == typeof(char))
                return Expression.Constant(' ');
            else if (type == typeof(Guid))
                return Expression.Constant(Guid.Empty);
            else if (type == typeof(object))
                return Expression.Constant(default);
            else
                throw new InvalidOperationException($"{nameof(ExpressionMapper<TIn, TOut>)}:未处理的类型【{type.FullName}】");
        }
    }
}
