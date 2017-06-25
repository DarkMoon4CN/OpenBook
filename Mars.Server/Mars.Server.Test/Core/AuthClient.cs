
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mars.Server.Test.Core
{
    public class AuthClient
    {
        HttpHelper h = new HttpHelper();
        public string TryLogin(string loginname, string pwd, out string sessionid)
        {
            string pwd1 = APIUrlBuilder.fun_MD5(pwd + "+^_^+Mars_V5");
            string data = JsonObj<object>.ToJson(new { LoginName = loginname, Pwd = pwd1 });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string url = APIUrlBuilder.CreateUrl("Auth", "Login", string.Empty, data, out dic);
            string json = h.Post(url, "data=" + System.Web.HttpUtility.UrlEncode(data, Encoding.UTF8), "", Encoding.UTF8, dic);
            JsonMessageBase<UserSessionEntity> jm = JsonObj<JsonMessageBase<UserSessionEntity>>.FromJson(json);
            if (jm.Status == 1)
            {
                G.CurrentSession = jm.Value;
                sessionid = jm.Value.SessionID;
            }
            else
                sessionid = string.Empty;
            return json;
        }

        public string DoAction(string modulename, string methodname, string data, int httpmethod)
        {
            try
            {
                if (G.CurrentSession == null) G.CurrentSession = new UserSessionEntity();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (httpmethod == 0)
                {
                    string url = APIUrlBuilder.CreateUrl(modulename, methodname, G.CurrentSession.SessionID, data, out dic);
                    string json = h.Post(url, "data=" + System.Web.HttpUtility.UrlEncode(data, Encoding.UTF8), "", Encoding.UTF8, dic);
                    return json;
                }
                else
                {
                    string url = APIUrlBuilder.CreateUrlWithGet(modulename, methodname, G.CurrentSession.SessionID, data, out dic);
                    string json = h.Get(url, "", Encoding.UTF8, dic);
                    return json;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
            }
        }

        public string UploadFile(string modulename, string methodname,string localfile)
        {
            try
            {
                if (G.CurrentSession == null) G.CurrentSession = new UserSessionEntity();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string url = APIUrlBuilder.CreateUrl(modulename, methodname, G.CurrentSession.SessionID, string.Empty, out dic);
                string json = h.Upload(url, localfile, dic); //h.Post(url, "data=" + System.Web.HttpUtility.UrlEncode(data, Encoding.UTF8), "", Encoding.UTF8, dic);
                return json;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
            }
        }
    }
}
