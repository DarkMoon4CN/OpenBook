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
    public partial class AppUpdateTemplate : TemplateBase
    {
        #region
        [TemplateProperty("selAppType", RequestMethod.Get)]
        public string selAppType { get; set; }
        [TemplateProperty("txtVersion", RequestMethod.Get)]
        public string txtVersion { get; set; }
        [TemplateProperty("txtStartTime", RequestMethod.Get)]
        public string txtStartTime { get; set; }
        [TemplateProperty("txtEndTime", RequestMethod.Get)]
        public string txtEndTime { get; set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(false);
        }
        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        } 
        private DataTable GetQueryData(bool isDownload)
        {
            searchAppUpdateEntity entity = new searchAppUpdateEntity();
            int totalcnt = 0;
            int AppType = 0;
            BCtrl_Common bll = new BCtrl_Common();
            if (!string.IsNullOrEmpty(txtStartTime))
            {
                string.Format("{0} 00:00:00", txtStartTime);
                entity.txtStartTime = txtStartTime;
            }
            if (!string.IsNullOrEmpty(txtEndTime))
            {
                string.Format("{0} 23:59:59", txtEndTime);
                entity.txtEndTime = txtEndTime;
            }
            if (!string.IsNullOrEmpty(txtVersion))
            {
                entity.txtVersion = txtVersion;
            }
            int.TryParse(selAppType, out AppType);
            entity.selAppType=AppType;
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;
            entity.OrderfieldType = OrderFieldType.Desc;
            DataTable db = bll.GetAppUpdateList(entity, out totalcnt);
            base.TotalRecords = totalcnt;
            return db;
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
                DataTable dt = GetQueryData(false);

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
        #endregion
    }
}