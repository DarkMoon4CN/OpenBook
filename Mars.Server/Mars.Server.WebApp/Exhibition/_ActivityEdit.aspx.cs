using Mars.Server.BLL.Exhibition;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity.Exhibition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Exhibition
{
    public partial class _ActivityEdit : BasePage
    {
        protected ActivityEntity item = new ActivityEntity();
        protected int id = 0;
        protected int pid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out pid);
            int.TryParse(Request.QueryString["id"], out id);
            if (!IsPostBack)
            {
                InitController();

                if (id > 0)
                {
                    InitActivity();
                }
            }
        }

        private void InitController()
        {
            BCtrl_Exhibition bll = new BCtrl_Exhibition();
            DataTable table = bll.GetExhibitionTable();
            if (table != null)
            {
                foreach (DataRow dr in table.Rows)
                {
                    this.sel_exhibition.Items.Add(new ListItem(dr["ExhibitionTitle"].ToString(), dr["ExhibitionID"].ToString()));
                }
            }

            if (pid > 0)
            {
                this.hid_pid.Value = pid.ToString();
            }
        }

        private void InitActivity()
        {
            BCtrl_Console_Activity bll = new BCtrl_Console_Activity();
            item = bll.GetEntity(id);
            if (item != null)
            {
                this.sel_exhibition.Value = item.ExhibitionID.ToString();

                //构建单选框
                BCtrl_Exhibitor exhibitorBll = new BCtrl_Exhibitor();
                List<ExhibitorToCustomerEntity> eList = exhibitorBll.GetExhibitorEntityList(item.ExhibitionID);
                if (eList != null)
                {
                    foreach (ExhibitorToCustomerEntity _item in eList)
                    {
                        this.sel_exhibitor.Items.Add(new ListItem(_item.ExhibitorName, _item.ExhibitorID.ToString()));
                    }
                }
                this.sel_exhibitor.Value = item.ExhibitorID.ToString();
                
                this.chk_statetype.Checked = item.StateTypeID == 1;
                this.hid_aid.Value = item.ActivityID.ToString();
                this.hid_pid.Value = item.ParentID.ToString();

                this.txt_activitytitle.Value = item.ActivityTitle;
                this.txt_starttime.Value = item.ActivityStartTime.ToString("yyyy-MM-dd HH:mm");
                this.txt_endTime.Value = item.ActivityEndTime.ToString("yyyy-MM-dd HH:mm");
                this.txt_activitylocation.Value = item.ActivityLocation;
                this.txt_activityhostunit.Value = item.ActivityHostUnit;
                this.txt_activityguest.Value = item.ActivityGuest;
                this.txt_activityabstract.Value = item.ActivityAbstract;
                this.txt_activityorder.Value = item.ActivityOrder.ToString();
            }
        }
    }
}