using Mars.Server.BLL.Users;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class UserController : BaseController
    {
        [AjaxHandlerAction]
        public string DeleteFeedback(HttpContext context)
        {
            int feedbackID = 0;

            if (int.TryParse(context.Request.Form["_id"], out feedbackID))
            {
                BCtrl_FeedbackSearch bll = new BCtrl_FeedbackSearch();
                if (bll.Delete(feedbackID))
                {
                    return string.Format(G.JSON_OK_STATE_STRING, "删除成功");
                }
                else
                {
                    return string.Format(G.JSON_ERROR_STATE_STRING, "删除失败");
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "获取参数失败");
            }
        }

        [AjaxHandlerAction]
        public string WriteNumber(HttpContext context)
        {
            int r = 0;
            string m ="";
            if (int.TryParse(context.Request.Form["_r"], out r) && !string.IsNullOrEmpty(context.Request.Form["_m"]))
            {
                BCtrl_Recommend bll = new BCtrl_Recommend();
                m = context.Request.Form["_m"];
                if (bll.WriteNumber(r,m)>0)
                {
                    return string.Format(G.JSON_OK_STATE_STRING, "登记成功");
                }
                else
                {
                    return string.Format(G.JSON_ERROR_STATE_STRING, "登记失败");
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "获取参数失败");
            }
        }


        [AjaxHandlerAction]
        public string DeleteReport(HttpContext context)
        {
            int reportID = 0;

            if (int.TryParse(context.Request.Form["_id"], out reportID))
            {
                BCtrl_ReportSearch bll = new BCtrl_ReportSearch();
                if (bll.Delete(reportID))
                {
                    return string.Format(G.JSON_OK_STATE_STRING, "删除成功");
                }
                else
                {
                    return string.Format(G.JSON_ERROR_STATE_STRING, "删除失败");
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "获取参数失败");
            }
        }
    }
}
