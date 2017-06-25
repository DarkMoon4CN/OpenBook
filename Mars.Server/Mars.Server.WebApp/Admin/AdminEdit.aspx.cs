using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;

namespace Mars.Server.WebApp.Admin
{
    public partial class AdminEdit : Mars.Server.Controls.BaseControl.BasePage
    {        
        private int pid;

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out pid);
            if (!IsPostBack)
            {
                InitData();

                if (pid > 0)
                {
                    BindAdminData();
                }
            }
            else
            {
                SubmitForm();
            }
        }

        private void BindAdminData()
        {         
            BCtrl_SysUser bllSysuser = new BCtrl_SysUser();

            AdminEntity entity = bllSysuser.QuerySysUserEntity(pid);

            if (entity != null)
            {
                this.txtLoginname.Disabled = true;
                this.divUserpwd.Visible = false;
                this.divConfirmuserpwd.Visible = false;

                this.txtLoginname.Value = entity.User_Name;
                this.txtTruename.Value = entity.TrueName;
                this.selsex.Value = entity.User_Sex.ToString();
                this.txtUser_Tel.Value = entity.User_Tel;
                this.txtUser_Mobile.Value = entity.User_Mobile;
                this.txtUser_Mail.Value = entity.User_Mail;
                this.selDepartMent.Value = entity.User_DeptID.ToString();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.dialog(\"参数异常!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () { window.location.href = 'AdminManager.aspx?fun="+ Master.fun +"';}}]);</script>");
            }
        }

        private void InitData()
        {
            this.hidPid.Value = pid.ToString();
            this.hidfun.Value = Master.fun.ToString();

            BCtrl_Department bllDepartment = new BCtrl_Department();
            List<DepartmentEntity> list = bllDepartment.QueryDepartments();

            foreach (DepartmentEntity entity in list)
            {
                selDepartMent.Items.Add(new ListItem(entity.Dept_Name,entity.Dept_ID.ToString()));
            }
            
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        private void SubmitForm()
        {
            if (ValidateData())
            {
                BCtrl_SysUser bllSysuser = new BCtrl_SysUser();
                bool isSuccess = false;
                AdminEntity entity = null;

                if (pid > 0)
                {
                    #region 修改
                    entity = bllSysuser.QuerySysUserEntity(pid);
                    if (entity != null)
                    {
                        entity.TrueName = this.txtTruename.Value.Trim();                       
                        entity.User_Sex = int.Parse(this.selsex.Value);
                        entity.User_Tel = this.txtUser_Tel.Value.Trim();
                        entity.User_Mobile = this.txtUser_Mobile.Value.Trim();
                        entity.User_Mail = this.txtUser_Mail.Value.Trim();
                        entity.User_DeptID = int.Parse(this.selDepartMent.Value);

                        isSuccess = bllSysuser.Update(entity);

                        if (isSuccess)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.dialog(\"保存成功!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () {window.location.href = 'AdminManager.aspx?fun="+ Master.fun +"';}}]);</script>");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.alert(\"保存失败\");</script>");
                        }

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.dialog(\"当前用户不存在或已被删除!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () { window.location.href = 'AdminManager.aspx?fun="+ Master.fun +"';}}]);</script>");                       
                    }
                    #endregion
                }
                else
                {
                    #region 新增
                    entity = new AdminEntity();
                    entity.User_Name = this.txtLoginname.Value.Trim();
                    entity.TrueName = this.txtTruename.Value.Trim();
                    entity.User_Pwd =  MD5.Encode(WebKeys.AdminPwdRandom,this.txtUserpwd.Value.Trim());
                    entity.User_Sex = int.Parse(this.selsex.Value);
                    entity.User_Tel = this.txtUser_Tel.Value.Trim();
                    entity.User_Mobile = this.txtUser_Mobile.Value.Trim();
                    entity.User_Mail = this.txtUser_Mail.Value.Trim();
                    entity.User_DeptID = int.Parse(this.selDepartMent.Value);

                    entity.IsValid = false;
                    entity.RegisterDate = DateTime.Now;

                    isSuccess = bllSysuser.Insert(entity);

                    if (isSuccess)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.dialog(\"添加成功!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () {window.location.href = 'AdminManager.aspx?fun="+ Master.fun +"';}}]);</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.alert(\"添加失败\");</script>");
                    }
                    #endregion
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "adminEdit", "<script>bootbox.alert(\"请检查必填项或数据格式是否正确\");</script>");
            }
        }

        /// <summary>
        /// 再一次验证数据
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            return true;
        }
    }
}