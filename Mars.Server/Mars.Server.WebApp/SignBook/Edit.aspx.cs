using Mars.Server.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.SignBook
{
    public partial class Edit : System.Web.UI.Page
    {
        public string sid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            sid = Request["sid"];
            if (sid == null)
            {
                sid = "";
            }
        }
    }
}