using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Fate.Common.Config;
using MimeKit.Text;
using System.IO;
using Fate.Common.Interface;

namespace Fate.Common.MailKit
{
    /// <summary>
    /// mailkit
    /// </summary>
    public class EmailKit : ICommonClassSigleDependency
    {
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="msgToEmail">邮件的收件人</param>
        /// <param name="title">标题</param>
        /// <param name="content">文字内容</param>
        /// <param name="html">html内容</param>
        /// <param name="msgPath">附件地址</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string msgToEmail, string title, string content = "", string html = "", string msgPath = "")
        {
            await SendToEmailAsync(msgToEmail, title, content, html, msgPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgToEmail"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="msgPath"></param>
        /// <returns></returns>
        private async Task SendToEmailAsync(string msgToEmail, string title, string content, string html, string msgPath = "")
        {
            //实例化需要发送的资源的消息
            MimeMessage message = new MimeMessage();
            //发件人的地址
            message.From.Add(new MailboxAddress(EmailConfig.SendEmailAddress));
            //收件人的地址
            message.To.Add(new MailboxAddress(msgToEmail));
            //标题
            message.Subject = title;
            //初始化mime的新的实例内容(邮件的内容)
            var multipart = new Multipart();
            //文本内容
            if (!string.IsNullOrWhiteSpace(content))
            {
                var text = new TextPart(TextFormat.Text)
                {
                    Text = content
                };
                multipart.Add(text);
            }
            //html内容
            if (!string.IsNullOrWhiteSpace(html))
            {
                var htmlContent = new TextPart(TextFormat.Html)
                {
                    Text = html
                };
                multipart.Add(htmlContent);
            }

            //附件
            if (!string.IsNullOrWhiteSpace(msgPath))
            {
                var paths = msgPath.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (paths != null && paths.Length > 0)
                {
                    foreach (var item in paths)
                    {
                        var attMime = new MimePart()
                        {
                            FileName = Path.GetFileName(item),//文件名
                            ContentTransferEncoding = ContentEncoding.Base64,//编码
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),//附件
                            IsAttachment = true,
                            Content = new MimeContent(File.OpenRead(item), ContentEncoding.Default)
                        };
                        multipart.Add(attMime);
                    }
                }
            }
            //内容
            message.Body = multipart;
            //发件 日期
            message.Date = DateTimeOffset.Now;
            //实例化 smtp 服务器
            using (SmtpClient smtpClient = new SmtpClient())
            {
                //连接到邮箱的服务器
                await smtpClient.ConnectAsync(EmailConfig.EmailHost, EmailConfig.EmailPort, false);
                //邮箱认证
                await smtpClient.AuthenticateAsync(new NetworkCredential(EmailConfig.SendEmailAddress, EmailConfig.SendEmailCode));
                //发送邮件
                await smtpClient.SendAsync(message);
                //断开服务连接
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
