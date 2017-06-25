using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Utils;
using Mars.Server.Entity;
using Mars.Server.BLL;

namespace Mars.Server.WebApp.Article
{
    public partial class ArticleGroupEdit2 : System.Web.UI.Page
    {
        protected int groupEventID = 0;
        BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 if (int.TryParse(Request.QueryString["pid"],out groupEventID) && groupEventID > 0)
                 {
                     this.hidPid.Value = groupEventID.ToString();
                     EventItemGroupEntity entity = bll.QueryGroupEntity(groupEventID);

                     if (entity != null && entity.GroupEventID > 0)
                     {
                         this.txtGroupEventName.Value = entity.GroupEventName;
                         this.txtPublishTime.Value = entity.PublishTime.ToString("yyyy-MM-dd HH:mm:ss");
                     }
                     else
                     {
                         WebMaster.EndPage();
                     }
                 }
                else
                 {
                     WebMaster.EndPage();
                 }
            }
        }
    }
}