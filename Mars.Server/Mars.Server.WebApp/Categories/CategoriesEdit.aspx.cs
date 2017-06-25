using Mars.Server.BLL;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Categories
{
    public partial class CategoriesEdit : System.Web.UI.Page
    {
        int CategoriesID = 0;
        int pid = 0;
        BCtrl_Categories catebll = new BCtrl_Categories();
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["id"], out CategoriesID);
            int.TryParse(Request.QueryString["pid"], out pid); 
            if (!IsPostBack)
            {
                InitData();
            }
            else
            {
                SaveCategoriesBySubmit();
            }
        }
        private void InitData()
        {
            this.hidPid.Value = CategoriesID.ToString();
            List<CategoriesEntity> CategoriesList = catebll.CategoriesFirstLevel();
            foreach (CategoriesEntity item in CategoriesList)
            {
                this.selCategoriesLevel.Items.Add(new ListItem(item.CalendarTypeName, item.CalendarTypeID.ToString()));
            }
            if (CategoriesID > 0)  //修改
            {
                CategoriesEntity CategoriesOne = catebll.QueryCategories(CategoriesID);
                if (int.Parse(CategoriesOne.ParentCalendarTypeID.ToString()) > 0)   //二级分类
                { this.selCategoriesLevel.Value = CategoriesOne.ParentCalendarTypeID.ToString(); }
                this.txtCategoriesName.Value = CategoriesOne.CalendarTypeName.ToString();
            }
        }
        private void SaveCategoriesBySubmit()
        {
            CategoriesEntity catEntity = new CategoriesEntity();
            catEntity.CalendarTypeName = this.txtCategoriesName.Value.Trim();
            int selFunLevel = int.Parse(this.selCategoriesLevel.Value);
            bool isSuccess = false;
            if (CategoriesID > 0)
            {
                #region 修改
                catEntity.CalendarTypeID = CategoriesID;
                catEntity.CalendarTypeKind = 2;
                if (selFunLevel == 0)
                {
                    #region  修改一级分类

                    catEntity.ParentCalendarTypeID = -1;

                    isSuccess = catebll.Categoriesedit(catEntity);
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
                else  //修改二级分类
                {
                    catEntity.ParentCalendarTypeID = selFunLevel;
                    isSuccess = catebll.Categoriesedit(catEntity);
                    if (isSuccess)
                    {
                        RetrunPageAlert(enumPageAlert.Success);
                    }
                    else
                    {
                        RetrunPageAlert(enumPageAlert.Fail);
                    }
                }
                #endregion
            }
            else
            {
                #region 保存
                if (selFunLevel == 0)
                {
                    #region 新建一级分类
                    catEntity.ParentCalendarTypeID = -1;
                    catEntity.CalendarTypeKind = 2;

                    isSuccess = catebll.Insert(catEntity);
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
                    #region 新建二级分类
                    catEntity.CalendarTypeName = this.txtCategoriesName.Value.Trim();
                    catEntity.CalendarTypeKind = 2;
                    catEntity.ParentCalendarTypeID = selFunLevel;

                    isSuccess = catebll.Insert(catEntity);

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
        /// <summary>
        /// 更新 返回页面结果提示对话框
        /// </summary>
        private void RetrunPageAlert(enumPageAlert enumPage)
        {
            string aa = (CategoriesID > 0) ? "修改" : "保存";
            string bb = (pid > 0) ? "CategoriesSecondManager" : "CategoriesManager";
            if (enumPage == enumPageAlert.Success)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CategoriesEdit", "<script>bootbox.dialog('" + aa + "成功!', [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () { window.location.href = '" + bb + ".aspx?fun=" + Master.fun + "';}}]);</script>");
            }
            else if (enumPage == enumPageAlert.Fail)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CategoriesEdit", "<script>bootbox.alert(’" + aa + "失败!');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CategoriesEdit", "<script>bootbox.alert(\"传输数据异常!\");</script>");
            }
        }
    }
}