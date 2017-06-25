using Mars.Server.BLL.Exhibition;
using Mars.Server.Entity;
using Mars.Server.Entity.Exhibition;
using Mars.Server.Sevice.BaseHandler;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class ExhibitionController : BaseController
    {
        [AjaxHandlerAction]
        public string GetExhibitorList(HttpContext context)
        {
            string key = HttpUtility.UrlDecode(context.Request.QueryString["key"]);
            string callback = HttpUtility.UrlDecode(context.Request.QueryString["callback"]);
            int top = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["top"]), out top);
            int exhibitionid = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["exhibitionid"]), out exhibitionid);

            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
            List<ExhibitorToCustomerEntity> list = bll.GetExhibitorEntityListUseMyCache(exhibitionid).FindAll(e => e.ExhibitorName.Contains(key.Trim()) || e.ExhibitorPinYin.ToLower().Contains(key.Trim()));

            int limit = list.Count > top ? top : list.Count;

            sb.Append(callback + "({\"exhibitors\":[");
            for (int i = 0; i < limit; i++)
            {
                sb.Append("{\"name\":\"" + list[i].ExhibitorName + "\"}");
                if (i < limit - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]})");

            return (sb.ToString());
        }
        [AjaxHandlerAction]
        public string ExhibitorList(HttpContext context)
        {
            int exhibitionid = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["eid"]), out exhibitionid);

            BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
            List<ExhibitorToCustomerEntity> list = bll.GetExhibitorEntityList(exhibitionid);

            var data = new
            {
                state = "1",
                list = list
            };
            return Utils.StringUti.ToUnicode(JsonObj<object>.ToJsonString(data));
        }

        [AjaxHandlerAction]
        public string GetActivityList(HttpContext context)
        {
            string key = HttpUtility.UrlDecode(context.Request.QueryString["key"]);
            string callback = HttpUtility.UrlDecode(context.Request.QueryString["callback"]);
            int top = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["top"]), out top);
            int exhibitionid = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["exhibitionid"]), out exhibitionid);

            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            BCtrl_Activity bll = new BCtrl_Activity();
            List<ActivityToCustomerEntity> list = bll.GetActivityEntityListUseMyCache(exhibitionid)
                .FindAll(e => e.ExhibitorName.Contains(key.Trim())
                || e.ExhibitorPinYin.ToLower().Contains(key.Trim())
                || e.ActivityTitle.Contains(key.Trim())
                || e.ActivityHostUnit.Contains(key.Trim())
                );

            int limit = list.Count > top ? top : list.Count;

            sb.Append(callback + "({\"activitylist\":[");
            for (int i = 0; i < limit; i++)
            {
                sb.Append("{\"name\":\"" + list[i].ActivityHostUnit + ":" + list[i].ActivityTitle + "\",\"atitle\":\"" + list[i].ActivityTitle + "\"}");
                if (i < limit - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]})");

            return (sb.ToString());
        }

        [AjaxHandlerAction]
        public string GetSearchKeyWordList(HttpContext context)
        {
            string key = HttpUtility.UrlDecode(context.Request.QueryString["key"]);
            string callback = HttpUtility.UrlDecode(context.Request.QueryString["callback"]);
            int top = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["top"]), out top);
            int exhibitionid = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["exhibitionid"]), out exhibitionid);
            int searchtype = 0;
            int.TryParse(HttpUtility.UrlDecode(context.Request.QueryString["searchtype"]), out searchtype);

            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            BCtrl_Search bll = new BCtrl_Search();
            List<SearchKeyWordEntity> list = bll.GetSearchKeyWordEntityListUseMyCache(exhibitionid)
                .FindAll(e => (e.SearchName.Contains(key.Trim())
                || e.SearchPinYin.ToLower().Contains(key.Trim()))
                && ((searchtype > 0 && e.SearchType == searchtype) || searchtype == 0)
                );

            int limit = list.Count > top ? top : list.Count;

            string strKeyType = "";
            sb.Append(callback + "({\"keywordlist\":[");
            for (int i = 0; i < limit; i++)
            {
                strKeyType = searchtype == 0 ? (list[i].SearchType == 1) ? "[展商]" : "[活动]" : "";
                sb.Append("{\"name\":\"" + strKeyType + " " + list[i].SearchName + "\",\"searchkey\":\"" + list[i].SearchName + "\"}");
                if (i < limit - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]})");

            return (sb.ToString());
        }

        [AjaxHandlerAction]
        public string DeleteExhibition(HttpContext context)
        {
            int exhibitionID = 0;
            if (int.TryParse(context.Request.Form["exhibitionid"], out exhibitionID))
            {
                BCtrl_Exhibition bll = new BCtrl_Exhibition();
                if (bll.DeleteExhibition(exhibitionID))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DeleteExhibitor(HttpContext context)
        {
            int exhibitorID = 0;

            if (int.TryParse(context.Request.Form["_id"], out exhibitorID))
            {
                BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
                if (bll.DeleteExhibitor(exhibitorID))
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
        public string DeleteActivity(HttpContext context)
        {
            int activityID = 0;

            if (int.TryParse(context.Request.Form["_id"], out activityID))
            {
                BCtrl_Console_Activity bll = new BCtrl_Console_Activity();
                if (bll.DeleteActivity(activityID))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ChangeExhibitorIsHadBookList(HttpContext context)
        {
            int exhibitorID = 0;
            int isHadBookList = 0;
            if (int.TryParse(context.Request.Form["_id"], out exhibitorID) && int.TryParse(context.Request.Form["_ishadbooklist"], out isHadBookList))
            {
                BCtrl_Exhibitor bll = new BCtrl_Exhibitor();
                if (bll.ChangeExhibitorIsHadBookList(exhibitorID, isHadBookList))
                {
                    return string.Format(G.JSON_OK_STATE_STRING, "修改成功");
                }
                else
                {
                    return string.Format(G.JSON_ERROR_STATE_STRING, "修改失败");
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "获取参数失败");
            }
        }

        [AjaxHandlerAction]
        public string ExhibitorEdit(HttpContext context)
        {
            int id = 0;
            int eid = 0;
            string ename = "";
            string eloc = "";
            bool ebook = false;
            bool estate = false;

            //De(context.Request.Form["rdate"])
            if (int.TryParse(context.Request.Form["eid"], out eid) 
                && !string.IsNullOrEmpty(context.Request.Form["ename"]))
            {
                int.TryParse(context.Request.Form["id"], out id);
                bool.TryParse(context.Request.Form["ebook"], out ebook);
                bool.TryParse(context.Request.Form["estate"], out estate);
                ename = De(context.Request.Form["ename"]);
                eloc = De(context.Request.Form["eloc"]);

                BCtrl_Exhibitor bll = new BCtrl_Exhibitor();

                ExhibitorEntity item = new ExhibitorEntity()
                {
                    ExhibitionID = eid,
                    ExhibitorName = ename,
                    ExhibitorPinYin = Utils.Hz2Py.GetWholePinyin(ename),
                    IsHadBookList = ebook,
                    OBCustomerID = 0,
                    OBCustomerTypeID = 0,
                    rowId = 0,
                    StateTypeID = estate ? 1 : 0,
                    CreateTime = DateTime.Now,
                    CreateUserID = this.CurrentAdmin.Sys_UserID,
                    ExhibitorID = id,
                    ExhibitorLocationList = new List<ExhibitorLocationEntity>()
                };

                string[] elocs = eloc.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (elocs != null)
                {
                    foreach (string loc in elocs)
                    {
                        ExhibitorLocationEntity locEntity= new ExhibitorLocationEntity()
                        {
                            ExhibitorID = id,
                            CreateTime = DateTime.Now,
                            CreateUserID= this.CurrentAdmin.Sys_UserID,
                            ExhibitiorLocationOrder = 1000,
                            ExhibitorLocation = loc,
                            ExhibitorLocationID = 0,
                            StateTypeID = estate ? 1 : 0
                        };
                        item.ExhibitorLocationList.Add(locEntity);
                    }
                }

                if (id > 0)
                {
                    if (bll.Update_Exhibitor(item))
                    {
                        return string.Format(G.JSON_OK_STATE_STRING, "修改成功");
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "修改失败");
                    }
                }
                else
                {
                    if (bll.Add_Exhibitor(item) > 0)
                    {
                        return string.Format(G.JSON_OK_STATE_STRING, "添加成功");
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "添加失败");
                    }
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "参数获取失败");
            }
        }

        //ActivityEdit
        [AjaxHandlerAction]
        public string ActivityEdit(HttpContext context)
        {
            int id = 0;
            int emid = 0;
            int eid = 0;
            int pid = 0;
            string aname = "";
            DateTime astime;
            DateTime aetime;
            string aloc = "";
            string ahost = "";
            string aabstract="";
            string aguest = "";
            int aorder = 1000;
            bool estate = false;

            int.TryParse(context.Request.Form["pid"], out pid);

            if ((pid ==0 && int.TryParse(context.Request.Form["emid"], out emid)
                && !string.IsNullOrEmpty(context.Request.Form["aname"])
                && DateTime.TryParse(context.Request.Form["astime"],out astime)
                && DateTime.TryParse(context.Request.Form["aetime"],out aetime)) 
                ||
                (pid > 0 
                && !string.IsNullOrEmpty(context.Request.Form["aname"])
                && DateTime.TryParse(context.Request.Form["astime"], out astime)
                && DateTime.TryParse(context.Request.Form["aetime"], out aetime))
                )
            {
                int.TryParse(context.Request.Form["eid"], out eid);
                int.TryParse(context.Request.Form["id"], out id);
                bool.TryParse(context.Request.Form["estate"], out estate);
                aname = De(context.Request.Form["aname"]);
                aloc = De(context.Request.Form["aloc"]);
                ahost = De(context.Request.Form["ahost"]);
                aabstract = De(context.Request.Form["aabstract"]);
                aguest = De(context.Request.Form["aguest"]);
                int.TryParse(context.Request.Form["aorder"], out aorder);

                BCtrl_Console_Activity bll = new BCtrl_Console_Activity();

                ActivityEntity item = new ActivityEntity()
                {
                    ExhibitorID = eid,
                    ExhibitionID=emid,
                    ActivityID = id,
                    ActivityTitle = aname,
                    ActivityStartTime=astime,
                    ActivityEndTime=aetime,
                    ParentID= pid,
                    ActivityAbstract=aabstract,
                    ActivityGuest=aguest,
                    ActivityHostUnit=ahost,
                    ActivityLocation=aloc,
                    ActivityOrder=aorder,
                    ActivityTypeID=0,
                    StateTypeID = estate ? 1 : 0,
                    CreateTime = DateTime.Now,
                    CreateUserID = this.CurrentAdmin.Sys_UserID
                };

                if (id > 0)
                {
                    if (bll.Update_Activity(item))
                    {
                        return string.Format(G.JSON_OK_STATE_STRING, "修改成功");
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "修改失败");
                    }
                }
                else
                {
                    if (bll.Add_Activity(item) > 0)
                    {
                        return string.Format(G.JSON_OK_STATE_STRING, "添加成功");
                    }
                    else
                    {
                        return string.Format(G.JSON_ERROR_STATE_STRING, "添加失败");
                    }
                }
            }
            else
            {
                return string.Format(G.JSON_ERROR_STATE_STRING, "参数获取失败");
            }
        }
    }
}
