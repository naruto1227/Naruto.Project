using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Common.Infrastructure
{
    /// <summary>
    /// 此地存放静态字段
    /// </summary>
    public class StaticFieldConfig
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
    }
}
