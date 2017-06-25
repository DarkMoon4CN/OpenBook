using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.BLL;
using Mars.Server.Utils;

namespace Mars.Server.MailService
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public partial class MailMgt : ServiceBase
    {
        MailManager mailMgt;
        public MailMgt()
        {
            InitializeComponent();
            mailMgt = new MailManager();
        }

        protected override void OnStart(string[] args)
        {
            LogUtilExpand.WriteLog("==============================邮件服务日志启动===============================");
            mailMgt.Start();            
        }

        protected override void OnStop()
        {           
            mailMgt.Stop();
            LogUtilExpand.WriteLog("==============================邮件服务日志终止===============================");
            LogUtil.WriteLog("");
        }
    }
}
