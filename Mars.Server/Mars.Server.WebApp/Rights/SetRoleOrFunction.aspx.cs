using Mars.Server.BLL;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Rights
{
    public partial class SetRoleOrFunction : Mars.Server.Controls.BaseControl.BasePage
    {
        protected string userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            userId = string.IsNullOrEmpty(Request.QueryString["uid"]) ? "" : Request.QueryString["uid"];

            if (!IsPostBack)
            {
                InitRoleList();
                InitFunctionList();
                if (!string.IsNullOrEmpty(userId))
                {
                    //初始化权限信息
                    GetFunByUserID();
                    GetRoleByUserID();
                }
            }
        }

        private void GetFunByUserID()
        {
            List<FunctionEntity> list = new BCtrl_Function().GetFunction(userId);
            foreach (FunctionEntity var in list)
            {
                userfun.Value += var.Function_ID;
                userfun.Value += ",";
            }
            if (userfun.Value != "")
            {
                userfun.Value = userfun.Value.Substring(0, userfun.Value.Length - 1);
            }
        }
        private void GetRoleByUserID()
        {
            int roleid = new BCtrl_SysUser().GetRoleByUserID(userId);
            hidrole.Value = roleid.ToString();
            hidOriginRole.Value = roleid.ToString();
        }

        private void InitRoleList()
        {
            DataTable dt = new BCtrl_SysUser().GetAllRole();
            this.rptRole.DataSource = dt;
            this.rptRole.DataBind();
        }

        private void InitFunctionList()
        {
            BCtrl_Function fbll = new BCtrl_Function();

            function_treeview.InnerHtml = fbll.GetFunctionTree();
        }
    }
}