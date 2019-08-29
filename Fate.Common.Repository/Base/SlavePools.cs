using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using System.Net.Sockets;
using System.Linq;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Fate.Common.Repository.Object
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// 存储 从库 slave 的连接字符串的集合
    /// </summary>
    internal static class SlavePools
    {
        /// <summary>
        /// 存放可以上下文的从库的连接字符串的信息
        /// </summary>
        internal static ConcurrentDictionary<Type, List<SlaveDbConnection>> slaveConnec = new ConcurrentDictionary<Type, List<SlaveDbConnection>>();

        /// <summary>
        /// 执行心跳检查 检查从库是否正在运行
        /// </summary>
        /// <param name="slaveConnections">从库的连接字符串</param>
        /// <param name="dbcontext">上下文类型</param>
        internal static bool HeartBeatCheck(Type dbcontext, SlaveDbConnection slaveConnections)
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    //连接tcp服务器
                    tcpClient.Connect(slaveConnections.HostName, slaveConnections.Port);
                }
                catch (Exception)
                {
                    slaveConnec.Where(a => a.Key == dbcontext).Select(a => a.Value).FirstOrDefault()?.ForEach(item =>
                    {
                        if (item == slaveConnections)
                        {
                            item.IsAvailable = false;
                        }
                    });
                    Console.WriteLine($"SlavePools:当前{slaveConnections.ConnectionString}从库的无法连接");
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 执行定时器 验证从库的状态
        /// </summary>
        internal static void TimerHeartBeatCheck()
        {
            if (slaveConnec != null && slaveConnec.Count() > 0)
            {
                slaveConnec.Select(a => a.Value).ToList().ForEach(itemList =>
                  {
                      itemList.ForEach(item =>
                      {
                          using (var tcpClient = new TcpClient())
                          {
                              try
                              {
                                  tcpClient.Connect(item.HostName, item.Port);
                                  item.IsAvailable = true;
                              }
                              catch
                              {
                                  item.IsAvailable = false;
                              }
                          }
                      });
                  });
            }
        }
    }
}
