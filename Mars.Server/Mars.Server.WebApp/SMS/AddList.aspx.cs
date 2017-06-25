using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.SMS
{
    public partial class AddList : System.Web.UI.Page
    {
        public string sUserID = string.Empty;
        public string signSMSModel = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminSessionEntity ue = (AdminSessionEntity)Session[WebKeys.AdminSessionKey];
            if(ue !=null)
            {
               sUserID= ue.Sys_UserID;
            }
           OperationResult<IList<SMSModeEntity>> result= BCtrl_SignSMSModel.Instance.SignSMSModel_GetWhere();
           if (result.ResultType == OperationResultType.Success)
           {
               IList<SMSModeEntity> entitys = result.AppendData;
               foreach (var entity in entitys)
               {
                   signSMSModel += " <option value='" + entity.ModelKey + "'>" + entity.ModelKey + "</option> ";
               }
           }
        }
    }
}