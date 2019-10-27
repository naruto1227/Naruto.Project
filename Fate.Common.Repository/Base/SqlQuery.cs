using Fate.Common.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Fate.Common.Repository.Base
{
    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的查询操作
    /// </summary>
    public class SqlQuery<TDbContext> : ISqlQuery<TDbContext> where TDbContext : DbContext
    {
        //定义一个上下文
        private DbContext repository;

        /// <summary>
        /// 构造获取上下文工厂
        /// </summary>
        public SqlQuery(IDbContextFactory factory)
        {
            repository = factory.Get<TDbContext>();
        }
        #region 同步
        /// <summary>
        /// 返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlQuery(string sql, params object[] _params)
        {
            return ExecuteSqlQueryAsync(sql, _params).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 执行sql返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] _params)
        {
            return ExecuteScalarAsync<T>(sql, _params).GetAwaiter().GetResult();
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
        public async Task<DataTable> ExecuteSqlQueryAsync(string sql, params object[] _params)
        {
            CheckSql(sql);
            //获取连接
            var connection = await GetConnection();
            var command = CreateCommand(connection, sql, _params);
            //执行返回
            DataTable dataTable = new DataTable();
            dataTable.Load((await command.ExecuteReaderAsync()));
            return dataTable;
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
        #endregion

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
            if (!sql.ToUpper().StartsWith("SELECT"))
                throw new Exception($"{nameof(sql)}不是一条查询语句");
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
            return command;
        }


        #endregion
    }
}
