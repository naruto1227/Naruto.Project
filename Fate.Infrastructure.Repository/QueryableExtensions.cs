
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// linq查询扩展
    /// </summary>
    public static class QueryableExtensions
    {
        private const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// 将linq转换成sql 返回sql和参数(EFCore 3.0) 
        /// 第一个字符串为替换过 参数的sql字符串
        /// 第二个字符串为原始的sql字符串
        /// 第三个参数为参数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static (string, string, IReadOnlyDictionary<string, object>) ToSqlWithParams<TEntity>(this IQueryable<TEntity> query)
        {
            var enumerator = query.Provider
                .Execute<IEnumerable<TEntity>>(query.Expression)
                .GetEnumerator();

            var enumeratorType = enumerator.GetType();
            //获取查询表达式列
            var selectFieldInfo = enumeratorType.GetField("_selectExpression", bindingFlags) ?? throw new InvalidOperationException($"cannot find field _selectExpression on type {enumeratorType.Name}");
            //获取sql工厂
            var sqlGeneratorFieldInfo = enumeratorType.GetField("_querySqlGeneratorFactory", bindingFlags) ?? throw new InvalidOperationException($"cannot find field _querySqlGeneratorFactory on type {enumeratorType.Name}");
            //查询上下文
            var queryContextFieldInfo = enumeratorType.GetField("_relationalQueryContext", bindingFlags) ?? throw new InvalidOperationException($"cannot find field _relationalQueryContext on type {enumeratorType.Name}");
            //获取查询表达式列
            var selectExpression = selectFieldInfo.GetValue(enumerator) as SelectExpression ?? throw new InvalidOperationException($"could not get SelectExpression");
            //获取sql工厂
            var factory = sqlGeneratorFieldInfo.GetValue(enumerator) as IQuerySqlGeneratorFactory ?? throw new InvalidOperationException($"could not get SqlServerQuerySqlGeneratorFactory");
            //查询上下文
            var queryContext = queryContextFieldInfo.GetValue(enumerator) as RelationalQueryContext ?? throw new InvalidOperationException($"could not get RelationalQueryContext");
            //创建一个查询的对象
            var sqlGenerator = factory.Create();
            //获取执行的命令
            var command = sqlGenerator.GetCommand(selectExpression);
            //执行sql传递的参数
            var parametersDict = queryContext.ParameterValues;
            //原始的sql执行文本
            var sql = command.CommandText;
            //将参数赋值转换
            var converSql = sql;

            if (parametersDict != null && parametersDict.Count() > 0)
            {
                foreach (var item in parametersDict)
                {
                    converSql = converSql.Replace($"@{item.Key}", $"'{item.Value.ToString()}'");
                }
            }
            return (converSql, sql, parametersDict);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">展示的数量</param>
        /// <returns></returns>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (query == null)
                return null;
            if (page < 1)
                page = 1;
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="query">源数据</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (query == null)
                return null;
            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            if (query == null)
                return query;
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            if (query == null)
                return null;
            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            if (query == null)
                return query;
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }
    }
}