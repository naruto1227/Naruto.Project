using Fate.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Config
{
    /// <summary>
    /// 邮箱的配置中心
    /// </summary>
    public class EmailConfig
    {
        /// <summary>
        /// 邮箱的发件人地址
        /// </summary>
        public static string SendEmailAddress
        {
            get
            {
                var sendEmailAddress = ConfigurationManage.GetAppSetting("EmailConfig:SendEmailAddress");
                return sendEmailAddress;
            }
        }
        /// <summary>
        /// 设置所需邮箱smtp服务器及支持的协议
        /// </summary>
        public static string EmailHost
        {
            get
            {
                var emailHost = ConfigurationManage.GetAppSetting("EmailConfig:EmailHost");
                return emailHost;
            }
        }
        /// <summary>
        /// 邮箱的SMTP的授权码
        /// </summary>
        public static string SendEmailCode
        {
            get
            {
                var sendEmailCode = ConfigurationManage.GetAppSetting("EmailConfig:SendEmailCode");
                return sendEmailCode;
            }
        }
        /// <summary>
        /// 邮箱服务器的端口 qq是 587
        /// </summary>
        public static int EmailPort
        {
            get
            {
                var emailPort = ConfigurationManage.GetAppSetting("EmailConfig:EmailPort");
                var res = 0;
                int.TryParse(emailPort,out res);
                return res;
            }
        }
    }
}
