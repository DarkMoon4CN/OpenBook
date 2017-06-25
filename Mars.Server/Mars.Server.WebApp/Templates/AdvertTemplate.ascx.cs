using Mars.Server.BLL;
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
    public partial class AdvertTemplate :  Mars.Server.Controls.TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        /// <summary>
        /// 标题
        /// </summary>
        [TemplateProperty("title", RequestMethod.Get)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 二级分类
        /// </summary>
        [TemplateProperty("type2", RequestMethod.Get)]
        public string SecondTypeID
        {
            get;
            set;
        }

        /// <summary>
        /// 一级分类
        /// </summary>
        [TemplateProperty("type1", RequestMethod.Get)]
        public string FirstTypeID
        {
            get;
            set;
        }

        [TemplateProperty("stime", RequestMethod.Get)]
        public string StartTime
        {
            get;
            set;
        }

        [TemplateProperty("etime", RequestMethod.Get)]
        public string EndTime
        {
            get;
            set;
        }
        [TemplateProperty("dtime", RequestMethod.Get)]
        public string DayTime
        {
            get;
            set;
        }
        /// <summary>
        /// 推荐
        /// </summary>
        [TemplateProperty("recommend", RequestMethod.Get)]
        public string Recommend
        {
            get;
            set;
        }       
     
        [TemplateProperty("op", RequestMethod.Get)]
        public int CreateOpID
        {
            get;
            set;
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

        private DataTable GetQueryData(bool isDownload)
        {
            BCtrl_EventItem bll = new BCtrl_EventItem();
            int totalcnt = 0;

            EventItemSearchEntity entity = new EventItemSearchEntity();
            entity.Title = this.Title;
            entity.FirstTypeID = !string.IsNullOrEmpty(this.FirstTypeID) ? int.Parse(FirstTypeID) : -1;
            entity.SecondTypeID = !string.IsNullOrEmpty(SecondTypeID) ? int.Parse(SecondTypeID) : -1;
            entity.StartTime = !string.IsNullOrEmpty(this.StartTime) ? DateTime.Parse(this.StartTime) : (DateTime?)null;
            entity.EndTime = !string.IsNullOrEmpty(this.EndTime) ? DateTime.Parse(this.EndTime) : (DateTime?)null;
            entity.Recommend = !string.IsNullOrEmpty(Recommend) ? int.Parse(Recommend) : -1;
            entity.Advert = 1;
            entity.IsEnableAdvertOrder = true;
            entity.DayTime = !string.IsNullOrEmpty(this.DayTime) ? DateTime.Parse(this.DayTime) : (DateTime?)null;
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryViewEventItemTable(entity, out totalcnt);
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
            this.mphArticle.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}