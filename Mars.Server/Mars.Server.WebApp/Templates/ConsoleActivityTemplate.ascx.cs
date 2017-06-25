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
    public partial class ConsoleActivityTemplate : TemplateBase
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
        private string _activityName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_activityname", RequestMethod.Get)]
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
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

        BCtrl_Console_Activity bll = new BCtrl_Console_Activity();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            ActivitySearchEntity entity = new ActivitySearchEntity();
            entity.ExhibitionID = _exhibitionID;
            entity.SearchName = _activityName;

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