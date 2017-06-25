using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Admin
{
    public partial class ChangePwd : Page //: Mars.Server.Controls.BaseControl.BasePage
    {
        protected string loginName = string.Empty;
        protected int userID = 0;
        protected string flag;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["pid"], out userID);
                loginName = Request.QueryString["uname"];
                flag = Request.QueryString["flag"];

                this.lblLoginName.InnerText = loginName;
                this.hidPid.Value = userID.ToString();
            }
        }
    }
}