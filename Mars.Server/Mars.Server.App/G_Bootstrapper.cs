using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Security;
using Nancy.Bootstrapper;
using System.IO;
using System.IO.Compression;
using Mars.Server.App.Core;
using Mars.Server.Entity;
using Mars.Server.Utils;

namespace SparrowF.Server
{
    /// <summary>
    /// https://github.com/NancyFx/Nancy/wiki/Documentation
    /// </summary>
    public class G_Bootstrapper : DefaultNancyBootstrapper
    {

        public static void AddGZip(IPipelines pipelines)
        {
            pipelines.AfterRequest += ctx =>
            {
                if ((!ctx.Response.ContentType.Contains("application/json")) || !ctx.Request.Headers.AcceptEncoding.Any(x => x.Contains("gzip")))
                    return;
                var jsonData = new MemoryStream();
                ctx.Response.Contents.Invoke(jsonData);
                jsonData.Position = 0;
                if (jsonData.Length < 4096)
                {
                    ctx.Response.Contents = s =>
                    {
                        jsonData.CopyTo(s);
                        s.Flush();
                    };
                }
                else
                {
                    ctx.Response.Headers["Content-Encoding"] = "gzip";
                    ctx.Response.Contents = s =>
                    {
                        var gzip = new GZipStream(s, CompressionMode.Compress, true);
                        jsonData.CopyTo(gzip);
                        gzip.Close();
                    };
                }
            };
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
        }
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            string gzipflag = System.Configuration.ConfigurationManager.AppSettings["EnableGzip"] ?? "1";
            if (gzipflag == "1")
            {
                AddGZip(pipelines);
            }
            
            pipelines.BeforeRequest += ctx =>
            {
                bool valid = false;
                string msg = string.Empty;
                try
                {
                    //LogUtil.WriteLog(ctx.Request.Url.ToString());
                    //放过登录入口
                    string url = ctx.Request.Path;
                    IList<string> freePass = new List<string>();//免除通行证
                    freePass.Add("/Auth/");
                    freePass.Add("/Exhibition/");
                    freePass.Add("/SignBook/");
                    freePass.Add("/DialogMessages/");
                    freePass.Add("/Article/ArticleCommon");
                    for (int i = 0; i < freePass.Count; i++)
                    {
                        if (url.Contains(freePass[i]))
                        {
                            return null;
                        }
                    }
                    string appkey = ctx.Request.Headers["mars_appkey"].FirstOrDefault();

                    //LogUtil.WriteLog(appkey==null ? "NULL" : appkey);
                    //LogUtil.WriteLog(AppServerDataInitializer.AppClients.Count > 0 ? AppServerDataInitializer.AppClients[appkey].AppKey : "");

                    if (appkey != null && AppServerDataInitializer.AppClients.ContainsKey(appkey))
                    {
                        string token = ctx.Request.Headers["mars_token"].FirstOrDefault();
                        string sessionid = ctx.Request.Headers["mars_sid"].FirstOrDefault();
                        string tick = ctx.Request.Headers["mars_tick"].FirstOrDefault();
                        string version = ctx.Request.Headers["mars_version"].FirstOrDefault();
                        string method = ctx.Request.Method.ToLower();
                        if (!string.IsNullOrEmpty(token))
                        {
                            SessionIdentity si = SessionCenter.GetIdentity(sessionid);
                           
                            if (si != null)
                            {
                                string data = method == "get" ? ctx.Request.Query.data : ctx.Request.Form.data;
                                string token1 = GetSign(data, tick, appkey, sessionid, version, AppServerDataInitializer.AppClients[appkey].AppSecrect);

                                StringBuilder sblog = new StringBuilder();

                                sblog.AppendFormat(",Path:{0}",ctx.Request.Url.Path);
                                sblog.AppendFormat(",Method:{0}",method);
                                sblog.AppendFormat(",Data:",data);
                                sblog.AppendFormat(",Tick:{0}",tick);
                                sblog.AppendFormat(",AppKey:{0}",appkey);
                                sblog.AppendFormat(",SessionID:{0}", sessionid);
                                sblog.AppendFormat(",Version:{0}", version);
                                sblog.AppendFormat(",AppSecrect:{0}", AppServerDataInitializer.AppClients[appkey].AppSecrect);
                                sblog.AppendFormat(",Token:{0}", token1);
                                sblog.AppendFormat(",TokenFromClient:{0}", token);

                                LogUtil.WriteLog(sblog.ToString());

                                if (token == token1)
                                {
                                    si.Version = version;
                                    si.AppKey = appkey;
                                    MarsUserIdentity identity = new MarsUserIdentity();
                                    identity.SessionID = si.SessionID;
                                    ctx.CurrentUser = identity;
                                    valid = true;
                                }
                                else
                                {
                                    msg = "请求密钥错误！";
                                }
                            }
                            else
                            {
                                msg = "非法会话ID,请退出系统重新登录";
                            }
                        }
                        else
                        {
                            msg = "缺少会话密钥";
                        }
                    }
                    else
                    {
                        msg = "非法AppKey";
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogUtil.WriteLog(ex);

                }
                if (!valid)
                {
                    var res = new Response();
                    res.ContentType = "application/json; charset=utf-8";
                    res.Contents = s =>
                    {
                        byte[] bs = Encoding.UTF8.GetBytes(JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = msg }));
                        s.Write(bs, 0, bs.Length);
                    };
                    return res;
                }
                return null;
            };

            base.ApplicationStartup(container, pipelines);
        }

        static string fun_MD5(string str)
        {
            return MD5.Fun_MD5(str);
        }

        static string GetSign(string data, string tick, string appkey, string sesstionid, string version, string AppScrect)
        {
            try
            {
                string text1 = fun_MD5(string.IsNullOrEmpty(data) ? "" : System.Web.HttpUtility.UrlDecode(data, Encoding.UTF8));
                string text2 = fun_MD5(string.Concat(new object[] {
                AppScrect,
                "&",text1,"&",tick,"&",version,"&",sesstionid,
            }));
                return text2;
            }
            catch
            {

                return string.Empty;
            }

        }
    }

    public class MarsUserIdentity : IUserIdentity
    {

        IEnumerable<string> _claims = new List<string>();
        public IEnumerable<string> Claims
        {
            get { return _claims; }
        }
        private string _name = "MarsUser";
        public string UserName
        {
            get { return _name; }
        }

        public string SessionID { get; set; }
    }
}
