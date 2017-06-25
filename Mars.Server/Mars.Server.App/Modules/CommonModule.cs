using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class CommonModule : ModuleBase
    {
        public CommonModule()
            : base("/Common")
        {
            BCtrl_Common commonobj = new BCtrl_Common();
            //获取服务器时间
            Get["FetchServerTime"] = _ =>
            {
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Tag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            };

            Post["/BindPhone"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string phone = data.Phone;
                    string vcode = data.VCode;
                    string pwd = data.Pwd;
                    string msg = string.Empty;
                    if (CurrentUser != null && SmsMananger.BindPhone(phone, vcode, CurrentUser.UserID, pwd, out msg))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "绑定成功！！" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = string.IsNullOrEmpty(msg) ? "当前用户无效" : msg });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["AddFeedback"] = _ =>
            {
                try
                {
                    FeedbackEntity e = FetchFormData<FeedbackEntity>();
                    e.UserID = CurrentUser.UserID;
                    bool state = commonobj.AddFeedback(e);
                    if (state)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "保存成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "保存失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };


            Post["/BindSns"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string thirdid = data.ThirdID; //Request.Form.ThirdID;
                    string username = data.ThirdUserName;//Request.Form.ThirdUserName;
                    int type = data.ThirdType; //Request.Form.ThirdType;
                    string picurl = data.Url;
                    BCtrl_Auth authobj = new BCtrl_Auth();
                    UserSessionEntity ue = authobj.IsThereExistUser(username, thirdid, type, picurl);
                    if (ue != null) 
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = username +" 已是APP用户,无需再次绑定！" });
                    }

                    bool rt = new BCtrl_Auth().AddSnsBinding(CurrentUser.UserID, username, thirdid, type, picurl);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = rt ? 1 : 0, Msg = rt ? "绑定成功！" : "绑定失败!请稍后重试" });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "服务器连接失败，请稍后重试" });
                }


                //UserSessionEntity ue = authobj.TryLoginX(username, thirdid, type, picurl);
                //if (ue != null)
                //{
                //    string sessionid = SessionCenter.AddSessionIdentity(ue);
                //    if (!string.IsNullOrEmpty(sessionid))
                //    {
                //        ue.SessionID = sessionid;
                //        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 1, Msg = "登录成功", Value = ue });
                //    }
                //    else
                //    {
                //        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "创建登录会话失败，请稍后重试！" });
                //    }
                //}
                //else
                //{
                //    return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "登录失败" });
                //}
            };

            #region  根据版本号，获取到时候可以打开内容
            Get["/CheckState"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string verCode = data.VerCode; //Request.Form.ThirdID;
                    BCtrl_Common commonBll = new BCtrl_Common();
                    bool isCheck = commonBll.IsVersionCheck(verCode);
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>() { Status = 1, Msg = "数据传输完成！！", Value = isCheck });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "服务器连接失败，请稍后重试" });
                }
            };
            #endregion
        }
    }
}
