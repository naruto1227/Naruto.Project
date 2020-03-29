using Naruto.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Naruto.Repository.Object;
using System.Threading;

namespace Naruto.Repository.Base
{
    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的从库查询操作
    /// </summary>
    public class SqlQuery<TDbContext> : SqlQueryAbstract, ISqlQuery<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlQuery(IRepositoryReadInfrastructure<TDbContext> _infrastructure, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions) : base(_infrastructure, _unitOfWorkOptions)
        {
        }
    }

    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的主库查询操作
    /// </summary>
    public class SqlMasterQuery<TDbContext> : SqlQueryAbstract, ISqlMasterQuery<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlMasterQuery(IRepositoryWriteInfrastructure<TDbContext> _infrastructure, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions) : base(_infrastructure, _unitOfWorkOptions)
        {
        }
    }


    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的抽象查询操作
    /// </summary>
    public abstract class SqlQueryAbstract : ISqlQuery
    {
        /// <summary>
        /// 获取基础设施
        /// </summary>
        private readonly IRepositoryInfrastructure infrastructure;

        /// <summary>
        /// 工作单元参数设置
        /// </summary>
        private readonly UnitOfWorkOptions unitOfWorkOptions;


        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlQueryAbstract(IRepositoryInfrastructure _infrastructure, UnitOfWorkOptions _unitOfWorkOptions)
        {
            infrastructure = _infrastructure;
            unitOfWorkOptions = _unitOfWorkOptions;
        }
        #region 同步
        /// <summary>
        /// 返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteSqlQuery(string sql, object[] _params = default)
        {
            return ExecuteSqlQueryAsync(sql, _params).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 执行sql返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string sql, object[] _params = default)
        {
            return ExecuteScalarAsync<T>(sql, _params).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        #endregion

        #region 异步

        /// <summary>
        /// 执行sql的异步查询操作 (返回DataTable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public virtual async Task<DataTable> ExecuteSqlQueryAsync(string sql, object[] _params = default, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection(cancellationToken);
            //执行脚本
            return await ExecCommand(connection, async command =>
            {
                //执行返回
                DataTable dataTable = new DataTable();
                dataTable.Load((await command.ExecuteReaderAsync().ConfigureAwait(false)));
                return dataTable;
            }, sql, _params);
        }
        /// <summary>
        /// 执行sql返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public virtual async Task<T> ExecuteScalarAsync<T>(string sql, object[] _params = default, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection(cancellationToken);
            ///执行命令
            return await ExecCommand(connection, async command =>
            {
                var res = (await command.ExecuteScalarAsync().ConfigureAwait(false));
                if (res == null || res.GetType() == typeof(DBNull))
                    return default;
                return (T)res;
            }, sql, _params);
        }
        #endregion

        #region base

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<DbConnection> GetConnection(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //获取连接
            var connection = infrastructure.Exec(repository => repository.Database.GetDbConnection());
            //验证连接是否开启
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
            }
            return connection;
        }

        /// <summary>
        /// 检查sql语句
        /// <param name="sql"></param>
        protected virtual void CheckSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException(nameof(sql));
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        protected virtual TResult ExecCommand<TResult>(DbConnection dbConnection, Func<DbCommand, TResult> func, string sql, object[] _params)
        {
            //创建一个命令
            var command = dbConnection.CreateCommand();
            command.CommandText = sql;
            if (_params != null && _params.Count() > 0)
                command.Parameters.AddRange(_params);
            //设置超时时间
            if (unitOfWorkOptions.CommandTimeout != null)
            {
                int.TryParse(unitOfWorkOptions.CommandTimeout.ToString(), out var commandTimeout);
                command.CommandTimeout = commandTimeout;
            }
            //执行命令
            return func.Invoke(command);
        }


        #endregion
    }
}
