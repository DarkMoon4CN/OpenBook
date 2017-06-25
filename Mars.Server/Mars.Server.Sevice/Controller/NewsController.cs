using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using System.Web;
using Mars.Server.Utils;

namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// 手机端Ajax控制类
    /// </summary>
    [AjaxController]
    public class NewsController
    {
        /// <summary>
        /// 文章浏览计数器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string NewsBrowserCounter(HttpContext context)
        {
            string status = "{\"status\":-1}";
            Guid eventItemGuid;

            if (Guid.TryParse(context.Request.Form["pid"], out eventItemGuid))
            {
                BCtrl_EventItemBrowseCnt bll = new BCtrl_EventItemBrowseCnt();
                bool isSuccess = bll.UpdateBrowserCnt(eventItemGuid);
                if (isSuccess)
                {
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}";
                }
            }

            return status;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetEventItemContent(HttpContext context)
        {
            dynamic result = new { status = "-1" };
            Guid eventItemGuid;

            if (Guid.TryParse(context.Request.Form["pid"], out eventItemGuid))
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                EventItemViewEntity entity = bll.QueryViewEntity(eventItemGuid);
                entity.Html = bll.DealHtmlImg(entity.Html);

                if (!string.IsNullOrEmpty(entity.ThemePicturePath))
                {
                    if(entity.StartTime.Year == 4000 && entity.EndTime.Year == 4000)
                    {
                        entity.ActiveTimeDesc="";
                    }
                    else if (entity.StartTime.ToString("HH:mm:ss") == "00:00:00" && entity.EndTime.ToString("HH:mm:ss") == "00:00:00")
                    {
                        entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd") + "至" + entity.EndTime.ToString("yyyy-MM-dd");
                    }
                    else if (entity.StartTime.ToShortDateString() == entity.EndTime.ToShortDateString())
                    {

                        if (entity.StartTime.Hour != 9 || entity.EndTime.Hour != 17)
                        {
                            entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd HH:mm") + "至" + entity.EndTime.ToString("yyyy-MM-dd HH:mm");
                        }
                        else
                        {
                            entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    else 
                    {
                        entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd HH:mm") + "至" + entity.EndTime.ToString("yyyy-MM-dd HH:mm");
                    }
                    //if (entity.StartTime.Year != 4000 && entity.EndTime.Year != 4000)
                    //{
                    //    if (entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss") == entity.StartTime.ToString("yyyy-MM-dd") + " 00:00:00"
                    //        && entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss") == entity.EndTime.ToString("yyyy-MM-dd") + " 23:59:59")
                    //    {
                    //        entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd") + "全天";
                    //    }
                    //    else if (entity.StartTime.ToString("HH:mm:ss") == "00:00:00" && entity.EndTime.ToString("HH:mm:ss") == "00:00:00")
                    //    {
                    //        entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd") + "至" + entity.EndTime.ToString("yyyy-MM-dd");
                    //    }
                    //    else
                    //    {
                    //        entity.ActiveTimeDesc = entity.StartTime.ToString("yyyy-MM-dd HH:mm") + "至" + entity.EndTime.ToString("yyyy-MM-dd HH:mm");
                    //    }
                    //}
                }

                if (entity != null)
                {
                    result = new { status = "1", item = entity };
                }
                else
                {
                    result = new { status = "0" };
                }
            }
            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(result));
        }
    }
}
