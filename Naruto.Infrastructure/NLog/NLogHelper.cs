using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Web.AspNetCore;
using Naruto.Infrastructure.Interface;
using System.Threading.Tasks;

namespace Naruto.Infrastructure.NLog
{
    /// <summary>
    /// 写日志
    /// </summary>
    public class NLogHelper : ILog
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private readonly Logger logger;
        public NLogHelper()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 一般提示
        /// </summary>

        public Task Info(string message)
        {
            return Task.Run(() =>
            {
                logger.Info(message);
            });
        }
        /// <summary>
        /// 重大错误提示
        /// </summary>
        /// <param name="message"></param>
        public Task Error(string message)
        {
            return Task.Run(() =>
            {
                logger.Error(message);
            });
        }

        /// <summary>
        /// bug
        /// </summary>
        /// <param name="message"></param>
        public Task Debug(string message)
        {
            return Task.Run(() =>
            {
                logger.Debug(message);
            });
        }

        public Task Trace(string message)
        {
            return Task.Run(() =>
            {
                logger.Trace(message);
            });
        }

        public Task Fatal(string message)
        {
            return Task.Run(() =>
            {
                logger.Fatal(message);
            });
        }
    }
}
