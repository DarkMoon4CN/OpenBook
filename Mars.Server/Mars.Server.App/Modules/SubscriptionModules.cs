using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.App.Modules
{
    public class SubscriptionModules : ModuleBase
    {
        public SubscriptionModules()
            : base("Subscription")
        {
            #region 获取用户订阅列表
            Get["List"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    bool isHeader = data.IsHeader;
                    int headerID = data.HeaderID;
                    int headerSize = data.HeaderSize;
                    int headerIndex = data.HeaderIndex == null ? 1 : data.HeaderIndex;

                    bool isFooter = data.IsFooter;
                    int footerID = data.FooterID;
                    int footerSize = data.FooterSize;
                    int footerIndex = data.FooterIndex == null ? 1 : data.FooterIndex;
                    int userId = CurrentUser.UserID;
                    string keyword = data.Keyword==null ?string.Empty :data.Keyword;

                    List<SubscriptionEntity> header = null;
                    List<SubscriptionEntity> footer = null;
                    BCtrl_Subscription.Instance.Subscription_GetList(isHeader, headerID, headerSize, headerIndex
                                                                    , isFooter, footerID, footerSize, footerIndex
                                                                    , userId,keyword, out header, out footer);
                    var jsonBase = new JsonMessageBase<List<SubscriptionEntity>, List<SubscriptionEntity>>();
                    jsonBase.Status = 1;
                    jsonBase.Msg = "订阅列表抓取完成！";
                    jsonBase.Value = header == null ? null : header;
                    jsonBase.Value2 = footer == null ? null : footer;
                    return JsonObj<JsonMessageBase<List<SubscriptionEntity>, List<SubscriptionEntity>>>.ToJson(jsonBase);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据提交不完全" });
                }
            };
            #endregion 

            #region  批量添加订阅
            Get["Add"] = _ =>
            {
                try
                {
                    List<int> subIDs = new List<int>();
                    subIDs = FecthQueryData<List<int>>();
                    int userID = CurrentUser.UserID;
                    var result = BCtrl_Subscription.Instance.Subscription_Insert(userID, subIDs);
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据提交不完全" });
                }
            };
            #endregion 

            #region  批量取消订阅
            Get["Del"] = _ =>
            {
                try
                {
                    List<int> subIDs = new List<int>();
                    subIDs = FecthQueryData<List<int>>();
                    int userID = CurrentUser.UserID;
                    var result = BCtrl_Subscription.Instance.Subscription_Del(userID, subIDs);
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据提交不完全" });
                }
            };
            #endregion

            #region 订阅号详情
            Get["Detail"] = _ => 
            {
                dynamic data = FecthQueryData();
                bool isHeader = data.IsHeader;
                int subID=data.SubID;
                int userID=CurrentUser.UserID;
                OperationResult<SubscriptionEntity> header = null;
                OperationResult<List<EventItemEntity>> result = null;
                if (isHeader)
                {
                    header = BCtrl_Subscription.Instance.Subscription_Get(userID,subID);
                }
                var jsonBase = new JsonMessageBase<SubscriptionEntity, List<EventItemEntity>>();
                jsonBase.Status = (int)result.ResultType;
                jsonBase.Msg = result.Message;
                jsonBase.Value = header == null ? null : header.AppendData;
                jsonBase.Value2 = result == null ? null : result.AppendData;
                return JsonObj<JsonMessageBase<SubscriptionEntity, List<EventItemEntity>>>.ToJson(jsonBase);
            };
            #endregion
        }
    }
}
