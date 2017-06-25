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
    public partial class ExhibitionTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region 查询条件
        private string _exhibitionTitle;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_exhibitiontitle", RequestMethod.Get)]
        public string ExhibitionTitle
        {
            get { return _exhibitionTitle; }
            set { _exhibitionTitle = value; }
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

        BCtrl_Exhibition bll = new BCtrl_Exhibition();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            ExhibitionSearchEntity entity = new ExhibitionSearchEntity();
            entity.ExhibitionTitle = _exhibitionTitle;

            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryData(entity, out totalcnt);
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

                var data = new
                {
                    cnt = TotalRecords,
                    showstyle = 2,
                    list = dt
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    list = new DataTable()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        #endregion
    }
}