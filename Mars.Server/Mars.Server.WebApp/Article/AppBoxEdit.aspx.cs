using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Article
{
    public partial class AppBoxEdit : System.Web.UI.Page
    {
        public DialogMessagesEntity entity = new DialogMessagesEntity();
        public string startTime = string.Empty;
        public int isTiming = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            string messageID=Request["mid"];
            if(!string.IsNullOrEmpty(messageID))
            {
                int mid=0;
                int.TryParse(messageID,out mid);
                string strWhere =string.Empty;
                strWhere=" AND MessageID ={0}";
                strWhere= string.Format(strWhere,mid);
                OperationResult<IList<DialogMessagesEntity>> result=
                             BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere);

                if (mid == 0 || result.AppendData.Count == 0)
                {
                    Response.Write("无此条目信息！");
                    Response.End();
                }
                if (result.ResultType == OperationResultType.Success  && result.AppendData.Count !=0) 
                {
                    entity = result.AppendData.FirstOrDefault();
                    startTime = result.AppendData.FirstOrDefault().StartTime.Value.ToString("yyyy-MM-dd  HH:mm:ss");
                    DateTime? startTimeFlag = result.AppendData.FirstOrDefault().StartTime;
                    if (startTimeFlag.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {

                        string shortDate = DateTime.Now.ToString("yyyy-MM-dd");
                        string startTime2 = shortDate + " 00:00:00";
                        string endTime = shortDate + " 23:59:59";
                        string strWhere2 = " AND StartTime   BETWEEN  '" + startTime2 + "' ";
                        strWhere2 += " AND  '" + endTime + "'   ORDER BY  StartTime  DESC ";
                        OperationResult<IList<DialogMessagesEntity>> result2 =
                                               BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere2);
                        if (mid == result2.AppendData.FirstOrDefault().MessageID) 
                        {
                            isTiming = 0;
                        }
                       
                    }
                }
            }
            
        }
    }
}