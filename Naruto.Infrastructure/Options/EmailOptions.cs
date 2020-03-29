using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Infrastructure.Options
{
    /// <summary>
    /// email的配置
    /// </summary>
    public class EmailOptions
    {
        /// <summary>
        /// 设置所需邮箱smtp服务器及支持的协议
        /// </summary>
        public string EmailHost { get; set; }

        /// <summary>
        /// 邮箱服务器的端口 qq是 587
        /// </summary>
        public int EmailPort { get; set; }

        /// <summary>
        /// 邮箱的发件人地址
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 邮箱的SMTP的授权码
        /// </summary>
        public string EmailCode { get; set; }
    }
}
