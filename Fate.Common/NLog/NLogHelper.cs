using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Web.AspNetCore;

namespace Fate.Common.NLog
{
    /// <summary>
    /// 写日志
    /// </summary>
    public class NLogHelper
    {
        /// <summary>
        /// 提供一个默认的静态入口
        /// </summary>
        public static NLogHelper Default { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static NLogHelper()
        {
            Default = new NLogHelper();
        }
        /// <summary>
        /// 写入一个基本信息消息
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            logger.Info(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
        public void Trace(string message)
        {
            logger.Trace(message);
        }
        public void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}
