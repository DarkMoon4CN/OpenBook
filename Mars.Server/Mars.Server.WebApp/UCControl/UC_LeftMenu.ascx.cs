using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.UCControl
{
    public partial class UC_LeftMenu : Mars.Server.Controls.BaseControl.UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitMeun();
            }
        }

        private void InitMeun()
        {
            string key = string.Format(WebKeys.CK_SYS_LEFTMENU, CurrentAdmin.Sys_UserID);           
       
            if (Session[key]  != null)
            {
                literalMenu.Text = Session[key] as string;
            }
            else
            {
                System.Text.StringBuilder sbstr = new System.Text.StringBuilder("<ul class=\"nav nav-list\">");
                #region 组合左侧菜单树
                List<FunctionEntity> chns = CurrentAdmin.Functions;
                List<FunctionEntity> syschns = chns.FindAll(delegate(FunctionEntity c) { return c.Function_ParentID == 0 && c.Function_IsNew == 1; }).OrderBy(item => item.Function_Order).ToList();

                foreach (FunctionEntity c in syschns)
                {
                    List<FunctionEntity> child_cs = chns.FindAll(delegate(FunctionEntity sc) { return sc.Function_ParentID == c.Function_ID && sc.Function_IsNew == 1; }).OrderBy(item => item.Function_Order).ToList();
                    if (child_cs.Count > 0)
                    {
                        //需要子目录
                        sbstr.Append("<li id=\"li_" + c.Function_ID + "\" >");
                        sbstr.Append("<a href=\"javascript:\" class=\"dropdown-toggle\">");
                        sbstr.Append(" <span>" + c.Function_Name + "</span>");
                        sbstr.Append("<b class=\"arrow icon-angle-down\"></b>");
                        sbstr.Append("</a>");
                        sbstr.Append("<ul class=\"submenu\">");
                        foreach (FunctionEntity _c in child_cs)
                        {
                            sbstr.Append("<li id=\"li_" + _c.Function_ID + "\" pid=\"li_" + c.Function_ID + "\"><a  href='" + (string.IsNullOrEmpty(_c.Function_URL_New) ? "javascript:" : VirtualPathUtility.ToAbsolute(_c.Function_URL_New)) + "?fun=" + _c.Function_ID + "' target='_self'><i class=\"icon-double-angle-right\"></i>" + _c.Function_Name + "</a></li>");
                        }
                        sbstr.Append("</ul>");
                        sbstr.Append("</li>");
                    }
                    else
                    {
                        sbstr.Append("<li id=\"li_" + c.Function_ID + "\">");
                        sbstr.Append("<a href='" + (string.IsNullOrEmpty(c.Function_URL_New) ? "javascript:" : VirtualPathUtility.ToAbsolute(c.Function_URL_New)) + "?fun=" + c.Function_ID + "' target='_self'>");
                        sbstr.Append("<span>" + c.Function_Name + "</span></a>");
                        sbstr.Append("</li>");
                    }
                }
                sbstr.Append("</ul>");
                #endregion           

                Session[key] = sbstr.ToString();
                literalMenu.Text = sbstr.ToString();
            }
        }
    }
}