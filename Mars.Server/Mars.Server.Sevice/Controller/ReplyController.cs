using Mars.Server.BLL.Comments;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class ReplyController : BaseController
    {
        [AjaxHandlerAction]
        public string ChangeViewState(HttpContext context)
        {
            int id = 0;
            int vs = 0;
            if (int.TryParse(context.Request.Form["_vs"], out vs) && int.TryParse(context.Request.Form["_id"], out id))
            {
                BCtrl_ReplySearch bll = new BCtrl_ReplySearch();
                if (bll.ChangeViewState(id, vs))
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, "修改状态成功");
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "修改状态失败");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }
    }
}
