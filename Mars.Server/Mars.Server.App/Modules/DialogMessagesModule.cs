using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.App.Modules
{
    public class DialogMessagesModule : ModuleBase
    {
        public DialogMessagesModule()
            : base("DialogMessages")
        {
            Get["GetMessages"] = _ =>
            {
                string shortDate = DateTime.Now.ToString("yyyy-MM-dd");
                string startTime = shortDate + " 00:00:00";
                string endTime = shortDate + " 23:59:59";
                string strWhere = " AND StartTime   BETWEEN  '" + startTime + "' ";
                strWhere += " AND  '" + endTime + "'   ORDER BY  StartTime  DESC ";
                OperationResult<IList<DialogMessagesEntity>> result =
                                       BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere);

                //Android 小于 206  IOS小于2.0.16
                string verson = base.Request.Headers["mars_version"].FirstOrDefault();
                string appkey = base.Request.Headers["mars_appkey"].FirstOrDefault();
                if (appkey.ToLower() == "mars.mobile")
                {
                    int averson = 0;
                    int.TryParse(verson, out averson);
                    if (averson < 206) 
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase()
                        {
                            Status = (int)result.ResultType,
                            Msg = result.GetEnumDescription(result.ResultType)
                        });
                    }
                }
                else if (appkey.ToLower() == "mars.mobile.ios")
                {
                    string[] strSplit = verson.Split('.');
                    int first = strSplit[0].ToInt();
                    if (first < 2) 
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase()
                        {
                            Status = (int)result.ResultType,
                            Msg = result.GetEnumDescription(result.ResultType)
                        });
                    }
                }

                if (result.ResultType == OperationResultType.Success)
                {

                    if (result.AppendData.Count != 0)
                    {
                         DialogMessagesEntity entity = result.AppendData[0];

                        return JsonObj<JsonMessageBase<DialogMessagesEntity>>.ToJson(new JsonMessageBase<DialogMessagesEntity>()
                        {
                            Status = (int)result.ResultType,
                            Msg = result.GetEnumDescription(result.ResultType),
                            Value = entity
                        });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<DialogMessagesEntity>>.ToJson(new JsonMessageBase<DialogMessagesEntity>()
                        {
                            Status = (int)result.ResultType,
                            Msg = result.GetEnumDescription(result.ResultType),
                            Value=new  DialogMessagesEntity()
                        });
                    }
                }
                else
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType)
                    });
                }
            };

        }
    }
}
