using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class LogController : BaseController
    {
        [AjaxHandlerAction]
        public string CountDownloadPageView(HttpContext context)
        {
            string chn = string.IsNullOrEmpty(context.Request.QueryString["chn"]) ? "" : De(context.Request.QueryString["chn"]);
            string share = string.IsNullOrEmpty(context.Request.QueryString["share"]) ? "" : De(context.Request.QueryString["share"]);
            string token = string.IsNullOrEmpty(context.Request.QueryString["token"]) ? "" : De(context.Request.QueryString["token"]);

            Utils.LogEntity log = new Utils.LogEntity()
            {
                LogContent = "下载页面浏览，下载浏览路径"+context.Request.RawUrl,
                LogTime = DateTime.Now,
                UnqiueID = Guid.NewGuid(),
                ExInfo = "ip:"+context.Request.UserHostAddress + ",agent:"+ context.Request.UserAgent,
                LogMeta = token
            };
            if (!string.IsNullOrEmpty(chn))
            {
                log.LogTypeID = 9;
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,chn:" + chn;
            }
            else if (!string.IsNullOrEmpty(share))
            {
                log.LogTypeID = 10;
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,share:" + share;
            }
            else
            {
                log.LogTypeID = 2;
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,customer";
            }

            Utils.LogUtil.WriteLog(log);

            return string.Empty;
        }

        [AjaxHandlerAction]
        public string CountDownloadClick(HttpContext context)
        {
            string chn = string.IsNullOrEmpty(context.Request.QueryString["chn"]) ? "" : De(context.Request.QueryString["chn"]);
            string share = string.IsNullOrEmpty(context.Request.QueryString["share"]) ? "" : De(context.Request.QueryString["share"]);
            string token = string.IsNullOrEmpty(context.Request.QueryString["token"]) ? "" : De(context.Request.QueryString["token"]);
            string systype = string.IsNullOrEmpty(context.Request.QueryString["systype"]) ? "" : De(context.Request.QueryString["systype"]);

            Utils.LogEntity log = new Utils.LogEntity()
            {
                LogContent = "下载页面浏览，下载按钮点击["+ systype + "],下载浏览路径" + context.Request.RawUrl,
                LogTypeID=11,
                LogTime = DateTime.Now,
                UnqiueID = Guid.NewGuid(),
                ExInfo = "ip:" + context.Request.UserHostAddress + ",agent:" + context.Request.UserAgent,
                LogMeta = token
            };
            if (!string.IsNullOrEmpty(chn))
            {
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,chn:" + chn;
            }
            else if (!string.IsNullOrEmpty(share))
            {
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,share:" + share;
            }
            else
            {
                log.UserID = 0;
                log.LogTitle = "下载页面浏览,customer";
            }

            Utils.LogUtil.WriteLog(log);

            return string.Empty;
        }

        [AjaxHandlerAction]
        public string CountNewsPageView(HttpContext context)
        {
            string chn = string.IsNullOrEmpty(context.Request.QueryString["chn"]) ? "" : De(context.Request.QueryString["chn"]);
            string share = string.IsNullOrEmpty(context.Request.QueryString["share"]) ? "" : De(context.Request.QueryString["share"]);
            string browse = string.IsNullOrEmpty(context.Request.QueryString["browse"]) ? "" : De(context.Request.QueryString["browse"]);
            string token = string.IsNullOrEmpty(context.Request.QueryString["token"]) ? "" : De(context.Request.QueryString["token"]);
            string sharetype = string.IsNullOrEmpty(context.Request.QueryString["sharetype"]) ? "" : De(context.Request.QueryString["sharetype"]);
            string e=string.IsNullOrEmpty(context.Request.QueryString["e"]) ? "" : De(context.Request.QueryString["e"]);
            string w = string.IsNullOrEmpty(context.Request.QueryString["w"]) ? "" : De(context.Request.QueryString["w"]);
            Utils.LogEntity log = new Utils.LogEntity()
            {
                LogContent = "发现频道页面浏览，浏览人["+ browse + "],分享人["+ share + "],token["+ token + "],分享方式[" + sharetype + "],浏览路径[" + context.Request.RawUrl+"],进入途径ID["+w+"]",
                LogTypeID = 12,
                LogTime = DateTime.Now,
                UnqiueID = Guid.NewGuid(),
                ExInfo = "ip:" + context.Request.UserHostAddress + ",agent:" + context.Request.UserAgent,
                LogMeta = token
            };
            int tmpUserid = 0;
            if (!string.IsNullOrEmpty(chn))
            {
                int.TryParse(chn, out tmpUserid);
                log.UserID = tmpUserid;
                log.LogTitle = "发现频道页面浏览["+e+"],chn:" + chn;
            }
            else if (!string.IsNullOrEmpty(share))
            {
                int.TryParse(share, out tmpUserid);
                log.UserID = tmpUserid;
                log.LogTitle = "发现频道页面浏览[" + e + "],share:" + share;
            }
            else if (!string.IsNullOrEmpty(browse))
            {
                int.TryParse(browse, out tmpUserid);
                log.UserID = tmpUserid;
                log.LogTitle = "发现频道页面浏览[" + e + "],browse:" + browse;
            }
            else
            {
                log.UserID = 0;
                log.LogTitle = "下载页面浏览[" + e + "],customer";
            }
            log.LogTitle += "[进入途径ID：" + w + "]";
            Utils.LogUtil.WriteLog(log);

            return string.Empty;
        }

        [AjaxHandlerAction]
        public string LogTroublePageInfo(HttpContext context)
        {
            Utils.LogEntity log = new Utils.LogEntity()
            {
                LogContent = "异常页面,请求路径" + context.Request.RawUrl+"["+context.Request.QueryString["path"] +"]",
                LogTypeID = 0,
                LogTime = DateTime.Now,
                UnqiueID = Guid.NewGuid(),
                ExInfo = "ip:" + context.Request.UserHostAddress + ",agent:" + context.Request.UserAgent,
                LogMeta = "",
                LogTitle= "异常页面",
                UserID=0
            };

            Utils.LogUtil.WriteLog(log);

            return string.Empty;
        }
    }
}
