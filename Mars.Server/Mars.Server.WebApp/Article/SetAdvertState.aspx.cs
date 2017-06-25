using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Entity;
using Mars.Server.BLL;
using System.Data;
using Mars.Server.Utils;

namespace Mars.Server.WebApp.Article
{
    public partial class SetAdvertState : System.Web.UI.Page
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
                    string advert = table.Rows[0]["Advert"].ToString();
                    chkAdvert.Checked = advert == "1" ? true : false;
                    txtAdvertOrder.Value = table.Rows[0]["AdvertOrder"].ToString();
                    txtAdsEndTime.Value = Convert.ToDateTime(table.Rows[0]["AdsEndTime"]).Year == 4000 ? "" : Convert.ToDateTime(table.Rows[0]["AdsEndTime"]).ToString("yyyy-MM-dd HH:mm:ss");
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