using Mars.Server.BLL.NewYear;
using Mars.Server.Entity.NewYear;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class NewYearController : BaseController
    {
        [AjaxHandlerAction]
        public string SubmitCouplet(HttpContext context)
        {
            string up = "";
            string down = "";
            string horizontal = "";

            if (!string.IsNullOrEmpty(context.Request.Form["_u"])
                && !string.IsNullOrEmpty(context.Request.Form["_d"])
                && !string.IsNullOrEmpty(context.Request.Form["_h"]))
            {
                up = De(context.Request.Form["_u"]);
                down = De(context.Request.Form["_d"]);
                horizontal = De(context.Request.Form["_h"]);

                BCtrl_Couplet bll = new BCtrl_Couplet();
                int coupletid = bll.Add(up, down, horizontal);
                if (coupletid > 0)
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, coupletid.ToString());
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "服务器繁忙，请您稍后再试");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }

        [AjaxHandlerAction]
        public string ShareCount(HttpContext context)
        {
            string share = string.IsNullOrEmpty(context.Request.QueryString["s"]) ? "0" : De(context.Request.QueryString["s"]);
            string machinecode = string.IsNullOrEmpty(context.Request.QueryString["m"]) ? "" : De(context.Request.QueryString["m"]);
            string sharetype = string.IsNullOrEmpty(context.Request.QueryString["st"]) ? "0" : De(context.Request.QueryString["st"]);
            string verson = string.IsNullOrEmpty(context.Request.QueryString["v"]) ? "" : De(context.Request.QueryString["v"]);
            string systemname = verson.IndexOf('_') > 0 ? verson.Substring(0, verson.IndexOf('_')) : "";
            string ip = context.Request.UserHostAddress;
            string agent = context.Request.UserAgent;

            string coupletid = string.IsNullOrEmpty(context.Request.QueryString["c"]) ? "28" : De(context.Request.QueryString["c"]);
            string imageid = string.IsNullOrEmpty(context.Request.QueryString["i"]) ? "1" : De(context.Request.QueryString["i"]);
            string isview = string.IsNullOrEmpty(context.Request.QueryString["iv"]) ? "1" : De(context.Request.QueryString["iv"]);
            BCtrl_Couplet bll = new BCtrl_Couplet();
            ShareLogEntity item = new ShareLogEntity()
            {
                CreateTime = DateTime.Now,
                ExInfo = agent,
                IPAddress = ip,
                MachineCode = machinecode,
                ShareTypeID = int.Parse(sharetype),
                ShareUserID = int.Parse(share),
                SystemName = systemname,
                Verson = verson,
                CoupletID = int.Parse(coupletid),
                ImageID = int.Parse(imageid),
            };
            bll.AddShareLog(item);

            return string.Empty;
        }
    }
}
