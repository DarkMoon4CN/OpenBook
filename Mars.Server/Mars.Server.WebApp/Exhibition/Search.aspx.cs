using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Exhibition
{
    public partial class Search : System.Web.UI.Page
    {
        protected string _searchtype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            _searchtype = string.IsNullOrEmpty(Request.QueryString["_searchtype"]) ? "" : Request.QueryString["_searchtype"];
        }
    }
}