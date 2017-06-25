using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Exhibition
{
    public partial class _ActivityChildren : System.Web.UI.Page
    {
        protected string parentid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            parentid = string.IsNullOrEmpty(Request.QueryString["id"]) ? "0" : Request.QueryString["id"];
        }
    }
}