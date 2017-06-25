using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Controls.BaseControl
{
   public class BasePage : System.Web.UI.Page
    {
       protected override void OnPreInit(EventArgs e)
       {
           if (CurrentAdmin != null)
           {

           }
           else
           {
               //LogUtil.WriteLog(" webApp admin session lost to root");
               string returnUrl = Request.Url.PathAndQuery;
               Response.Redirect("~/Logins/Login.aspx?returnUrl=" + returnUrl);               
           }
           base.OnPreInit(e);
       }

       /// <summary>
       /// 当前用户信息
       /// </summary>
       protected AdminSessionEntity CurrentAdmin
       {
           get
           {
               if (Session[WebKeys.AdminSessionKey] != null)
               {
                   return Session[WebKeys.AdminSessionKey] as AdminSessionEntity;
               }
               else
               {                   
                   return null;
               }
           }
       }
    }
}
