using Mars.Server.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Entity;

namespace Mars.Server.WebApp.Rights
{
    public partial class RoleEdit : Mars.Server.Controls.BaseControl.BasePage
    {
        BCtrl_Function fbll = new BCtrl_Function();
        BCtrl_SysRole roleBll = new BCtrl_SysRole();

        int roleID = 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"],out roleID);
            
            if (!IsPostBack)
            {
                InitFunctionTree();

                if (roleID > 0) //编辑
                {
                    SysRoleEntity roleEntity = roleBll.QueryEntity(roleID);
                    this.txtRoleName.Value = roleEntity.Role_Name;

                    this.hidpid.Value = roleID.ToString();
                    List<FunctionEntity> funList = fbll.QueryFunctiolnList(roleID);
                    
                    string[] funArray = funList.Select(fun => fun.Function_ID.ToString()).ToArray();
                    this.userfun.Value = string.Join(",", funArray);
                }           
            }
        }

        private void InitFunctionTree()
        {
            hidfun.Value = Master.fun.ToString();
            function_treeview.InnerHtml = fbll.GetFunctionTree();
        }
    }
}