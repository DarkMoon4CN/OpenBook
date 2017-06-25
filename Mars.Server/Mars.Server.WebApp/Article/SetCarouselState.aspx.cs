using Mars.Server.BLL;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Article
{
    public partial class SetCarouselState : System.Web.UI.Page
    {
        int eventItemID = 0;
        BCtrl_EventItem bllEventItem = new BCtrl_EventItem();

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out eventItemID);
            if (!IsPostBack)
            {
                if (eventItemID > 0)
                {
                    this.hidPid.Value = eventItemID.ToString();
                    DataTable table = bllEventItem.QueryViewOnlyEventItemTable(eventItemID);
                    string advert = table.Rows[0]["DiscoverAdvert"].ToString();
                    chkAdvert.Checked = advert == "1" ? true : false;
                    txtAdsEndTime.Value = Convert.ToDateTime(table.Rows[0]["DiscoverAdsEndTime"]).Year==4000?"" : Convert.ToDateTime(table.Rows[0]["DiscoverAdsEndTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    txtAdvertOrder.Value = table.Rows[0]["AdvertOrder"].ToString();
                    if (advert != "1")
                    {
                        txtAdvertOrder.Attributes.Add("disabled", "disabled");
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