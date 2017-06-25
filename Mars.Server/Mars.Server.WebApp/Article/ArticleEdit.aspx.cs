using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Article
{
    public partial class ArticleEdit : System.Web.UI.Page
    {
        int eventItemID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out eventItemID);
            if (!IsPostBack)
            {
                InitData();               
            }
        }

        private void InitData()
        {
            this.hidFun.Value = Master.fun.ToString(); 
            
         
            if (eventItemID > 0)
            {
                this.hidPid.Value = eventItemID.ToString();
            }           
        }
    }
}