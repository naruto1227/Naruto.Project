using Fate.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Fate.Infrastructure.Repository.Object;
using System.Threading;

namespace Fate.Infrastructure.Repository.Base
{
    /// <summary>
    /// 张海波
    /// 2019-10-25
    /// 执行SQL语句的增删改操作
    /// </summary>
    public class SqlCommand<TDbContext> : ISqlCommand<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 获取读写的基础设施
        /// </summary>
        private readonly IRepositoryWriteInfrastructure<TDbContext> infrastructure;
        /// <summary>
        /// 工作单元参数设置
        /// </summary>
        private readonly UnitOfWorkOptions<TDbContext> unitOfWorkOptions;
        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlCommand(IRepositoryWriteInfrastructure<TDbContext> _infrastructure, UnitOfWorkOptions<TDbContext> _unitOfWorkOptions)
        {
            infrastructure = _infrastructure;
            unitOfWorkOptions = _unitOfWorkOptions;
        }
        /// <summary>
        /// 执行sql 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, object[] _params = default)
        {
            return ExecuteNonQueryAsync(sql, _params).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 执行sql返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object[] _params = default)
        {
            return ExecuteScalarAsync<T>(sql, _params).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, object[] _params = default, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection(cancellationToken);
            //执行命令
            return await ExecCommand(connection, async command => await command.ExecuteNonQueryAsync(cancellationToken), sql, _params).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行sql返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object[] _params = default, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection(cancellationToken);
            //执行命令
            return await ExecCommand(connection, async command =>
              {
                  var res = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                  if (res == null)
                      return default;
                  return (T)res;
              }, sql, _params);
        }
        #region base
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        private async Task<DbConnection> GetConnection(CancellationToken cancellationToken = default)
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
        private void CheckSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException(nameof(sql));
        }

        /// <summary>
        /// 创建sqlcommand
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        private TResult ExecCommand<TResult>(DbConnection dbConnection, Func<DbCommand, TResult> func, string sql, object[] _params)
        {
            //创建一个命令
            var command = dbConnection.CreateCommand();
            command.CommandText = sql;
            if (_params != null && _params.Count() > 0)
                command.Parameters.AddRange(_params);
            //获取当前执行的事务
            var currentTransaction = infrastructure.Exec(repository => repository.Database.CurrentTransaction);
            //判断是否开启了事务
            if (currentTransaction != null)
                //绑定事务
                command.Transaction = currentTransaction.GetDbTransaction();
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
