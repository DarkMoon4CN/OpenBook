using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Mars.Server.Utils
{
    public class SendEmail
    {
        public static bool Send(SendEmailEntity  entity)
        {
            try
            {
                string sendName = string.Empty;
                try
                {
                    if (entity.FromSendName != null)
                    {

                        sendName = entity.FromSendName;
                    }
                    else
                    {
                        sendName = "开卷网络";
                        //sendName = System.Configuration.ConfigurationManager.AppSettings["Mail_SendName"];
                    }
                }
                catch 
                {
                    sendName = "开卷网络";
                }

                //设置 收件人和发件人地址
                MailAddress from = new MailAddress(entity.FromEmail, sendName);
                MailAddress to = new MailAddress(entity.ToEmail, entity.ToSendName);
                MailMessage message = new MailMessage(from, to);

                message.Subject = entity.Subject;
                message.Body =entity.Body;
                message.BodyEncoding = Encoding.GetEncoding("gb2312");
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;


                SmtpClient client = new SmtpClient(); 
                client.Timeout = 100000;
                client.Host = entity.Host;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(entity.FromEmail, entity.FromPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("邮件发送异常："+ex.Message);
                return false;
            }
        }

        public static bool IsEmail(string email)
        {
            string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (Regex.IsMatch(email, expression, RegexOptions.Compiled))
            {
                return false;
            }
            return true;
        }
    }
    public class SendEmailEntity 
    {
        /// <summary>
        /// 发送者邮箱
        /// </summary>
        public string FromEmail { get; set; }
    
        /// <summary>
        /// 发送者名
        /// </summary>
        public string FromSendName { get; set; }

        /// <summary>
        /// 发送者邮箱密码
        /// </summary>
        public string FromPassword { get; set; }


       
        /// <summary>
        /// 接收者邮箱
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// 接收者名
        /// </summary>
        public string ToSendName { get; set; } 

        /// <summary>
        ///  附件
        /// </summary>
        public Attachment Att { get; set; }

        /// <summary>
        ///  标题或者主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///  邮件正文
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 发送者邮箱地址或者IP
        /// </summary>
        private string _host;

        public string Host
        {
            get 
            {
                if (string.IsNullOrEmpty(_host) == true || _host == "") 
                {
                    return "smtp.qiye.163.com";
                }
                return _host; 
            }
            set { _host = value; }
        }

        


    }
}
