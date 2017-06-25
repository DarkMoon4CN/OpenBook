using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.App.Modules
{
    public class MessageModule:ModuleBase
    {
        public MessageModule() : base("/Message") 
        {
            Get["RelateTotal"]=_=>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int userId = CurrentUser.UserID;
                    var result = BCtrl_MobileMessage.Instance.MobileMessage_Total(userId, 1);
                    return JsonObj<JsonMessageBase<int>>.ToJson(new JsonMessageBase<int>()
                    {
                        Status =(int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<int>>.ToJson(new JsonMessageBase<int>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = 0
                    });
                }
            };

            Get["RelateList"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int messageID = data.MessageID;
                    int pageSize = data.PageSize == null ? 20 : data.PageSize;
                    int pageIndex = data.PageIndex == null ? 1 : data.PageIndex;
                    if (pageSize > 50)
                    {
                        pageSize = 50;
                    }
                    int userId = CurrentUser.UserID;
                    var result = BCtrl_MobileMessage.Instance.Relate_GetList(userId,messageID, pageIndex, pageSize);
                    return JsonObj<JsonMessageBase<List<RelateEntity>>>.ToJson(new JsonMessageBase<List<RelateEntity>>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });

                   
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<RelateEntity>>>.ToJson(new JsonMessageBase<List<RelateEntity>>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new List<RelateEntity>()
                    });
                }
            };
        }


    }
}
