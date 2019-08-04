using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Options;
using Fate.Common.Repository.Mysql.Base;
using System.Linq;

namespace Fate.Common.Repository.Mysql.Interceptor
{
    public class EFCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        public bool isSumbitTran = false;
        private IOptions<List<EFOptions>> options;
        private readonly object _lock = new object();
        public EFCommandInterceptor(IOptions<List<EFOptions>> _options)
        {
            options = _options;
        }
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            lock (_lock)
            {
                //如果是主动开启事务 
                if (value.Key == RelationalEventId.TransactionStarted.Name && isSumbitTran == false)
                {
                    //获取当前事务连接信息
                    var connec = ((IDbConnection)((TransactionEventData)value.Value).Transaction.Connection);
                    //获取连接的库的信息
                    var database = connec.Database;
                    //获取当前连接对应的ef配置连接信息
                    var info = options.Value.Where(a => a.WriteReadConnectionName.ToLower().Contains($"database={database}") || a.ReadOnlyConnectionName.ToLower().Contains($"database={database}")).FirstOrDefault();
                    if (info == null)
                    {
                        throw new ArgumentNullException("找不到EF配置连接信息!");
                    }
                    if (connec.State == ConnectionState.Open)
                    {
                        connec.Close();
                    }
                    //更改为主库的连接字符串
                    connec.ConnectionString = info.WriteReadConnectionName;
                    //
                    if (connec.State == ConnectionState.Closed)
                    {
                        connec.Open();
                    }
                    isSumbitTran = true;
                }
                //验证是否追踪到的是efcore 的 savechange
                else if (value.Key == CoreEventId.SaveChangesStarting.Name && isSumbitTran == false)
                {
                    var dbContext = ((DbContextEventData)value.Value).Context;

                    if (dbContext.Database.GetDbConnection().State == ConnectionState.Open)
                    {
                        dbContext.Database.GetDbConnection().Close();
                    }

                    //获取只读的连接字符串
                    dbContext.Database.GetDbConnection().ConnectionString = options.Value.Where(a => a.DbContextType == dbContext.GetType()).FirstOrDefault().WriteReadConnectionName;

                    //更改连接的状态
                    if (dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    {
                        dbContext.Database.GetDbConnection().Open();
                    }
                    isSumbitTran = true;
                }
                else if (value.Key == RelationalEventId.CommandExecuting.Name && isSumbitTran == false)
                {
                    var command = ((CommandEventData)value.Value).Command;
                    if (command.CommandText.StartsWith("SELECT"))
                    {
                        //获取当前连接信息
                        var connec = command.Connection;
                        //获取使用的库
                        var database = connec.Database;
                        //获取当前连接对应的ef配置连接信息
                        var info = options.Value.Where(a => a.WriteReadConnectionName.ToLower().Contains($"database={database}") || a.ReadOnlyConnectionName.ToLower().Contains($"database={database}")).FirstOrDefault();
                        if (info == null)
                        {
                            throw new ArgumentNullException("找不到EF配置连接信息!");
                        }
                        if (connec.State == ConnectionState.Open)
                        {
                            connec.Close();
                        }
                        //更改为从库的连接字符串
                        connec.ConnectionString = info.ReadOnlyConnectionName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        //
                        if (connec.State == ConnectionState.Closed)
                        {
                            connec.Open();
                        }

                        isSumbitTran = true;
                    }
                }
                //上下文释放之后 并且状态还为处于事物的 状态
                else if (value.Key == CoreEventId.ContextDisposed.Name && isSumbitTran)
                {
                    //更改状态为未开启事务的状态
                    isSumbitTran = false;
                }
            }
        }
        /// <summary>
        /// 更改连接的状态
        /// </summary>
        /// <param name="dbConnection"></param>
        private void ChangeConnectionStatus(IDbConnection dbConnection)
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
                return;
            }
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
                return;
            }
        }
    }
}
