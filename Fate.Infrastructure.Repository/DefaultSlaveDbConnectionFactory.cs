using Fate.Infrastructure.Repository.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Fate.Infrastructure.Repository
{
    /// <summary>
    /// 从库的连接工厂的默认实现
    /// </summary>
    public class DefaultSlaveDbConnectionFactory : ISlaveDbConnectionFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultSlaveDbConnectionFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="eFOptions"></param>
        /// <returns></returns>
        public List<SlaveDbConnection> Get(EFOptions eFOptions)
        {
            using var services = serviceProvider.CreateScope();
            using var dbcontent = services.ServiceProvider.GetRequiredService(eFOptions.DbContextType) as DbContext;
            if (dbcontent.Database.ProviderName.EndsWith("MySql"))
            {
                return MySqlSlaveDbConnection(eFOptions);
            }
            else if (dbcontent.Database.ProviderName.EndsWith("SqlServer"))
            {
                return SqlServerSlaveDbConnection(eFOptions);
            }
            throw new ApplicationException("不支持的数据库类型");
        }
        /// <summary>
        /// mysql的连接信息
        /// </summary>
        /// <param name="eFOptions"></param>
        /// <returns></returns>
        private List<SlaveDbConnection> MySqlSlaveDbConnection(EFOptions eFOptions)
        {
            //实例化连接信息
            List<SlaveDbConnection> slaveDbConnections = new List<SlaveDbConnection>();

            //遍历从库连接
            eFOptions.ReadOnlyConnectionString.ToList().ForEach(connectionString =>
            {
                var items = connectionString.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                slaveDbConnections.Add(new SlaveDbConnection()
                {
                    ConnectionString = connectionString,
                    IsAvailable = true,
                    HostName = items.Where(a => a.Contains("datasource")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1],
                    Port = Convert.ToInt32(items.Where(a => a.Contains("port")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)?[1])
                });
            });
            return slaveDbConnections;
        }
        /// <summary>
        /// mssql的连接信息
        /// </summary>
        /// <param name="eFOptions"></param>
        /// <returns></returns>
        private List<SlaveDbConnection> SqlServerSlaveDbConnection(EFOptions eFOptions)
        {
            //实例化连接信息
            List<SlaveDbConnection> slaveDbConnections = new List<SlaveDbConnection>();

            //遍历从库连接
            eFOptions.ReadOnlyConnectionString.ToList().ForEach(connectionString =>
            {
                var items = connectionString.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                //获取服务器
                var datasource = items.Where(a => a.Contains("data source")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (datasource == null)
                {
                    datasource = items.Where(a => a.Contains("server")).FirstOrDefault()?.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                }
                //获取ip和port
                var ipPort = datasource?[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                slaveDbConnections.Add(new SlaveDbConnection()
                {
                    ConnectionString = connectionString,
                    IsAvailable = true,
                    HostName = ipPort?[0],
                    Port = Convert.ToInt32(ipPort?[1])
                });
            });
            return slaveDbConnections;
        }
    }
}
