using Mars.Server.BLL;
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
    public class CommentController:BaseController
    {
        [AjaxHandlerAction]
        public string ChangeViewState(HttpContext context)
        {
            int id = 0;
            int vs = 0;
            if (int.TryParse(context.Request.Form["_vs"], out vs) && int.TryParse(context.Request.Form["_id"], out id))
            {
                BCtrl_CommentSearch bll = new BCtrl_CommentSearch();
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

        /// <summary>
        /// 评论用户点赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ILikeThis(HttpContext context)
        {
            int id = 0;
            int uid = 0;
            if (int.TryParse(context.Request.Form["_id"], out id) && int.TryParse(context.Request.Form["_uid"], out uid))
            {
                BCtrl_Comment bll = new BCtrl_Comment();
                int result = bll.ILikeThis(id, uid);
                if (result > 0)
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, "评论点赞成功");
                }
                else if (result == -2) {
                    return string.Format(Entity.G.JSON_OTHER_STATE_STRING,"-2", "已经点赞");
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "评论点赞失败");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteComment(HttpContext context)
        {
            int id = 0;
            int uid = 0;
            if (int.TryParse(context.Request.Form["_id"], out id) && int.TryParse(context.Request.Form["_uid"], out uid))
            {
                BCtrl_Comment bll = new BCtrl_Comment();
                BCtrl_MobileMessage.Instance.Relate_Delete(id); // 删除  与我相关 的数据
                if (bll.DeleteComment(id,uid))
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, "删除评论成功");
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "删除评论失败");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }
    }
}
