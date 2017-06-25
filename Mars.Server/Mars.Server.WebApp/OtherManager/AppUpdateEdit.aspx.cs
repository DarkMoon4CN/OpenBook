using Mars.Server.BLL;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.OtherManager
{
    public partial class AppUpdateEdit : System.Web.UI.Page
    {
        public int fun = 0;
        public int id = 0;
        int flg = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["fun"], out fun);
            int.TryParse(Request.QueryString["id"],out id);
            int.TryParse(Request.QueryString["flg"], out flg);
            if (!IsPostBack && id != 0 && flg!=0)
            {
                InitData();
            }
        }
        public void InitData()
        {
            BCtrl_Common commonobj = new BCtrl_Common();
            AppUpdateEntity entity = new AppUpdateEntity();
            entity=commonobj.TryGetVersion(id);
            if (entity.AppType!= 0)
            {
                this.txtapptype.Value = entity.AppType.ToString();
                this.txtVersion.Value = entity.Version;
                this.txtdownloadUrl.Value = entity.DownloadUrl;
                this.txtforcedUpdate.Value = (entity.ForcedUpdate?"1":"0");
                this.txtSize.Value = entity.AppSize.ToString();
                this.txt_updateProfile.Value = entity.UpdateProfile;
                this.Hidtime.Value = Convert.ToDateTime(entity.CreateTime).ToString();
                this.hidPid.Value = id.ToString();
            }
        }
    }
}