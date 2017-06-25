using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.app
{
    public partial class recommend : System.Web.UI.Page
    {
        protected string strRecommendID;
        protected void Page_Load(object sender, EventArgs e)
        {
            strRecommendID = string.IsNullOrEmpty(Request.QueryString["rec"]) ? "" : Request.QueryString["rec"];
        }
    }
}