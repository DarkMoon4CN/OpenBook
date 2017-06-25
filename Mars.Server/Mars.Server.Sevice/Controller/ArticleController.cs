using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Sevice.BaseHandler;
using System.Web;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System.Data;
using System.IO;


namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// 文章管理控制器
    /// </summary>
    [AjaxController]
    public class ArticleController : Mars.Server.Sevice.BaseHandler.BaseController
    {
        [AjaxHandlerAction]
        public string IsUseableTitle(HttpContext context)
        {
            int eventItemID = 0;
            string title = context.Request.Form["title"];

            if (!string.IsNullOrEmpty(title) && int.TryParse(context.Request.Form["pid"], out eventItemID))
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isUseable = bll.IsUseableTitle(eventItemID, title);

                if (isUseable)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        [AjaxHandlerAction]
        public string DeleteImage(HttpContext context)
        {
            string status = "{\"status\":-1}";

            string imgurl = context.Request.Form["imgurl"];

            if (!string.IsNullOrEmpty(imgurl))
            {
                UploadImageHelper uploadHelper = new UploadImageHelper();

                string imgKey = uploadHelper.ConvertImgkeyByUrl(imgurl, false);
                if (imgKey != "")
                {
                    bool isDelSuccess = uploadHelper.DeleteImage(imgKey);
                    if (isDelSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string Insert(HttpContext context)
        {
            string status = "{\"status\":-1}";
            try
            {
                BCtrl_EventItem bllEventItem = new BCtrl_EventItem();
                BCtrl_PictureServer bllPicServer = new BCtrl_PictureServer();

                //封面图片和发现轮播图共用[Advert]字段  1有封面状态  2有发现轮播图
                EventItemPictureState enumConver = EventItemPictureState.None;
                EventItemPictureState enumDiscover = EventItemPictureState.None;

                //如果是活动链接子页面，则一级 二级分类都为空
                int firsttype = context.Request.Form["firsttype"] != "" ? int.Parse(context.Request.Form["firsttype"]) : 0;
                int secondtype = context.Request.Form["secondtype"] != "" ? int.Parse(context.Request.Form["secondtype"]) : 0;

                if (secondtype == 0 && firsttype > 0)
                {
                    secondtype = firsttype; //如果二级分类为空，则存放一级分类
                }

                int publishzone = 0;
                string[] zones = context.Request.Form["publishzone"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (zones.Count(array => array == "0") == 0)
                {
                    foreach (string zone in zones)
                    {
                        publishzone = publishzone | int.Parse(zone);
                    }
                }

                string activeplace = context.Request.Form["activeplace"];

                string title = HttpUtility.UrlDecode(context.Request.Form["title"], Encoding.UTF8);
                string content = HttpUtility.UrlDecode(context.Request.Form["content"], Encoding.UTF8); ;
                string imgurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgurl"], true);
                string imgthemeurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgthemeurl"], true);
                string html = HttpUtility.UrlDecode(context.Request.Form["html"], Encoding.UTF8);

                DateTime publishtime = DateTime.Parse(context.Request.Form["publishtime"]);
                string publishsource = context.Request.Form["publishsource"];
                int recommend = int.Parse(context.Request.Form["recommend"]);

                int advert = int.Parse(context.Request.Form["advert"]);
                enumConver = (EventItemPictureState)advert;

                DateTime adsendtime = new DateTime(4000, 1, 1);
                if (!string.IsNullOrEmpty(context.Request.Form["adsendtime"])) { 
                DateTime.TryParse(context.Request.Form["adsendtime"], out adsendtime);
                }

                DateTime discoveradsendtime = new DateTime(4000, 1, 1);
                if (!string.IsNullOrEmpty(context.Request.Form["discoveradsendtime"]))
                {
                    DateTime.TryParse(context.Request.Form["discoveradsendtime"], out discoveradsendtime);
                }
                int advertorder = int.Parse(context.Request.Form["advertorder"]);
                DateTime starttime = DateTime.Parse(context.Request.Form["starttime"]);
                DateTime endtime = DateTime.Parse(context.Request.Form["endtime"]);
                int eventItemID = int.Parse(context.Request.Form["pid"]);

                string allday = context.Request.Form["allday"];
                DateTime alldaytime;
                if (DateTime.TryParse(allday, out alldaytime))
                {
                    starttime = alldaytime.Date;
                    endtime = alldaytime.Date.AddDays(1).AddSeconds(-1);
                }

                string booklistpath = HttpUtility.UrlDecode(context.Request.Form["booklistpath"]);

                int eventItemFlag = int.Parse(context.Request.Form["eventItemFlag"]); //是否是节日文章
                string imgcarouselurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgcarouselurl"], true);

                int discoverAdvert = int.Parse(context.Request.Form["discoveradvert"]);
                enumDiscover = (EventItemPictureState)discoverAdvert;
                string title2 = string.Empty;
                DateTime starttime2 = new DateTime(4000, 1, 1);
                DateTime endtime2 = new DateTime(4000, 1, 1);
                Guid festivalid = new Guid();

                if (eventItemFlag != 0)
                {
                    title2 = context.Request.Form["title2"];
                    starttime2 = DateTime.Parse(context.Request.Form["starttime2"]);
                    endtime2 = DateTime.Parse(context.Request.Form["endtime2"]);
                    festivalid = new Guid(context.Request.Form["festivalid"]);
                }

                int singlegroup = int.Parse(context.Request.Form["singlegroup"]);

                EventItemEntity itemEntity = new EventItemEntity();
                itemEntity.EventItemGUID = Guid.NewGuid();
                itemEntity.UserID = -1; //后台发布默认为-1
                itemEntity.Title = title;
                itemEntity.Content = content;
                itemEntity.StartTime = starttime;
                itemEntity.EndTime = endtime;
                itemEntity.CalendarTypeID = secondtype;
                itemEntity.EventItemState = 1;
                itemEntity.Remark = null;
                itemEntity.Url = WebMaster.ArticleDomain + itemEntity.EventItemGUID.ToString() + ".html";
                itemEntity.Recommend = recommend;
                itemEntity.PublishTime = publishtime;
                itemEntity.PublishSource = publishsource;
                itemEntity.PictureID = 0;
                itemEntity.ThemePictureID = 0;
                itemEntity.Advert = (int)enumConver | (int)enumDiscover;
                itemEntity.AdvertOrder = advertorder;
                itemEntity.PublishAreaID = publishzone;
                itemEntity.ActivePlace = activeplace;
                itemEntity.Html = html;
                itemEntity.CreateOpID = int.Parse(base.CurrentAdmin.Sys_UserID);
                itemEntity.CreateTime = DateTime.Now;
                itemEntity.BookListPath = booklistpath;

                itemEntity.Title2 = title2;
                itemEntity.StartTime2 = starttime2;
                itemEntity.EndTime2 = endtime2;
                itemEntity.CarouselPictureID = 0;
                itemEntity.EventItemFlag = eventItemFlag;
                itemEntity.Singlegroup = singlegroup;
                itemEntity.FestivalID = festivalid;

                itemEntity.AdsEndTime = adsendtime;
                itemEntity.DiscoverAdsEndTime = discoveradsendtime;

                PictureEntity pictureEntity = null;
                if (!string.IsNullOrEmpty(imgurl))
                {
                    pictureEntity = new PictureEntity();
                    pictureEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                    pictureEntity.PicturePath = imgurl;
                    pictureEntity.PictureState = 0;
                }

                PictureEntity themePictureEntity = null;
                if (!string.IsNullOrEmpty(imgthemeurl))
                {
                    themePictureEntity = new PictureEntity();
                    themePictureEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                    themePictureEntity.PicturePath = imgthemeurl;
                    themePictureEntity.PictureState = 0;
                }

                PictureEntity carouselPicEntity = null;
                if (!string.IsNullOrEmpty(imgcarouselurl))
                {
                    carouselPicEntity = new PictureEntity();
                    carouselPicEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                    carouselPicEntity.PicturePath = imgcarouselurl;
                    carouselPicEntity.PictureState = 0;
                }

                if (bllEventItem.IsUseableTitle(eventItemID, title))
                {
                    bool isSuccess = bllEventItem.Insert(itemEntity, pictureEntity, themePictureEntity, carouselPicEntity);
                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
                else
                {
                    status = "{\"status\":2}";
                }
            }
            catch
            {
                return status;
            }
            return status;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string Update(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
            {
                try
                {
                    BCtrl_EventItem bllEventItem = new BCtrl_EventItem();
                    BCtrl_PictureServer bllPicServer = new BCtrl_PictureServer();

                    //封面图片和发现轮播图共用[Advert]字段  1有封面状态  2有发现轮播图
                    EventItemPictureState enumConver = EventItemPictureState.None;
                    EventItemPictureState enumDiscover = EventItemPictureState.None;

                    #region 逻辑处理

                    //如果是活动链接子页面，则一级 二级分类都为空
                    int firsttype = context.Request.Form["firsttype"] != "" ? int.Parse(context.Request.Form["firsttype"]) : 0;
                    int secondtype = context.Request.Form["secondtype"] != "" ? int.Parse(context.Request.Form["secondtype"]) : 0;

                    if (secondtype == 0 && firsttype > 0)
                    {
                        secondtype = firsttype; //如果二级分类为空，则存放一级分类
                    }

                    int publishzone = 0;
                    string[] zones = context.Request.Form["publishzone"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zones.Count(array => array == "0") == 0)
                    {
                        foreach (string zone in zones)
                        {
                            publishzone = publishzone | int.Parse(zone);
                        }
                    }


                    string activeplace = context.Request.Form["activeplace"];

                    string title = HttpUtility.UrlDecode(context.Request.Form["title"], Encoding.UTF8);
                    string content = HttpUtility.UrlDecode(context.Request.Form["content"], Encoding.UTF8); ;
                    string imgurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgurl"], true);
                    string imgthemeurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgthemeurl"], true);
                    string html = HttpUtility.UrlDecode(context.Request.Form["html"], Encoding.UTF8);

                    DateTime publishtime = DateTime.Parse(context.Request.Form["publishtime"]);
                    string publishsource = context.Request.Form["publishsource"];
                    int recommend = int.Parse(context.Request.Form["recommend"]);
                    int advert = int.Parse(context.Request.Form["advert"]);
                    enumConver = (EventItemPictureState)advert;

                    DateTime adsendtime = new DateTime(4000, 1, 1);
                    if (!string.IsNullOrEmpty(context.Request.Form["adsendtime"]))
                    {
                        DateTime.TryParse(context.Request.Form["adsendtime"], out adsendtime);
                    }
                    DateTime discoveradsendtime = new DateTime(4000, 1, 1);
                    if (!string.IsNullOrEmpty(context.Request.Form["discoveradsendtime"]))
                    {
                        DateTime.TryParse(context.Request.Form["discoveradsendtime"], out discoveradsendtime);
                    }
                    int advertorder = int.Parse(context.Request.Form["advertorder"]);
                    DateTime starttime = DateTime.Parse(context.Request.Form["starttime"]);
                    DateTime endtime = DateTime.Parse(context.Request.Form["endtime"]);

                    //string allday = context.Request.Form["allday"];
                    //DateTime alldaytime;
                    //if (DateTime.TryParse(allday, out alldaytime))
                    //{
                    //    starttime = alldaytime.Date;
                    //    endtime = alldaytime.Date.AddDays(1).AddSeconds(-1);
                    //}
                    string booklistpath = HttpUtility.UrlDecode(context.Request.Form["booklistpath"]);

                    int eventItemFlag = int.Parse(context.Request.Form["eventItemFlag"]);
                    string imgcarouselurl = new UploadImageHelper().ConvertImgkeyByUrl(context.Request.Form["imgcarouselurl"], true);

                    int discoverAdvert = int.Parse(context.Request.Form["discoveradvert"]);
                    enumDiscover = (EventItemPictureState)discoverAdvert;
                    string title2 = string.Empty;
                    DateTime starttime2 = new DateTime(4000, 1, 1);
                    DateTime endtime2 = new DateTime(4000, 1, 1);
                    Guid festivalid = new Guid();

                    if (eventItemFlag != 0)
                    {
                        title2 = context.Request.Form["title2"];
                        starttime2 = DateTime.Parse(context.Request.Form["starttime2"]);
                        endtime2 = DateTime.Parse(context.Request.Form["endtime2"]);
                        festivalid = new Guid(context.Request.Form["festivalid"]);
                    }

                    int singlegroup = int.Parse(context.Request.Form["singlegroup"]);

                    EventItemEntity itemEntity = bllEventItem.QueryEntity(eventItemID);
                    itemEntity.Title = title;
                    itemEntity.Content = content;
                    itemEntity.StartTime = starttime;
                    itemEntity.EndTime = endtime;
                    itemEntity.CalendarTypeID = secondtype;
                    itemEntity.EventItemState = 1;
                    itemEntity.Recommend = recommend;
                    itemEntity.PublishTime = publishtime;
                    itemEntity.PublishSource = publishsource;
                    itemEntity.Advert = (int)enumConver | (int)enumDiscover;
                    itemEntity.AdvertOrder = advertorder;
                    itemEntity.PublishAreaID = publishzone;
                    itemEntity.ActivePlace = activeplace;
                    itemEntity.Html = html;
                    itemEntity.BookListPath = booklistpath;
                    itemEntity.EventItemFlag = eventItemFlag;
                    itemEntity.Title2 = title2;
                    itemEntity.StartTime2 = starttime2;
                    itemEntity.EndTime2 = endtime2;
                    itemEntity.Singlegroup = singlegroup;
                    itemEntity.FestivalID = festivalid;

                    itemEntity.AdsEndTime = adsendtime;
                    itemEntity.DiscoverAdsEndTime = discoveradsendtime;

                    PictureEntity pictureEntity = null;
                    #region 封面图片以前必填项，现在改为不必填项
                    //pictureEntity = bllPicServer.QueryPictureEntity(itemEntity.PictureID);
                    //if (pictureEntity.PicturePath != imgurl)
                    //{
                    //    pictureEntity.PicturePath = imgurl;
                    //}
                    //else
                    //{
                    //    pictureEntity = null;
                    //}
                    #endregion
                    if (itemEntity.PictureID > 0 && !string.IsNullOrEmpty(imgurl))
                    {
                        pictureEntity = bllPicServer.QueryPictureEntity(itemEntity.PictureID);
                        if (pictureEntity.PicturePath != imgurl)
                        {
                            pictureEntity.PicturePath = imgurl;
                        }
                        else
                        {
                            pictureEntity = null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(imgurl))
                    {
                        pictureEntity = new PictureEntity();
                        pictureEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                        pictureEntity.PicturePath = imgurl;
                        pictureEntity.PictureState = 0;
                    }

                    PictureEntity themePictureEntity = null;
                    if (itemEntity.ThemePictureID > 0 && !string.IsNullOrEmpty(imgthemeurl))
                    {
                        themePictureEntity = bllPicServer.QueryPictureEntity(itemEntity.ThemePictureID);
                        if (themePictureEntity.PicturePath != imgthemeurl)
                        {
                            themePictureEntity.PicturePath = imgthemeurl;
                        }
                        else
                        {
                            themePictureEntity = null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(imgthemeurl))
                    {
                        themePictureEntity = new PictureEntity();
                        themePictureEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                        themePictureEntity.PicturePath = imgthemeurl;
                        themePictureEntity.PictureState = 0;
                    }

                    PictureEntity carouselPicEntity = null;
                    if (itemEntity.CarouselPictureID > 0 && !string.IsNullOrEmpty(imgcarouselurl))
                    {
                        carouselPicEntity = bllPicServer.QueryPictureEntity(itemEntity.CarouselPictureID);
                        if (carouselPicEntity.PicturePath != imgcarouselurl)
                        {
                            carouselPicEntity.PicturePath = imgcarouselurl;
                        }
                        else
                        {
                            carouselPicEntity = null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(imgcarouselurl))
                    {
                        carouselPicEntity = new PictureEntity();
                        carouselPicEntity.PictureServerID = bllPicServer.QueryPicServer(WebMaster.ImageServerID).PictureServerID.ToString();
                        carouselPicEntity.PicturePath = imgcarouselurl;
                        carouselPicEntity.PictureState = 0;
                    }

                    if (bllEventItem.IsUseableTitle(eventItemID, title))
                    {
                        bool isSuccess = bllEventItem.Update(itemEntity, pictureEntity, themePictureEntity, carouselPicEntity);
                        if (isSuccess)
                        {
                            status = "{\"status\":1}";
                        }
                        else
                        {
                            status = "{\"status\":0}";
                        }
                    }
                    else
                    {
                        status = "{\"status\":2}";
                    }
                    #endregion
                }
                catch
                {
                    return status;
                }
            }

            return status;
        }

        /// <summary>
        /// 更改推荐状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ChangeRecommendState(HttpContext context)
        {
            string status = "{\"status\":0}";
            int eventItemID = 0;
            int recommend = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && int.TryParse(context.Request.Form["status"], out recommend) && recommend > -1)
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();

                bool isSuccess = bll.ChangeRecommendState(eventItemID, recommend);
                if (isSuccess)
                {
                    status = "{\"status\":1}";
                }
            }
            else
            {
                status = "{\"status\":-1}";
            }

            return status;
        }

        /// <summary>
        /// 首页轮播状态设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ChangeAdvertState(HttpContext context)
        {
            string status = "{\"status\":0}";
            int eventItemID = 0;
            int advert = 0;
            int advertOrder = 0;
            DateTime dateTime = new DateTime(4000, 1, 1);
            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && int.TryParse(context.Request.Form["status"], out advert) &&
                int.TryParse(context.Request.Form["order"], out advertOrder))
            {
                DateTime.TryParse(context.Request.Form["adsendtime"], out dateTime);
                BCtrl_EventItem bll = new BCtrl_EventItem();

                //当设置轮播时，检查轮播数量
                if (bll.IsCanSetAdvert(eventItemID))
                {
                    if (bll.IsCanSetOrderNum(eventItemID, advertOrder))
                    {
                        bool isSuccess = bll.ChangeAdvertState(eventItemID, advert, advertOrder,dateTime);
                        if (isSuccess)
                        {
                            status = "{\"status\":1}";
                        }
                    }
                    else
                    {
                        status = "{\"status\":3}";
                    }
                }
                else
                {
                    int num = WebMaster.ArticlePageAdvertNum;
                    status = "{\"status\":2, \"num\":" + num + "}";
                }
            }
            else
            {
                status = "{\"status\":-1}";
            }

            return status;
        }

        /// <summary>
        /// 发现轮播状态设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ChangeCarouselState(HttpContext context)
        {
            string status = "{\"status\":0}";
            int eventItemID = 0;
            int advert = 0;
            DateTime dateTime = new DateTime(4000, 1, 1);
            int advertOrder = 0;
            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && int.TryParse(context.Request.Form["status"], out advert) &&
                int.TryParse(context.Request.Form["order"], out advertOrder))
            {
                DateTime.TryParse(context.Request.Form["adsendtime"], out dateTime);
                BCtrl_EventItem bll = new BCtrl_EventItem();

                //当设置轮播时，检查轮播数量
                if (bll.IsCanSetCarousel(eventItemID))
                {
                    bool isSuccess = bll.ChangeCarouselState(eventItemID, advert, advertOrder, dateTime);
                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                }
                else
                {
                    int num = WebMaster.ArticlePageCarouselNum;
                    status = "{\"status\":2, \"num\":" + num + "}";
                }
            }
            else
            {
                status = "{\"status\":-1}";
            }

            return status;
        }

        [AjaxHandlerAction]
        public string DeleteEventItem(HttpContext context)
        {
            string status = "{\"status\":0}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
            {
                BCtrl_EventItemGroup egBll = new BCtrl_EventItemGroup();
                BCtrl_EventItem bll = new BCtrl_EventItem();
                EventItemEntity entity= bll.QueryEntity(eventItemID);
                if (entity == null)
                {
                    return "{\"status\":-1}"; ;
                }
                //删除所有用户里关于这篇文章的收藏
                bool isSuccess= bll.RemoveFavor(entity.EventItemGUID);
                if (!isSuccess)
                {
                    return "{\"status\":-1}"; ;
                }
                //更改轮播状态
                isSuccess = bll.ChangeAdvertState(eventItemID, 0, 1000);
                if (!isSuccess)
                {
                    return "{\"status\":-1}"; ;
                }
               
                egBll.DeleteGroupRel(eventItemID);
                isSuccess = bll.Delete(eventItemID);
                if (isSuccess)
                {
                    status = "{\"status\":1}";
                }
            }
            else
            {
                status = "{\"status\":-1}";
            }

            return status;
        }

        [AjaxHandlerAction]
        public string SearchItem(HttpContext context)
        {
            dynamic list = new { status = 0 };
            int eventItemID = 0;
            try
            {
                if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
                {
                    BCtrl_EventItem bll = new BCtrl_EventItem();
                    DataTable table = bll.QueryViewOnlyEventItemTable(eventItemID);

                    if (table != null && table.Rows.Count > 0)
                    {
                        var items = new { html = "@@@hj中国堙？?$", type2 = 12, type1 = 6 };
                        list = new { status = 1, items = items };
                    }
                    else
                    {
                        list = new { status = 0 }; //数据错误
                    }
                }
                else
                {
                    list = new { status = -1 };//参数错误
                }
            }
            catch
            {
            }
            string result = StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
            return StringUti.ToUnicode(JsonObj<object>.ToJsonString(list));
        }

        /// <summary>
        /// 是否可以设置文章轮播
        /// </summary>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsCanSetAdvert(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID))
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isCanusable = bll.IsCanSetAdvert(eventItemID);
                int canconvetnum = WebMaster.ArticlePageAdvertNum;

                if (isCanusable)
                {

                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0, \"num\":" + canconvetnum + "}";
                }
            }

            return status;
        }

        /// <summary>
        /// 是否可以设置发现轮播图片
        /// </summary>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsCanSetCarousel(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID))
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isCanusable = bll.IsCanSetCarousel(eventItemID);
                int canconvetnum = WebMaster.ArticlePageCarouselNum;

                if (isCanusable)
                {

                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0, \"num\":" + canconvetnum + "}";
                }
            }

            return status;
        }

        /// <summary>
        /// 取消首页轮播
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string CancelAdvert(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isSuccess = bll.ChangeAdvertState(eventItemID, 0, 1000);

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
        /// 取消发现轮播
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string CancelCarousel(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isSuccess = bll.ChangeCarouselState(eventItemID, 0);

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
        /// 删除主题图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DelThemePic(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventItemID = 0;
            string imgurl = context.Request.Form["imgurl"];
            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && !string.IsNullOrEmpty(imgurl))
            {
                bool isSuccess = new BCtrl_EventItem().DeleteThemePicture(eventItemID);
                if (isSuccess)
                {
                    UploadImageHelper uploadHelper = new UploadImageHelper();

                    string imgKey = uploadHelper.ConvertImgkeyByUrl(imgurl, false);
                    if (imgKey != "")
                    {
                        bool isDelSuccess = uploadHelper.DeleteImage(imgKey);
                    }
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}"; //删除失败
                }
            }

            return status;
        }

        /// <summary>
        /// 删除轮播图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DelCarouselPic(HttpContext context)
        {
            string status = "{\"status\":-1}";

            int eventItemID = 0;
            string imgurl = context.Request.Form["imgurl"];
            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && !string.IsNullOrEmpty(imgurl))
            {
                bool isSuccess = new BCtrl_EventItem().DeleteCarouselPicture(eventItemID);
                if (isSuccess)
                {
                    UploadImageHelper uploadHelper = new UploadImageHelper();

                    string imgKey = uploadHelper.ConvertImgkeyByUrl(imgurl, false);
                    if (imgKey != "")
                    {
                        bool isDelSuccess = uploadHelper.DeleteImage(imgKey);
                    }
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}"; //删除失败
                }
            }
            return status;
        }


        /// <summary>
        /// 删除封面图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DelConverPic(HttpContext context)
        {
            string status = "{\"status\":-1}";

            int eventItemID = 0;
            string imgurl = context.Request.Form["imgurl"];
            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0 && !string.IsNullOrEmpty(imgurl))
            {
                bool isSuccess = new BCtrl_EventItem().DeleteConverPicture(eventItemID);
                if (isSuccess)
                {
                    UploadImageHelper uploadHelper = new UploadImageHelper();

                    string imgKey = uploadHelper.ConvertImgkeyByUrl(imgurl, false);
                    if (imgKey != "")
                    {
                        bool isDelSuccess = uploadHelper.DeleteImage(imgKey);
                    }
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}"; //删除失败
                }
            }
            return status;
        }

        [AjaxHandlerAction]
        public string IsCanSetOrderNum(HttpContext context)
        {
            string result = "false";
            int eventItemID = 0;
            int advertOrder = 0;

            if (int.TryParse(context.Request.Form["pid"], out eventItemID) && int.TryParse(context.Request.Form["advertorder"], out advertOrder))
            {
                if (new BCtrl_EventItem().IsCanSetOrderNum(eventItemID, advertOrder))
                {
                    result = "true";
                }
            }

            return result;
        }

        /// <summary>
        /// 上传书单文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string ReceiveFile(HttpContext context)
        {
            try
            {
                HttpRequest Request = context.Request;
                HttpPostedFile file_upload = Request.Files["Filedata"];

                //int filetype = 0;
                //if (!int.TryParse(Request.QueryString["filetype"], out filetype))
                //{
                //    filetype = 0;
                //}

                string serverpath = "~/UploadFile/BookList/" + DateTime.Now.ToString("yyyyMM");
                //if (filetype == 1)
                //{
                //    serverpath = "~/UploadFile/BookList/" + DateTime.Now.ToString("yyyyMM");
                //}
                string targetfolder = context.Server.MapPath(serverpath);

                if (!Directory.Exists(targetfolder))
                {
                    Directory.CreateDirectory(targetfolder);
                }
                string booklistname = file_upload.FileName.Substring(0, file_upload.FileName.LastIndexOf("."));
                string ext = file_upload.FileName.Substring(file_upload.FileName.LastIndexOf('.'));
                string filename = string.Concat(booklistname, DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.Millisecond.ToString(), ext);

                file_upload.SaveAs(Path.Combine(targetfolder, filename));

                string virtualPath = string.Concat(serverpath, "/", filename);
                return "{\"fullpath\":\"" + VirtualPathUtility.ToAbsolute(virtualPath) + "\",\"filename\":\"" + filename + "\",\"vpath\":\"" + virtualPath + "\"}";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return "{\"fullpath\":\"#\",\"filename\":\"error\"}";
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string DestroyFiles(HttpContext context)
        {
            string state = "{\"state\":-1}";

            string filename = context.Request.Form["filename"];
            int eventItemID = 0;

            if (!string.IsNullOrEmpty(filename))
            {
                try
                {
                    BCtrl_EventItem bll = new BCtrl_EventItem();
                    filename = context.Server.MapPath(filename);
                    FileInfo fileInfo = new FileInfo(filename);

                    if (fileInfo.Exists)
                    {
                        int.TryParse(context.Request.Form["pid"], out eventItemID);
                        if (eventItemID > 0 && bll.DeleteBookListPath(eventItemID))
                        {
                            File.Delete(filename);
                            state = "{\"state\":1}";
                        }
                        else
                        {
                            File.Delete(filename);
                            state = "{\"state\":1}";
                        }
                    }
                    else
                    {
                        state = "{\"state\":-2}";
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    state = "{\"state\":0}";
                }
            }

            return state;
        }

        /// <summary>
        /// 设置活动报名开关
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string SetActiveApply(HttpContext context)
        {
            string state = "{\"state\":-1}";
            bool isApply = false;
            int eventItemID = 0;

            if (bool.TryParse(context.Request.Form["activeApply"], out isApply) && int.TryParse(context.Request.Form["pid"], out eventItemID) && eventItemID > 0)
            {
                BCtrl_EventItem bll = new BCtrl_EventItem();
                bool isSuccess = bll.SetActiveApply(eventItemID, isApply);

                if (isSuccess)
                {
                    state = "{\"state\":1}";
                }
                else
                {
                    state = "{\"state\":0}";
                }
            }

            return state;
        }

        /// <summary>
        /// 专题分组保存
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GroupRelInsert(HttpContext context)
        {
            string status = "{\"status\":-1}";

            try
            {
                string groupEventName = context.Request.Form["groupname"];
                DateTime publishtime = DateTime.Parse(context.Request.Form["publishtime"]);

                EventItemGroupEntity groupEntity = new EventItemGroupEntity();
                groupEntity.GroupEventName = groupEventName.Trim();
                groupEntity.PublishTime = publishtime;
                groupEntity.CreatedTime = DateTime.Now;
                groupEntity.GroupState = 1;

                string ids = context.Request.Form["ids"];
                string hascarouselIds = context.Request.Form["hascarouselIds"];

                List<EventItemGroupRelEntity> groupRelEntityList = null;
                if (!string.IsNullOrEmpty(ids))
                {
                    groupRelEntityList = new List<EventItemGroupRelEntity>();
                    List<int> eventItemIDList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(o => int.Parse(o)).ToList();

                    //有发现轮播图的设置显示顺序
                    int[] hasCarouselIds = hascarouselIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(o => int.Parse(o)).ToArray();

                    for (int i = 0; i < hasCarouselIds.Count(); i++)
                    {
                        groupRelEntityList.Add(new EventItemGroupRelEntity { EventItemID = hasCarouselIds[i], DisplayOrder = i + 1 });
                        eventItemIDList.Remove(hasCarouselIds[i]);
                    }

                    groupRelEntityList.AddRange(eventItemIDList.Select(item => new EventItemGroupRelEntity { EventItemID = item, DisplayOrder = 999 }).ToList());
                }

                BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();

                if (bll.IsUseableGroupName(0, groupEntity.GroupEventName))
                {
                    bool isSuccess = bll.InsertGroupRel(groupEntity, groupRelEntityList);
                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
                else
                {
                    status = "{\"status\":2}";
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// 专题分组修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GroupRelUpdate(HttpContext context)
        {
            string status = "{\"status\":-1}";

            try
            {
                BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();
                int groupEventID = int.Parse(context.Request.Form["groupid"]);

                string ids = context.Request.Form["ids"];
                List<EventItemGroupRelEntity> groupRelEntityList = null;
                if (!string.IsNullOrEmpty(ids))
                {
                    List<int> eventItemIDList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(o => int.Parse(o)).ToList();
                    List<EventItemGroupRelViewEntity> relViewEntityList = bll.QueryGroupRelViewList(groupEventID);

                    if (relViewEntityList != null && relViewEntityList.Count > 0)
                    {
                        eventItemIDList.AddRange(relViewEntityList.Where(o => !eventItemIDList.Contains(o.EventItemID)).Select(o => o.EventItemID).ToList());
                    }

                    if (!bll.IsHasCoverPicByGroupArticle(eventItemIDList))
                    {
                        return "{\"status\":2}";
                    }


                    if (relViewEntityList != null && relViewEntityList.Count > 0)
                    {
                        groupRelEntityList = relViewEntityList.Select(item => new EventItemGroupRelEntity { EventItemID = item.EventItemID, EventGroupID = item.EventGroupID, DisplayOrder = item.DisplayOrder }).ToList();

                        int[] savedIds = relViewEntityList.Select(item => item.EventItemID).ToArray();
                        eventItemIDList = eventItemIDList.Where(id => !savedIds.Contains(id)).ToList();

                        if (eventItemIDList != null)
                        {
                            List<EventItemGroupRelEntity> newRelEntityList = eventItemIDList.Select(item => new EventItemGroupRelEntity { EventItemID = item, DisplayOrder = 999, EventGroupID = groupEventID }).ToList();
                            groupRelEntityList.AddRange(newRelEntityList);
                        }
                    }
                    else
                    {
                        groupRelEntityList = eventItemIDList.Select(item => new EventItemGroupRelEntity { EventItemID = item, DisplayOrder = 999 }).ToList();
                    }
                }

                bool isSuccess = bll.UpdateGroupRel(groupEventID, groupRelEntityList);
                if (isSuccess)
                {
                    status = "{\"status\":1}";
                }
                else
                {
                    status = "{\"status\":0}";
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// 专题分组名称是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableGroupName(HttpContext context)
        {
            string result = "false";
            int groupEventID = 0;
            string groupEventName = context.Request.Form["groupeventname"];

            if (int.TryParse(context.Request.Form["pid"], out groupEventID) && !string.IsNullOrEmpty(groupEventName))
            {
                if (new BCtrl_EventItemGroup().IsUseableGroupName(groupEventID, groupEventName))
                {
                    result = "true";
                }
            }

            return result;
        }

        /// <summary>
        /// 删除专题分组指定文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GroupRelDelete(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int groupEventid = 0;
            int eventItemID = 0;

            try
            {
                if (int.TryParse(context.Request.Form["groupid"], out groupEventid) && groupEventid > 0 && int.TryParse(context.Request.Form["itemid"], out eventItemID) && eventItemID > 0)
                {
                    BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();
                    EventItemGroupRelEntity groupRelEntity = new EventItemGroupRelEntity();
                    groupRelEntity.EventGroupID = groupEventid;
                    groupRelEntity.EventItemID = eventItemID;

                    bool isSuccess = bll.DeleteGroupRel(groupRelEntity);

                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// 删除专题分组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GroupDelete(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int groupEventid = 0;

            try
            {
                if (int.TryParse(context.Request.Form["groupid"], out groupEventid) && groupEventid > 0)
                {
                    BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();

                    bool isSuccess = bll.DeleteGroup(groupEventid);

                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// 更新专题分组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string GroupUpdate(HttpContext context)
        {
            string status = "{\"status\":-1}";
            try
            {
                int groupEventID = int.Parse(context.Request.Form["groupid"]);
                string groupEventName = context.Request.Form["groupname"].ToString().Trim();
                DateTime publishTime = DateTime.Parse(context.Request.Form["publishtime"]);

                BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();

                EventItemGroupEntity groupEntity = bll.QueryGroupEntity(groupEventID);

                if (groupEntity != null)
                {
                    groupEntity.PublishTime = publishTime;
                    groupEntity.GroupEventName = groupEventName;

                    bool isSuccess = bll.UpdateGroup(groupEntity);

                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
                else
                {
                    status = "{\"status\":2}";
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// 判断指定专题中的文章，该显示序号是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string IsUseableOrderByGroupArticle(HttpContext context)
        {
            string result = "false";
            int eventGroupID = 0;
            int displayOrder = 0;
            int eventItemID = 0;

            if (int.TryParse(context.Request.Form["groupid"], out eventGroupID) && eventGroupID > 0
                && int.TryParse(context.Request.Form["itemid"], out eventItemID) && int.TryParse(context.Request.Form["displayorder"], out displayOrder))
            {
                if (new BCtrl_EventItemGroup().IsUseableOrderByGroupArticle(eventGroupID, eventItemID, displayOrder))
                {
                    result = "true";
                }
            }

            return result;
        }

        /// <summary>
        /// 更新专题中指定文章显示序号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [AjaxHandlerAction]
        public string UpdateGroupRelOrder(HttpContext context)
        {
            string status = "{\"status\":-1}";
            int eventGroupID = 0;
            int eventItemID = 0;
            int displayOrder = 0;

            try
            {
                if (int.TryParse(context.Request.Form["groupid"], out eventGroupID) && eventGroupID > 0
              && int.TryParse(context.Request.Form["itemid"], out eventItemID) && eventItemID > 0
              && int.TryParse(context.Request.Form["displayorder"], out displayOrder))
                {
                    BCtrl_EventItemGroup bll = new BCtrl_EventItemGroup();
                    bool isSuccess = bll.UpdateGroupRelOrder(eventGroupID, eventItemID, displayOrder);

                    if (isSuccess)
                    {
                        status = "{\"status\":1}";
                    }
                    else
                    {
                        status = "{\"status\":0}";
                    }
                }
            }
            catch
            {
                return status;
            }

            return status;
        }

    }
}
