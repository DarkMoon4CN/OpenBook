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
    public partial class ExhibitorsTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        private int _intExhibitionID;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_exhibitionid", RequestMethod.Get)]
        public int IntExhibitionID
        {
            get { return _intExhibitionID; }
            set { _intExhibitionID = value; }
        }

        private string _strExhibitorName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("_exhibitorname", RequestMethod.Get)]
        public string StrExhibitorName
        {
            get { return _strExhibitorName; }
            set { _strExhibitorName = value; }
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
        ExhibitorSearchEntity entity = new ExhibitorSearchEntity();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            entity.ExhibitorName = _strExhibitorName;
            entity.ExhibitionID =_intExhibitionID;

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

                DataTable tableLocation = bll.GetExhibitorLocation(entity.ExhibitionID);
                List<ExhibitorToCustomerEntity> list = bll.GetExhibitorEntityList(dt,tableLocation);

                var data = new
                {
                    cnt = TotalRecords,
                    pindex = this.PageIndex,
                    showstyle = 2,
                    list = list
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    pindex = this.PageIndex,
                    list = new List<ExhibitorToCustomerEntity>()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        #endregion
    }
}