using Mars.Server.BLL;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Festival
{
    public partial class FestivalEdit : System.Web.UI.Page
    {
        public int fun = 0;
        public string id = "";
        public string starttime = ""; 
        public string Endtime = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            fun = Master.fun;
            id = Request.QueryString["id"];
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    InitData();
                }
            }
        }
        private void InitData()
        {
            if (!string.IsNullOrEmpty(id))
            {
                FestivalEntity entity = new FestivalEntity();
                entity.FestivalID = new Guid(id); 
                BCtrl_Festival bll = new BCtrl_Festival();
                DataTable db = bll.GetFestival(entity);
                if (db.Rows.Count > 0)
                {
                    this.FestivalName.Value = db.Rows[0]["FestivalName"].ToString();
                    this.FestivalShortName.Value = db.Rows[0]["FestivalShortName"].ToString(); 
                    starttime =Convert.ToDateTime(db.Rows[0]["StartTime"].ToString()).ToString("yyyy-MM-dd"); 
                    Endtime = Convert.ToDateTime(db.Rows[0]["EndTime"].ToString()).ToString("yyyy-MM-dd"); 
                    this.FestivalType.Value = db.Rows[0]["FestivalType"].ToString();
                    this.FestivalWeight.Value = db.Rows[0]["FestivalWeight"].ToString();
                    this.txtid.Value = db.Rows[0]["FestivalID"].ToString();
                } 
            }

        }
    }
}