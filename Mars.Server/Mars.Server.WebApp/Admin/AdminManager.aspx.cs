using Mars.Server.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Admin
{
    public partial class AdminManager : Mars.Server.Controls.BaseControl.BasePage
    {
        protected int fun = 0;
        protected void Page_Load(object sender, EventArgs e)
        {          
            if (!IsPostBack)
            {
                fun = Master.fun;
            }
        }       
    }
}