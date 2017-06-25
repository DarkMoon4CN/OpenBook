using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Mars.Server.Controls.BaseControl
{
   public class AdminControlBase : UserControl
    {
       protected virtual AdminSessionEntity CurrentAdmin
       {
           get
           {
               if (Session[WebKeys.AdminSessionKey] != null)
               {
                   return Session[WebKeys.AdminSessionKey] as AdminSessionEntity;
               }
               else
               {
                   LogUtil.WriteLog(" webApp admin session lost to root");
                   Response.Redirect("~/Logins/Login.aspx");
                   return null;
               }
           }
       }
    }
}
