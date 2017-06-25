using Mars.Server.BLL.NewYear;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Entity.NewYear;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.NewYear
{
    public partial class Index : WxBasePage
    {
        protected string domain = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            domain = WebMaster.Domain;
            if (!IsPostBack) {
                InitPageData();
                this.CheckSignature();
            }
        }

        private void InitPageData()
        {
            BCtrl_Couplet bll = new BCtrl_Couplet();
            List<CoupletGroupEntity>  list = bll.GetCoupletGroupEntityListWithCache();
            this.hid_coupletlist.Value = JsonObj<List<CoupletGroupEntity>>.ToJsonString(list);
            List<FuImageEntity> imgList = bll.GetFuImageEntityListWithCache();
            this.hid_fuImagelist.Value = JsonObj<List<FuImageEntity>>.ToJsonString(imgList);
        }
    }
}