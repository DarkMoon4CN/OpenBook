using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    /// <summary>
    /// 用户邮件实体
    /// </summary>
   public class UserMailEntity
    {
       public int MailID { get; set; }

       public int UserID {get;set;}

       public int EventItemID { get; set; }

       public DateTime CreateDate { get; set; }

       /// <summary>
       /// 发送时间
       /// </summary>
       public DateTime SendDate { get; set; }

       /// <summary>
       /// 邮件状态 0新加 1处理中 2发送失败 3发送成功
       /// </summary>
       public int MailStatus { get; set; }

       public string Remark { get; set; }
    }
   
    /// <summary>
    /// 用户邮件视图实体
    /// </summary>
   public class UserMailViewEntity
   {
       public int UserID { get; set; }
       public string LoginName { get; set; }

       public string Telphone { get; set; }

       public string EMail { get; set; }

       public string UserName { get; set; }

       public int MailID { get; set; }

       public DateTime CreateDate { get; set; }

       public DateTime SendDate { get; set; }

       /// <summary>
       /// 邮件状态 0新加 1处理中 2发送失败 3发送成功
       /// </summary>
       public int MailStatus { get; set; }

       public string Remark { get; set; }

       public int EventItemID { get; set; }

       public string Title { get; set; }

       public string BookListPath { get; set; }      
   }

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public class CompanyEmailAccountEntity
    {
        public int CompanyEmailAccountID { get; set; }

        public string EmailName { get; set; }

        public string EmailPassword { get; set; }

        public string EmailSMTPHost { get; set; }

        /// <summary>
        /// 邮箱状态 false禁用 1启用
        /// </summary>
        public bool CompanyEmailStatus { get; set; }
    }
}
