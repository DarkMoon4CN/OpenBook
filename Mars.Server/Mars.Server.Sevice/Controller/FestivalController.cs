using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class FestivalController : BaseController
    {
        /// <summary>
        /// 插入日期
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string InsertFestival(HttpContext context)
        { 
            string str="{\"status\":0}";
            FestivalEntity entity = new FestivalEntity();
            entity.FestivalName = context.Request.Form["name"];
            entity.FestivalShortName = context.Request.Form["shortname"]; 
            int type=0;int Weight=0;
            int.TryParse(context.Request.Form["type"],out type);
            int.TryParse(context.Request.Form["Weight"],out Weight);
            entity.FestivalType = type;
            entity.FestivalWeight = Weight;
            if (!string.IsNullOrEmpty(context.Request.Form["txtStartTime"]))
            {
                entity.StartTime = DateTime.Parse(context.Request.Form["txtStartTime"]).Date;
            }
            if (!string.IsNullOrEmpty(context.Request.Form["txtEndTime"]))
            {
                entity.EndTime = DateTime.Parse(context.Request.Form["txtEndTime"]).AddDays(1).Date.AddSeconds(-1);
            }
            BCtrl_Festival bll = new BCtrl_Festival();
            string flg=bll.InsertFestival(entity);
            if (flg == "0")
            {
                str = "{\"status\":0}";
            }
            else if (flg == "1")
            {
                str = "{\"status\":1}";
            }
            else if (flg == "2")
            {
                str = "{\"status\":2}";
            }
            return str;
        } 
        /// <summary>
        /// 删除节日
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string delFestival(HttpContext context)
        {
            string stat="{\"status\":0}";
            string id = context.Request.Form["Festivalid"]; 
            FestivalEntity entity=new FestivalEntity();
            entity.FestivalID=new Guid(id);
            BCtrl_Festival bll = new BCtrl_Festival(); 
            string flg=bll.deleteFestival(entity);
            if(flg=="1")
            {
                stat="{\"status\":1}";
            }
            else
            {
                stat="{\"status\":0}";
            } 
                return stat;
        }
        [AjaxHandlerAction]
        public string UpdateFestival(HttpContext context)
        {
            string str="{\"status\":0}";
            FestivalEntity entity = new FestivalEntity();
            if (!string.IsNullOrEmpty(context.Request.Form["id"]))
            {
                entity.FestivalID = new Guid(context.Request.Form["id"]);
                entity.FestivalName = context.Request.Form["name"];
                entity.FestivalShortName = context.Request.Form["shortname"]; 
                int type = 0; int Weight = 0;
                int.TryParse(context.Request.Form["type"], out type);
                int.TryParse(context.Request.Form["Weight"], out Weight);
                entity.FestivalType = type;
                entity.FestivalWeight = Weight; 
                 
                if (!string.IsNullOrEmpty(context.Request.Form["txtStartTime"]))
                {
                    entity.StartTime = DateTime.Parse(context.Request.Form["txtStartTime"]).Date;
                }
                if (!string.IsNullOrEmpty(context.Request.Form["txtEndTime"]))
                {
                    entity.EndTime = DateTime.Parse(context.Request.Form["txtEndTime"]).Date.AddDays(1).AddSeconds(-1);
                }
                BCtrl_Festival bll = new BCtrl_Festival();
                string flg = bll.UpdateFestival(entity);
                if (flg == "0")
                {
                    str = "{\"status\":0}";
                }
                else if (flg == "1")
                {
                    str = "{\"status\":1}";
                }
                else if (flg == "2")
                {
                    str = "{\"status\":2}";
                }
            }
            return str;
        } 
    }
}
