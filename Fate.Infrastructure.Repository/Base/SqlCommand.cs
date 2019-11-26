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
namespace Fate.Infrastructure.Repository.Base
{
    /// <summary>
    /// 张海波
    /// 2019-10-25
    /// 执行SQL语句的增删改操作
    /// </summary>
    public class SqlCommand<TDbContext> : ISqlCommand<TDbContext> where TDbContext : DbContext
    {
        //定义一个上下文
        private DbContext repository;

        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlCommand(IDbContextFactory factory)
        {
            repository = factory.Get<TDbContext>();
        }
        /// <summary>
        /// 执行sql 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params object[] _params)
        {
            return ExecuteNonQueryAsync(sql, _params).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 执行sql返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] _params)
        {
            return ExecuteScalarAsync<T>(sql, _params).GetAwaiter().GetResult();
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, params object[] _params)
        {
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection();
            //创建一个命令
            var command = CreateCommand(connection, sql, _params);
            return await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// 执行sql返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] _params)
        {
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection();
            //创建一个命令
            var command = CreateCommand(connection, sql, _params);
            //获取结果
            var res = (await command.ExecuteScalarAsync());
            if (res == null)
                return default;
            return (T)res;
        }
        #region base
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        private async Task<DbConnection> GetConnection()
        {
            //获取连接
            var connection = repository.Database.GetDbConnection();
            //验证连接是否开启
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
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
            if (sql.ToUpper().StartsWith("SELECT"))
                throw new Exception($"{nameof(sql)}不是一条增删改语句");
        }

        /// <summary>
        /// 创建sqlcommand
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        private DbCommand CreateCommand(DbConnection dbConnection, string sql, params object[] _params)
        {
            //创建一个命令
            var command = dbConnection.CreateCommand();
            command.CommandText = sql;
            if (_params != null && _params.Count() > 0)
            {
                foreach (var item in _params)
                {
                    command.Parameters.Add(item);
                }
            }
            //判断是否开启了事务
            if (repository.Database.CurrentTransaction != null)
            {
                //绑定事务
                command.Transaction = repository.Database.CurrentTransaction.GetDbTransaction();
            }
            //设置超时时间
            if (repository.Database.GetCommandTimeout() != null)
            {
                int.TryParse(repository.Database.GetCommandTimeout()?.ToString(), out var commandTimeout);
                command.CommandTimeout = commandTimeout;
            }
            return command;
        }


        #endregion
    }
}
