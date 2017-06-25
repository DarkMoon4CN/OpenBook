using Mars.Server.BLL;
using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Entity;
using Mars.Server.Utils;

namespace Mars.Server.WebApp.Templates
{
    public partial class RolesTemplate : TemplateBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        private string _strRolename;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("rolename", RequestMethod.Get)]
        public string StrLoginName
        {
            get { return _strRolename; }
            set { _strRolename = value; }
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
            BCtrl_SysRole bll = new BCtrl_SysRole();
            int totalcnt = 0;

            SysRoleSearchEntity entity = new SysRoleSearchEntity();
            entity.Role_Name = _strRolename;
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;          

            DataTable table = bll.QueryRoleTableByPage(entity, out totalcnt);
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
            this.mphRoleList.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}