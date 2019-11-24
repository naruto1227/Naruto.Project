using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Repository.Interface
{
    public interface ISqlQuery : ISqlCommon
    {
        #region table

        /// <summary>
        /// 执行sql的同步查询操作 (返回DataTable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        DataTable ExecuteSqlQuery(string sql, params object[] _params);

        /// <summary>
        /// 执行sql的异步查询操作 (返回DataTable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        Task<DataTable> ExecuteSqlQueryAsync(string sql, params object[] _params);
        #endregion
    }
    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的查询操作
    /// </summary>
    public interface ISqlQuery<TDbContext> : ISqlQuery, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
