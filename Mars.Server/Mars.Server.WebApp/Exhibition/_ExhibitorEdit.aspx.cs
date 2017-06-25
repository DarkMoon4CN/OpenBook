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
    public partial class _ExhibitorEdit : BasePage
    {
        protected ExhibitorEntity item = new ExhibitorEntity();
        protected int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["id"], out id);
            if (!IsPostBack)
            {
                InitController();

                if (id > 0)
                {
                    InitExhibitor();
                }
            }
        }

        private void InitExhibitor()
        {
            BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
            item = bll.GetExhibitorEntity(id);
            if (item != null)
            {
                this.sel_exhibition.Value = item.ExhibitionID.ToString();
                this.txt_exhibitorname.Value = item.ExhibitorName;
                if (item.ExhibitorLocationList != null)
                {
                    string tmploc = "";
                    foreach (ExhibitorLocationEntity loc in item.ExhibitorLocationList)
                    {
                        tmploc += loc.ExhibitorLocation+"\n";
                    }
                    this.txt_exhibitorlocations.Value = tmploc;
                }
                
                this.chk_ishadbooklist.Checked = item.IsHadBookList;
                this.chk_statetype.Checked = item.StateTypeID == 1;
                this.hid_eid.Value = item.ExhibitorID.ToString();
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
        }
    }
}