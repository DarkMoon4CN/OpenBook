using Mars.Server.BLL.Comments;
using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Entity.Comments;
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
    public partial class ReplyListTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [TemplateProperty("_title", RequestMethod.Get)]
        public string Title { get; set; }
        [TemplateProperty("_ct", RequestMethod.Get)]
        public string CheckType { get; set; }
        [TemplateProperty("_vt", RequestMethod.Get)]
        public string ViewType { get; set; }

        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        }

        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(true);
        }

        ReplySearchEntity entity = new ReplySearchEntity();
        BCtrl_ReplySearch bll = new BCtrl_ReplySearch();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            entity.Title = Title;
            entity.CheckType = CheckType;
            entity.ViewType = ViewType;

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