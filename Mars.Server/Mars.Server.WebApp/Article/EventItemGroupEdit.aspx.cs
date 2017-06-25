using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Utils;

namespace Mars.Server.WebApp.Article
{
    public partial class EventItemGroupEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["groupid"]))
                {
                    this.hidGroupID.Value = Request.QueryString["groupid"];
                    this.hidFun.Value = Request.QueryString["fun"];
                }
                else
                {
                    WebMaster.EndPage();   
                }
            }
        }
    }
}