using Mars.Server.BLL;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Categories
{
    public partial class CategoriesSecondManager : System.Web.UI.Page
    {
        public int fun = 0;
        public int pid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fun = Master.fun;
                DateBind(); 
            }
        }
        public void DateBind()
        {
            BCtrl_Categories bll = new BCtrl_Categories();
            List<CategoriesEntity> list=bll.CategoriesFirstLevel();
            foreach (CategoriesEntity item in list)
            {
                this.txtCategoriesName.Items.Add(new ListItem(item.CalendarTypeName, item.CalendarTypeID.ToString()));
            }
            pid = int.Parse(this.txtCategoriesName.Value);
        }
    }
}