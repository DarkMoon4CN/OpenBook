using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.BLL;
using Mars.Server.Utils;
using Mars.Server.Entity;

namespace Mars.Server.WebApp.Article
{
    public partial class SetGroupArticleOrder : System.Web.UI.Page
    {
        BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();
        protected int eventGroupID = 0;
        protected int eventItemID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["groupid"], out eventGroupID) && eventGroupID > 0
                    && int.TryParse(Request.QueryString["itemid"], out eventItemID) && eventItemID > 0)
                {
                    EventItemGroupRelViewEntity entity = bll.QueryGroupRelViewEntity(eventGroupID, eventItemID);
                    if (entity != null)
                    {
                        this.txtTitle.Value = entity.Title;
                        this.txtDisplayOrder.Value = entity.DisplayOrder.ToString();
                        this.hid_GroupID.Value = eventGroupID.ToString();
                        this.hid_ItemID.Value = eventItemID.ToString();
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