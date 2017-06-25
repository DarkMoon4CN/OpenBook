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
    public partial class AdminsTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        private string _strLoginName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("loginname", RequestMethod.Get)]
        public string StrLoginName
        {
            get { return _strLoginName; }
            set { _strLoginName = value; }
        }

        private string _StrTrueName;
        /// <summary>
        /// 姓名
        /// </summary>
        [TemplateProperty("username", RequestMethod.Get)]
        public string StrTrueName
        {
            get { return _StrTrueName; }
            set { _StrTrueName = value; }
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
            BCtrl_SysUser bll = new BCtrl_SysUser();
            int totalcnt = 0;

            AdminSearchEntity entity = new Entity.AdminSearchEntity();
            entity.LoginName = _strLoginName;
            entity.TrueName = _StrTrueName;

            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryAdminTable(entity, out totalcnt);
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
                    message = GetMessage(false),
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

        /// <summary>
        /// 获得消息
        /// </summary>
        /// <returns></returns>
        private string GetMessage(bool isexport)
        {
            string tmpMessage = "";

            tmpMessage = "查询结果：共有 " + TotalRecords + " 个待审批客户";

            return tmpMessage;
        }     

        public override void HandleTemplate()
        {
            this.mphRegUserList.Visible = true;         
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();        
        }
        #endregion
    }
}