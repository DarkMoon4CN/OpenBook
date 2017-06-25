using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Article
{
    public partial class ArticleManager : System.Web.UI.Page
    {
        public int fun;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fun = Master.fun;
            }
        }     
    }
}