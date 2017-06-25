using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Sevice.BaseHandler;
using System.Web;
using Mars.Server.Entity;
using Mars.Server.BLL;
using Mars.Server.Utils;

namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// 公共字典控制器
    /// 各种下拉列表，基础字典数据库查询，返回JSON
    /// </summary>
    [AjaxController]
    public class CommonDictController
    {
        /// <summary>
        /// 查询省
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetProvince(HttpContext context)
        {
            BCtrl_Zone bll = new BCtrl_Zone();

            List<ZoneEntity> list = bll.QueryZoneProvince();

            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
        }

        /// <summary>
        /// 根据区域分组查询各个省份
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetProvinceToGroup(HttpContext context)
        {
            BCtrl_Zone bll = new BCtrl_Zone();
            List<ZoneEntity> list = bll.QueryZoneToGroup();
            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
        }

        /// <summary>
        /// 查询文章发布一级分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetFirstCalendarType(HttpContext context)
        {
            BCtrl_CalendarType bll = new BCtrl_CalendarType();
            List<CalendarTypeEntity> list =  bll.QueryFirstCalendarType();
            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
        }

        /// <summary>
        /// 根据一级分类，查询二级分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetSecondCalendarType(HttpContext context)
        {
            int parentCalendarTypeID = 0;
            if (int.TryParse(context.Request.Form["pid"], out parentCalendarTypeID) && parentCalendarTypeID > 0)
            {
                BCtrl_CalendarType bll = new BCtrl_CalendarType();
                List<CalendarTypeEntity> list = bll.QuerySecondCalendarType(parentCalendarTypeID);
                return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询文章专题分组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetEventItemGroup(HttpContext context)
        {
            string groupName = context.Request.Form["groupname"];

            BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();
            List<EventItemGroupEntity> list = bll.QueryTop20GroupList(groupName);
            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
        }

        /// <summary>
        /// 系统升级修改功能
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string UpdateAppVersion(HttpContext context)
        {
            string statu = "{\"status\":0}";
            bool flg = false;
            int type = 0;
            bool force = false;
            int size = 0;
            int appid = 0;
            try
            {
                BCtrl_Common bll = new BCtrl_Common();
                AppUpdateEntity entity = new AppUpdateEntity();
                int.TryParse(context.Request.Form["apptype"].ToString(), out type);
                entity.AppType = type;
                entity.Version = context.Request.Form["version"];
                entity.UpdateProfile = HttpUtility.UrlDecode(context.Request.Form["profile"], Encoding.UTF8);
                entity.DownloadUrl = HttpUtility.UrlDecode(context.Request.Form["downurl"], Encoding.UTF8);
                bool.TryParse(context.Request.Form["forcedupdate"] == "1" ? "true" : "false", out force);
                entity.ForcedUpdate = force;
                int.TryParse(context.Request.Form["size"], out size);
                int.TryParse(context.Request.Form["appid"], out appid);
                entity.AppId = appid;
                entity.AppSize = size;
                flg = bll.UpdateAppVersion(entity);
                if (flg)
                { statu = "{\"status\":1}"; }
                else
                {
                    statu = "{\"status\":2}";  //修改失败
                }
            }
            catch (Exception e)
            {
                LogUtil.WriteLog(e);
                statu = "{\"status\":0}";  //数据传输错误
            }
            return statu;
        }
       /// <summary>
       /// 删除系统升级记录
       /// </summary>
       /// <param name="context"></param>
       /// <returns></returns>
        [AjaxHandlerAction]
        public string delAppUpdate(HttpContext context)
        {
            int appid = 0;
            string stat = "{\"status\":0}";
            int.TryParse(context.Request.Form["appid"], out appid);
            BCtrl_Common bll = new BCtrl_Common();
            string flg = bll.deleteAppUpdate(appid);
            if (flg == "1")
            {
                stat = "{\"status\":1}";
            }
            else
            {
                stat = "{\"status\":0}";
            }
            return stat;
        }

        /// <summary>
        /// 新建文章时，关联行业节日
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetFestivalList(HttpContext context)
        {
            BCtrl_Festival bll = new BCtrl_Festival();
            List<FestivalEntity> list = bll.GetFestivalList("",3);

            if (list == null)
            {
                return string.Empty;
            }

            var groupYear = list.OrderByDescending(o => o.StartTime).GroupBy(o => o.StartTime.ToString("yyyy年"));
            List<FestivalEntity> resultlist = new List<FestivalEntity>();
            foreach (var year  in groupYear)
            {
                FestivalEntity entity = new FestivalEntity { FestivalName = year.Key, FestivalList = year.Select(o => o).ToList() };
                resultlist.Add(entity);                
            }

            string result = StringUti.ToUnicode(JsonObj<object>.ToJsonString(resultlist));
            return result;
        }

        /// <summary>
        /// 关联行业节日 自动填充 按年分组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GetFestivalListByGroup(HttpContext context)
        {
               BCtrl_Festival bll = new BCtrl_Festival();

            string qvalue = context.Request.QueryString["query"];

            List<FestivalEntity> list = bll.GetFestivalList(qvalue,3);

            if (list != null)
            { 
                dynamic result = new { query = "Unit", suggestions = list.Select(o => new { value = o.FestivalName, data = new { category = o.StartTime.ToString("yyyy年"), text = o.FestivalID } }) };

                string temp = StringUti.ToUnicode(JsonObj<object>.ToJsonString(result));
                return temp;
            }

            return string.Empty;
        }

    }
}
