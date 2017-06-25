using Mars.Server.BLL.Exhibition;
using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Entity.Exhibition;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Templates
{
    public partial class ExhibitorTemplate  : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             
        }

        #region 查询条件
        private int _exhibitionID;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_exhibitionid", RequestMethod.Get)]
        public int ExhibitionID
        {
            get { return _exhibitionID; }
            set { _exhibitionID = value; }
        }
        private string _exhibitorName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_exhibitorname", RequestMethod.Get)]
        public string ExhibitorName
        {
            get { return _exhibitorName; }
            set { _exhibitorName = value; }
        }
        #endregion

        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        }

        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(true);
        }

        BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            ExhibitorSearchEntity entity = new ExhibitorSearchEntity();
            entity.ExhibitionID = _exhibitionID;
            entity.ExhibitorName = _exhibitorName;

            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryDataConsole(entity, out totalcnt);
            base.TotalRecords = totalcnt;

            return table;
        }

        #region 重写

        /// <summary>
        /// 重写
        /// </summary>
        protected override void RenderJsonData()
        {
            if (ValidQueryCondition())
            {
                this.Visible = false;
                Response.Clear();
                DataTable dt = QueryDataPerPage();
                DataTable table_location = bll.GetExhibitorLocation(_exhibitionID);

                List<ExhibitorEntity> list = bll.GetConsoleExhibitorEntityList(dt, table_location);

                var data = new
                {
                    cnt = TotalRecords,
                    list = list
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    list = new List<ExhibitorEntity>()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        #endregion
    }
}