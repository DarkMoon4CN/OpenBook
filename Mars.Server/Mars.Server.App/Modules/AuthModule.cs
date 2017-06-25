using Mars.Server.App.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Mars.Server.Utils;
using Mars.Server.BLL;
using Nancy;
using System.IO;
namespace Mars.Server.App.Modules
{
    public class AuthModule : ModuleBase
    {
        BCtrl_Auth authobj = new BCtrl_Auth();
        BCtrl_Common commonobj = new BCtrl_Common();
        public AuthModule()
            : base("Auth")
        {

            Get["/GetLog/{date}"] = _ => {
                try
                {
                    int d = _.date;
                    var response = new Response();
                    response.Headers.Add("Content-Disposition", "attachment; filename=log.txt");
                    response.ContentType = "application/octet-stream";
                    string dir = Path.Combine(AppPath.LogFolder, d.ToString());
                    if (Directory.Exists(dir))
                    {
                        string[] files = Directory.GetFiles(dir,"*.txt");
                        if (files.Length > 0)
                        {
                            byte[] buffer = File.ReadAllBytes(files[0]);
                            response.Contents = stream =>
                            {
                                using (BinaryWriter bw = new BinaryWriter(stream))
                                {

                                    bw.Write(buffer);
                                }
                            };
                            return response;
                        }
                        else
                            return null;
                       
                    }
                    else
                        return null; 
                    
                }
                catch {
                    return null;
                }
            };

            Post["/Login"] = _ =>
            {

                dynamic data = FetchFormData();
                string loginname = data.LoginName;
                string pwd = data.Pwd;
                string msg = string.Empty;

                
                bool state = authobj.CheckUserExist(loginname);
                if (!state) 
                {
                    return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "你所登录的账号不存在！" });
                }
                UserSessionEntity ue = authobj.TryLogin(loginname, pwd, out msg);
                if ( ue!=null)
                {
                    string sessionid = SessionCenter.AddSessionIdentity(ue);
                    if (!string.IsNullOrEmpty(sessionid))
                    {
                        ue.SessionID = sessionid;
                        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 1, Msg = "登录成功", Value = ue });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "创建登录会话失败，请稍后重试！" });
                    }
                }
                else
                {
                    return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = msg });
                }
            };

            Post["/LoginX"] = _ =>
            {
                 dynamic data = FetchFormData();
                 string thirdid = data.ThirdID; //Request.Form.ThirdID;
                 string username = data.ThirdUserName;//Request.Form.ThirdUserName;
                 int type = data.ThirdType; //Request.Form.ThirdType;
                 string picurl = data.Url;
                UserSessionEntity ue = authobj.TryLoginX(username, thirdid, type,picurl);
                if (ue!=null)
                {
                    string sessionid = SessionCenter.AddSessionIdentity(ue);
                    if (!string.IsNullOrEmpty(sessionid))
                    {
                        ue.SessionID = sessionid;
                        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 1, Msg = "登录成功", Value = ue });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "创建登录会话失败，请稍后重试！" });
                    }
                }
                else
                {
                    return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "登录失败" });
                }
            };

            Post["/Register"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string phone = data.Phone;
                    string code = data.VCode;
                    string pwd = data.Pwd;
                    string msg = string.Empty;
                    string nickName = string.Empty;
                    int userid = authobj.RegesiterNewUser(phone, pwd, code, out msg,out nickName);
                    if (userid > 0)
                    {
                        UserSessionEntity ue = new UserSessionEntity();
                        ue.UserID = userid;
                        ue.ZoneID = 0;
                        ue.PicturePath = AppUtil.UserDefaultHeader + AppUtil.ConvertJpg;
                        string sessionid = SessionCenter.AddSessionIdentity(ue);
                        if (!string.IsNullOrEmpty(sessionid))
                        {
                            ue.SessionID = sessionid;
                            ue.NickName = nickName;
                            return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 1, Msg = "注册成功", Value = ue });
                        }
                        else
                        {
                            return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "创建登录会话失败，请稍后重试！" });
                        }
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = msg }); 
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<UserSessionEntity>>.ToJson(new JsonMessageBase<UserSessionEntity>() { Status = 0, Msg = "注册失败" });
                }


            };

            Post["/CheckUserExist"] = _ => {
                try
                {
                    dynamic data=FetchFormData();
                    string loginname=data.LoginName;
                    bool state = authobj.CheckUserExist(loginname);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg ="检测成功",Tag=state.ToString() });
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/SendVCode"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string phone = data.Phone;
                    string msg = string.Empty;
                    if (SmsMananger.SendCode(phone, out msg))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = msg });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = msg });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/ResetMyPassword"] = _ => {
                try
                {
                    dynamic data = FetchFormData();
                    string phone = data.Phone;
                    string vcode = data.VCode;
                    string pwd = data.Pwd;
                    string msg = string.Empty;
                    if (authobj.ResetPassword(phone, vcode, pwd, out msg))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "重置密码成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = string.IsNullOrEmpty(msg) ? "重置失败" : msg });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Get["/CheckAppUpdateState"] = _ =>
            {
                try
                {
                    var data = FecthQueryData();
                    int appid = data.AppType;
                    string ver = data.Version;
                    int versionType = 0;
                    try
                    {
                        versionType = data.VersionType;
                    }
                    catch 
                    {
                        versionType=0;
                    }
                    AppUpdateEntity info = new AppUpdateEntity();
                    bool state = false;
                    if (versionType == 0)
                    {
                        state = commonobj.CheckAppUpdateState(appid, ver, out info);
                    }
                    else 
                    {
                        state = commonobj.CheckAppUpdateState(appid, ver,versionType, out info);
                    }

                    if (state)
                    {
                        info.NeedUpdate = true;
                        return JsonObj<JsonMessageBase<AppUpdateEntity>>.ToJson(new JsonMessageBase<AppUpdateEntity>() { Status = 1, Msg = "有新版本", Tag = "1", Value = info });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<AppUpdateEntity>>.ToJson(new JsonMessageBase<AppUpdateEntity>() { Status = 1, Msg = "程序为最新版不需要更新", Tag = "0", Value = new AppUpdateEntity() });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase<AppUpdateEntity>>.ToJson(new JsonMessageBase<AppUpdateEntity>() { Status = 0, Msg = ex.Message, Value = new AppUpdateEntity() });
                }
            };

            #region  改变客户签到状态 2015/11/12
            Get["UpdateSignState"] = _ =>
            {
                dynamic data = FecthQueryData();
                string customerKey = data.CustomerKey;
                string stateStr = data.State;
                int state = 0;
                int.TryParse(stateStr, out state);
                IList<SignBookEntity> entity = BCtrl_SignBook.Instance.SignBook_Get(customerKey);
                if (entity == null)
                { 
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据查询异常，无法连接服务器" });
                }
                else if (entity.Count == 0)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "不是有效报名用户，不能签到！" });
                }
                else if (entity[0].IsSign == 1)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = " 重复签到 " + entity[0].Company + "  " + entity[0].Customer });
                }
                bool flag = BCtrl_SignBook.Instance.SignBook_Update(customerKey, state);
                if (!flag)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据查询异常，无法连接服务器" });
                }
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg =  entity[0].Company + " " + entity[0].Customer +  " 签到成功！" });
            };
            #endregion 


        }
    }
}
