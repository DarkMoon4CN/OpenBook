using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Test
{
    public partial class EdwardTestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_test_Click(object sender, EventArgs e)
        {
            Utils.LogEntity log = new Utils.LogEntity()
            {
                UserID = 1,
                LogTitle = "测试日志",
                LogContent = "测试日志内容信息",
                LogTime = DateTime.Now,
                LogTypeID = 1,
                UnqiueID = Guid.NewGuid(),
                ExInfo = Request.UserHostAddress,
                LogMeta = Request.UserAgent
            };
            Utils.LogUtil.WriteLog(log);
        }

        protected void btn_del_Click(object sender, EventArgs e)
        {
            string txt = this.txt_word.Text;
            string rev = "";

            if (!string.IsNullOrEmpty(txt))
            {
                for (int i = 0; i < txt.Length; i++) {
                    string tv = txt.Substring(i, 1);
                    if (!string.IsNullOrEmpty(tv.Trim())) { 
                    if (!rev.Contains(tv)) {
                        rev += tv;
                    }
                    }
                }
            }

            this.txt_word.Text = rev;
        }
    }
}