using Mars.Server.BLL.Systems;
using Mars.Server.Entity;
using Mars.Server.Entity.Systems;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class StartPicController : BaseController
    {
        [AjaxHandlerAction]
        public string ChangeDefault(HttpContext context)
        {
            int id = 0;
            int isdefault = 0;
            if (int.TryParse(context.Request.Form["_isdefault"], out isdefault) && int.TryParse(context.Request.Form["_id"], out id))
            {
                BCtrl_StartPicSearch bll = new BCtrl_StartPicSearch();
                if (bll.ChangeDefault(id, isdefault))
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
        /// 删除开屏图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteStartPic(HttpContext context)
        {
            int id = 0;
            if (int.TryParse(context.Request.Form["_id"], out id))
            {
                BCtrl_StartPicSearch bll = new BCtrl_StartPicSearch();
                if (bll.DeleteStartPic(id))
                {
                    return string.Format(Entity.G.JSON_OK_STATE_STRING, "删除开屏图片成功");
                }
                else
                {
                    return string.Format(Entity.G.JSON_ERROR_STATE_STRING, "删除开屏图片失败");
                }
            }
            else
            {
                return string.Format(Entity.G.JSON_PMSERROR_STATE_STRING, "获取参数失败");
            }
        }

        [AjaxHandlerAction]
        public string StartPicEdit(HttpContext context)
        {
            string pid = "";
            DateTime stime;
            DateTime etime;
            bool defalut = false;
            string url = "";

            if (!string.IsNullOrEmpty(context.Request.Form["pid"]))
            {
                pid = De(context.Request.Form["pid"]);
                url = string.IsNullOrEmpty(context.Request.Form["url"]) ? "" : context.Request.Form["url"];
                bool.TryParse(context.Request.Form["defalut"], out defalut);
                
                BCtrl_StartPicSearch bll = new BCtrl_StartPicSearch();

                StartPicEntity item = new StartPicEntity()
                {
                    PicURL = pid,
                    IsDefault=defalut,
                    IsOutdate=false,
                    URL=url
                };
                if (!string.IsNullOrEmpty(context.Request.Form["stime"]))
                {
                    DateTime.TryParse(context.Request.Form["stime"], out stime);
                    item.StartTime = stime;
                }
                if (!string.IsNullOrEmpty(context.Request.Form["etime"]))
                {
                    DateTime.TryParse(context.Request.Form["etime"], out etime);
                    item.EndTime = etime;
                }

                if (bll.Add(item))
                {
                    return string.Format(G.JSON_OK_STATE_STRING, "添加成功");
                }
                else
                {
                    return string.Format(G.JSON_ERROR_STATE_STRING, "添加失败");
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "参数获取失败");
            }
        }
    }
}
