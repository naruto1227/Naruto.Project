using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Common.Interface
{
    /// <summary>
    /// 邮件的接口
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="msgToEmail">收件人的地址</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="html">html内容</param>
        /// <param name="msgPath">附件地址</param>
        /// <returns></returns>
        Task<int> SendEmailAsync(string msgToEmail, string title, string content = "", string html = "", string msgPath = "");
    }
}
