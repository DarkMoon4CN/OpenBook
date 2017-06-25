using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.BLL;
using Mars.Server.Entity;

namespace Mars.Server.WebApp.Rights
{
    public partial class FunctionEdit : System.Web.UI.Page
    {
        int functionID = 0;

        BCtrl_Function bllFunc = new BCtrl_Function();
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out functionID);          

            if (!IsPostBack)
            {
                InitData();
            }
            else
            {
                SaveFunctionBySubmit();
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void InitData()
        {
            this.hidPid.Value = functionID.ToString();
            this.hidfun.Value = Master.fun.ToString();

            List<FunctionEntity> funcList = bllFunc.QueryFirstLevelList();

            foreach (FunctionEntity item in funcList)
            {
                this.selFunctionLevel.Items.Add(new ListItem(item.Function_Name, item.Function_ID.ToString()));
            }

            if (functionID == 0)
            {
                //首次加载设置
                this.txtFunctionUrl.Disabled = true;
                this.txtFunctionOrder.Value = "1000";
            }
            else
            {
                FunctionEntity entity = bllFunc.QueryFunction(functionID);
                this.selFunctionLevel.Value = entity.Function_ParentID.ToString();
                this.txtFunctionName.Value = entity.Function_Name;
                this.txtFunctionOrder.Value = entity.Function_Order.ToString();

                if (entity.Function_Level == 1)
                {
                    this.txtFunctionUrl.Disabled = true;     
                    this.txtFunctionUrl.Value = "";
                }
                else
                {
                    this.txtFunctionUrl.Value = entity.Function_URL_New;
                }                
            }            
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveFunctionBySubmit()
        {
            if (ValidForm())
            {
                if (functionID > 0)
                {
                    #region 修改
                    FunctionEntity updateEntity = bllFunc.QueryFunction(functionID);

                    if (updateEntity != null)
                    {
                        int selFunLevel = int.Parse(this.selFunctionLevel.Value);
                        if (selFunLevel == 0)
                        {
                            #region 修改成为一级菜单
                            updateEntity.Function_Name = this.txtFunctionName.Value.Trim();
                            updateEntity.Function_Order = this.txtFunctionOrder.Value.Trim();

                            updateEntity.Function_URL_New = string.Empty;
                            updateEntity.Function_ParentID = 0;
                            updateEntity.Function_Level = 1;

                            bool isSuccess = bllFunc.Update(updateEntity);

                            if (isSuccess)
                            {                               
                                RetrunPageAlert(enumPageAlert.Success);
                            }
                            else
                            {
                                RetrunPageAlert(enumPageAlert.Fail);
                            }
                            #endregion
                        }
                        else
                        {
                            #region 修改二级菜单
                            updateEntity.Function_Name = this.txtFunctionName.Value.Trim();
                            updateEntity.Function_URL_New = this.txtFunctionUrl.Value.Trim();
                            updateEntity.Function_Order = this.txtFunctionOrder.Value.Trim();

                            updateEntity.Function_Level = 2;
                            updateEntity.Function_ParentID = selFunLevel;

                            bool isSuccess = bllFunc.Update(updateEntity);
                            if (isSuccess)
                            {
                                RetrunPageAlert(enumPageAlert.Success);
                            }
                            else
                            {
                                RetrunPageAlert(enumPageAlert.Fail);
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        this.RetrunPageAlert(enumPageAlert.NoExists);
                    }
                    #endregion
                }
                else
                {
                    #region 保存
                    FunctionEntity funEntity = new FunctionEntity();
                    funEntity.Function_IsNew = 1;
                    funEntity.Function_isValid = 1;
                    funEntity.Function_Order = this.txtFunctionOrder.Value.Trim();
                    funEntity.Function_Name = this.txtFunctionName.Value.Trim();
                    funEntity.CreateDate = DateTime.Now;

                     int selFunLevel = int.Parse(this.selFunctionLevel.Value);
                    if (selFunLevel == 0)
                    {
                        #region 新建一级菜单
                        funEntity.Function_URL_New = string.Empty;
                        funEntity.Function_ParentID = 0;
                        funEntity.Function_Level = 1;

                        bool isSuccess = bllFunc.Insert(funEntity);
                        if (isSuccess)
                        {
                            RetrunPageAlert(enumPageAlert.Success);
                        }
                        else
                        {
                            RetrunPageAlert(enumPageAlert.Fail);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 新建二级菜单                      
                        funEntity.Function_URL_New = this.txtFunctionUrl.Value.Trim();
                        funEntity.Function_Level = 2;
                        funEntity.Function_ParentID = selFunLevel;

                        bool isSuccess = bllFunc.Insert(funEntity);

                        if (isSuccess)
                        {
                            RetrunPageAlert(enumPageAlert.Success);
                        }
                        else
                        {
                            RetrunPageAlert(enumPageAlert.Fail);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 验证表单
        /// </summary>
        /// <returns></returns>
        private bool ValidForm()
        {
            return true;
        }

        /// <summary>
        /// 更新 返回页面结果提示对话框
        /// </summary>
        private void RetrunPageAlert(enumPageAlert enumPage)
        {
            if (enumPage == enumPageAlert.Success)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "functionEdit", "<script>bootbox.dialog(\"保存成功!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () { window.location.href = 'FunctionManager.aspx?fun="+ Master.fun +"';}}]);</script>");
            }
            else if (enumPage == enumPageAlert.Fail)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "functionEdit", "<script>bootbox.alert(\"保存失败!\");</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "functionEdit", "<script>bootbox.alert(\"传输数据异常!\");</script>");
            }
        }

    }  
}