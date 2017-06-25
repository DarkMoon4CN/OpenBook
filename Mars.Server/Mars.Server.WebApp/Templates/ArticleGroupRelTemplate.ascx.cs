using Mars.Server.BLL;
using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
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
    public partial class ArticleGroupRelTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件      

        /// <summary>
        /// 文章主键IDs 
        /// </summary>
        [TemplateProperty("ids", RequestMethod.Get)]
        public string ArticleIds { get; set; }

        /// <summary>
        /// 专题组ID
        /// </summary>
        [TemplateProperty("groupid", RequestMethod.Get)]
        public int GroupEventID { get; set; }

        #endregion

        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        }

        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(true);
        }

        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;
            DataTable table = null;

            BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();

            EventItemGroupSearchEntity entity = new EventItemGroupSearchEntity();
            entity.GroupEventID = GroupEventID;          
           // entity.EventItemIDs = ArticleIds;
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;

            table = bll.QueryGroupRelViewList(entity, out totalcnt);

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
                    list = dt,
                    fun = base.fun         
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    message = "暂无权限",
                    list = new DataTable()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        public override void HandleTemplate()
        {
            this.mphArticleGroupRel.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}