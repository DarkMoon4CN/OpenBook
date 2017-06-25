using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Utils;
using System.Data;
using System.Text.RegularExpressions;
namespace Mars.Server.BLL
{
    public class BCtrl_EventItem
    {
        EventItemDAO eventitemobj = new EventItemDAO();

        public List<EventItemEntity> QueryFestvalEvents(DateTime dt)
        {
            try
            {
                return eventitemobj.QueryFestvalEvents(dt);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new List<EventItemEntity>();
            }
        }

        /// <summary>
        /// 获取没有关联节日文章的节日实体
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<FestivalEntity> QueryFestvalExceptExistsEvents(DateTime dt)
        {
            return eventitemobj.QueryFestvalExceptExistsEvents(dt);
        }

        public List<EventItemGroupEntity> QueryAllEventItemGroups(int pageindex, int pagesize)
        {
            List<EventItemGroupEntity> items = new List<EventItemGroupEntity>();
            DataTable dt = eventitemobj.QueryAllEventItemGroups(pageindex, pagesize);

            int groupid = 0;
            EventItemGroupEntity tempgroup = null;
            foreach (DataRow dr in dt.Rows)
            {
                int gid = dr["GroupEventID"].ToString().ToInt();
                if (groupid == gid)
                {
                    tempgroup.EventItems.Add(new EventItemEntity()
                    {
                        EventItemID = dr["EventItemID"].ToString().ToInt(),
                        Title = dr["Title"].ToString(),
                        Content = dr["Content"].ToString(),
                        Domain = dr["Domain"].ToString(),
                        PicturePath = dr["PicturePath"].ToString(),
                        PictureID = dr["PictureID"].ToString().ToInt(),
                        EventItemGUID = Guid.Parse(dr["EventItemGUID"].ToString()),
                        StartTime = Convert.ToDateTime(dr["StartTime"]),
                        EndTime = Convert.ToDateTime(dr["EndTime"]),
                        LikeCnt= dr["LikeCnt"].ToString().ToInt(),
                        CommentCnt = dr["CommentCnt"].ToString().ToInt(),
                        Url = dr["Url"].ToString()
                    });
                }
                else
                {
                    if (tempgroup != null)
                    {
                        items.Add(tempgroup);
                    }
                    tempgroup = new EventItemGroupEntity();
                    tempgroup.EventItems = new List<EventItemEntity>();
                    tempgroup.EventItems.Add(new EventItemEntity()
                    {
                        EventItemID = dr["EventItemID"].ToString().ToInt(),
                        Title = dr["Title"].ToString(),
                        Content = dr["Content"].ToString(),
                        Domain = dr["Domain"].ToString(),
                        PicturePath = dr["PicturePath"].ToString(),
                        PictureID = dr["PictureID"].ToString().ToInt(),
                        EventItemGUID = Guid.Parse(dr["EventItemGUID"].ToString()),
                        StartTime=Convert.ToDateTime(dr["StartTime"]),
                        EndTime=Convert.ToDateTime(dr["EndTime"]),
                        LikeCnt = dr["LikeCnt"].ToString().ToInt(),
                        CommentCnt = dr["CommentCnt"].ToString().ToInt(),
                        Url = dr["Url"].ToString()
                    });
                    tempgroup.GroupEventID = gid;
                    groupid = gid;
                }
            }

            if (tempgroup != null)
            {
                items.Add(tempgroup);
            }

            return items;
        }

        //public DataTable FetchAllItemGUID(int userid, DateTime limittime)
        //{
        //    try
        //    {
        //        SqlParameter[] prms = { 
        //                                new SqlParameter("@UserID",userid),
        //                                new SqlParameter("@LimitTime",limittime)
        //                              };
        //        string sql = "select EventItemGUID from M_EventItem where UserID=@UserID and CreateTime>=@LimitTime";
        //        return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.WriteLog(ex);
        //        return null;
        //    }
        //}

        public List<EventItemEntity> GetItemListByCalanderTypeID(int pageno, int pagesize, int typeid, int recommandflag, int zoneid)
        {
            return eventitemobj.GetItemListByCalanderTypeID(pageno, pagesize, typeid, recommandflag, zoneid);

        }
        public List<EventItemEntity> FetchNewEventItems(List<Guid> gids, int userid)
        {
            if (gids == null)
                gids = new List<Guid>();
            List<KeyValuePairEntity<Guid>> tags = new List<KeyValuePairEntity<Guid>>();
            List<KeyValuePairEntity<Guid>> pics = new List<KeyValuePairEntity<Guid>>();
            List<ReminderEntity> remiders = new List<ReminderEntity>();
            List<EventItemEntity> items = eventitemobj.FetchNewEventItems(gids, userid, out tags, out pics, out remiders);
            if (items != null)
            {
                foreach (EventItemEntity item in items)
                {

                    if (item.Reminders == null)
                    {
                        item.Reminders = new List<ReminderEntity>();
                    }

                    var res = remiders.FindAll(t => t.EventItemGUID == item.EventItemGUID);
                    item.Reminders.AddRange(res);

                    if (item.Tags == null)
                    {
                        item.Tags = new List<string>();
                    }
                    List<KeyValuePairEntity<Guid>> parts = tags.FindAll(t => t.T1 == item.EventItemGUID);
                    foreach (KeyValuePairEntity<Guid> p in parts)
                    {
                        item.Tags.Add(p.V);
                    }

                    if (item.Pics == null)
                    {
                        item.Pics = new List<int>();
                    }

                    List<KeyValuePairEntity<Guid>> parts2 = pics.FindAll(t => t.T1 == item.EventItemGUID);
                    foreach (KeyValuePairEntity<Guid> p in parts2)
                    {
                        item.Pics.Add(int.Parse(p.V));
                    }
                }
            }
            return items;
        }


        public bool TrySaveEventItems(List<EventItemEntity> items, int userid)
        {
            foreach (EventItemEntity item in items)
            {
                item.UserID = userid;

                if (item.Reminders != null)
                {
                    foreach (var re in item.Reminders)
                    {
                        re.UserID = userid;
                        re.EventItemGUID = item.EventItemGUID;
                    }
                }
                else
                {
                    item.Reminders = new List<ReminderEntity>();
                    var re = new ReminderEntity();
                    re.EventItemGUID = item.EventItemGUID;
                    item.Reminders.Add(re);
                }

                if (item.Tags == null)
                {
                    item.Tags = new List<string>();
                }

                if (item.Pics == null)
                {
                    item.Pics = new List<int>();
                }

            }
            return eventitemobj.TrySaveEventItems(items);
        }

        public List<EventItemEntity> QueryItemList(int pageno, int pagesize, string keyword)
        {
            return eventitemobj.QueryItemList(pageno, pagesize, keyword.ToSafeString());
        }

        public List<EventItemEntity> QueryFavorItemList(int pageno, int pagesize, int userid)
        {
            return eventitemobj.QueryFavorItemList(pageno, pagesize, userid);
        }

        public bool AddFavor(Guid itemguid, int userid)
        {
            return eventitemobj.AddFavor(itemguid, userid);
        }

        public bool RemoveFavor(int userid, Guid itemguid)
        {
            return eventitemobj.RemoveFavor(userid, itemguid);
        }
        public bool RemoveFavor(Guid itemguid)
        {
            return eventitemobj.RemoveFavor(itemguid);
        
        }

        public bool AddFavorBatch(List<Guid> itemguid, int userid)
        {
            return eventitemobj.AddFavorBatch(itemguid, userid);
        }

        public bool RemoveFavorBatch(int userid, List<Guid> itemguid)
        {
            return eventitemobj.RemoveFavorBatch(userid, itemguid);
        }

        public bool RemoveFavorBatch(List<Guid> itemguid)
        {
            return eventitemobj.RemoveFavorBatch(itemguid);
        }

        public List<EventItemEntity> GetAds(bool ishome = false)
        {
            return eventitemobj.GetAds(ishome);
        }
        public bool CheckFavorState(Guid itemguid, int userid)
        {
            return eventitemobj.CheckFavorState(itemguid, userid);
        }

        public List<CalendarTypeEntity> QueryMyFavorItemByCalendarType(DateTime dt, int userid)
        {
            List<CalendarTypeEntity> rt = new List<CalendarTypeEntity>();
            List<EventItemEntity> items = eventitemobj.QueryMyFavorItemByCalendarType(dt, userid);

            if (items != null)
            {
                var data = items.GroupBy(v => new { v.CalendarTypeID, v.CalendarTypeName }).Select(g => new { C = g.Key, Events = g.ToList() }).ToList();
                foreach (var item in data)
                {
                    CalendarTypeEntity c = new CalendarTypeEntity();
                    c.CalendarTypeID = item.C.CalendarTypeID;
                    c.CalendarTypeName = item.C.CalendarTypeName;
                    c.Events = item.Events;
                    rt.Add(c);
                }
                return rt;
            }
            else
                return null;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="searchEntity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryViewEventItemTable(EventItemSearchEntity searchEntity, out int totalcnt)
        {
            return eventitemobj.QueryViewEventItemTable(searchEntity, out totalcnt);
        }

        public DataTable QueryViewEventItemTable(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            return eventitemobj.QueryViewEventItemTable(pageIndex, pageSize, orderType, strWhere, out recordCount);
        }

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public DataTable QueryViewOnlyEventItemTable(int eventItemID)
        {
            return eventitemobj.QueryViewOnlyEventItemTable(eventItemID);
        }

        /// <summary>
        /// 判断文章标题是否可用
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsUseableTitle(int eventItemID, string title)
        {
            return eventitemobj.IsUseableTitle(eventItemID, title);

        }

        /// <summary>
        /// 管理员添加  
        /// 只添加后台文章发布所需字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="converPicEntity">封面图片实体</param>
        /// <param name="themePicEntity">主题图片实体</param>
        /// <param name="carouselPicEntity">轮播图片实体</param>
        /// <returns></returns>
        public bool Insert(EventItemEntity entity, PictureEntity converPicEntity, PictureEntity themePicEntity, PictureEntity carouselPicEntity)
        {
            return eventitemobj.Insert(entity, converPicEntity, themePicEntity, carouselPicEntity);
        }

        /// <summary>
        /// 设置推荐状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        public bool ChangeRecommendState(int eventItemID, int recommend)
        {
            return eventitemobj.ChangeRecommendState(eventItemID, recommend);
        }

        /// <summary>
        /// 设置首页轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool ChangeAdvertState(int eventItemID, int advert, int advertOrder)
        {
            return eventitemobj.ChangeAdvertState(eventItemID, advert, advertOrder);
        }
        /// <summary>
        /// 设置首页轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool ChangeAdvertState(int eventItemID, int advert, int advertOrder,DateTime dateTime)
        {
            return eventitemobj.ChangeAdvertState(eventItemID, advert, advertOrder, dateTime);
        }

        /// <summary>
        /// 设置发现轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>      
        /// <returns></returns>
        public bool ChangeCarouselState(int eventItemID, int advert)
        {
            return eventitemobj.ChangeCarouselState(eventItemID, advert);
        }
        /// <summary>
        /// 设置发现轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>      
        /// <returns></returns>
        public bool ChangeCarouselState(int eventItemID, int advert,DateTime dateTime)
        {
            return eventitemobj.ChangeCarouselState(eventItemID, advert, dateTime);
        }
        public bool ChangeCarouselState(int eventItemID, int advert,int order , DateTime dateTime)
        {
            return eventitemobj.ChangeCarouselState(eventItemID, advert,order , dateTime);
        }

        public bool Delete(int eventItemID)
        {
            return eventitemobj.Delete(eventItemID);
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public EventItemViewEntity QueryViewEntity(int eventItemID)
        {
            return eventitemobj.QueryViewEntity(eventItemID);
        }

        public EventItemEntity QueryEntity(int eventItemID)
        {
            return eventitemobj.QueryEntity(eventItemID);
        }


        /// <summary>
        ///  修改  
        /// 只添加后台文章发布所需字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pictureEntity">封面图片</param>
        /// <param name="themePicEntity">主题图片</param>
        /// <param name="carouselPicEntity">轮播图片</param>
        /// <returns></returns>
        public bool Update(EventItemEntity entity, PictureEntity pictureEntity, PictureEntity themePicEntity, PictureEntity carouselPicEntity)
        {
            return eventitemobj.Update(entity, pictureEntity, themePicEntity, carouselPicEntity);
        }

        /// <summary>
        /// 查询视图实体(新闻页调用)
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public EventItemViewEntity QueryViewEntity(Guid eventItemGUID)
        {
            return eventitemobj.QueryViewEntity(eventItemGUID);
        }

        /// <summary>
        /// HTML文本中的图片进行格式转换
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string DealHtmlImg(string html)
        {
            try
            {
                string regImg = "<img\\s*src=\"(.*?)\".*?/>";
                Regex rexExp = new Regex(regImg, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                html = rexExp.Replace(html, "<img class=\"ui-imglazyload preload\" data-url=\"$1?imageView2/0/interlace/1/format/jpg\" data-original=\"$1?imageView2/0/interlace/1/format/jpeg\" data-id=\"img225\" data-attr=\"attr\">");
                return html;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return html;
            }
        }

        /// <summary>
        /// 是否可以设置文章轮播
        /// </summary>      
        /// <returns></returns>
        public bool IsCanSetAdvert(int eventItemID)
        {
            int canadvertnum = WebMaster.ArticlePageAdvertNum;
            return eventitemobj.IsCanSetAdvert(eventItemID, canadvertnum);
        }

        /// <summary>
        /// 是否可以设置当前轮播序号
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool IsCanSetOrderNum(int eventItemID, int advertOrder)
        {
            return eventitemobj.IsCanSetOrderNum(eventItemID, advertOrder);
        }

        /// <summary>
        /// 删除主题图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteThemePicture(int eventItemID)
        {
            return eventitemobj.DeleteThemePicture(eventItemID);
        }

        /// <summary>
        /// 删除书单文件路径
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteBookListPath(int eventItemID)
        {
            return eventitemobj.DeleteBookListPath(eventItemID);
        }

        /// <summary>
        /// 设置活动报名开关
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetActiveApply(int eventItemID, bool state)
        {
            return eventitemobj.SetActiveApply(eventItemID, state);
        }

        /// <summary>
        /// 删除轮播图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteCarouselPicture(int eventItemID)
        {
            return eventitemobj.DeleteCarouselPicture(eventItemID);
        }

        /// <summary>
        /// 删除封面图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteConverPicture(int eventItemID)
        {
            return eventitemobj.DeleteConverPicture(eventItemID);
        }

        /// <summary>
        /// 根据IDs查询文章
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryViewEventItemTableByIDs(string ids, int pageIndex, int pageSize, out int totalcnt)
        {
            return eventitemobj.QueryViewEventItemTableByIDs(ids, pageIndex, pageSize, out totalcnt);
        }

        /// <summary>
        /// 查询已设置文章发现轮播的数量
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="canadvertnum">配置中一共可以设置轮播数量</param>
        /// <returns></returns>
        public bool IsCanSetCarousel(int eventItemID)
        {
            int carouselnum = WebMaster.ArticlePageCarouselNum;
            return eventitemobj.IsCanSetCarousel(eventItemID, carouselnum);
        }


        #region 2015-12-23  更新同步函数
        public List<UserEventItemEntity> EventItem_AsyncPull(List<EventItemAsyncPullRequest> items, int userid)
        {
            List<KeyValuePairEntity<Guid>> tags = new List<KeyValuePairEntity<Guid>>();
            List<KeyValuePairEntity<Guid>> pics = new List<KeyValuePairEntity<Guid>>();
            List<ReminderEntity> reminders = new List<ReminderEntity>();
            if (items == null) 
            {
                items = new List<EventItemAsyncPullRequest>();
            }
            List<EventItemAsyncPullRequest> itemFlag = new List<EventItemAsyncPullRequest>();
            var tmpGUID = (from p in items select p.EventItemGUID).Distinct().ToList();
            for (int i = 0; i < tmpGUID.Count; i++)
            {
                var dataEntity = items.Where(p => p.EventItemGUID == tmpGUID[i]).FirstOrDefault();
                itemFlag.Add(dataEntity);
            }

            List<UserEventItemEntity> entitys = eventitemobj.EventItem_AsyncPull(itemFlag, userid, out tags, out pics, out reminders);
            if (entitys != null)
            {
                foreach (UserEventItemEntity item in entitys)
                {
                    if (item.Reminders == null)
                    {
                        item.Reminders = new List<ReminderEntity>();
                    }

                    var res = reminders.FindAll(t => t.EventItemGUID == item.EventItemGUID);
                    item.Reminders.AddRange(res);

                    if (item.Tags == null)
                    {
                        item.Tags = new List<string>();
                    }
                    List<KeyValuePairEntity<Guid>> parts = tags.FindAll(t => t.T1 == item.EventItemGUID);
                    foreach (KeyValuePairEntity<Guid> p in parts)
                    {
                        item.Tags.Add(p.V);
                    }

                    if (item.Pics == null)
                    {
                        item.Pics = new List<int>();
                    }

                    List<KeyValuePairEntity<Guid>> parts2 = pics.FindAll(t => t.T1 == item.EventItemGUID);
                    foreach (KeyValuePairEntity<Guid> p in parts2)
                    {
                        item.Pics.Add(int.Parse(p.V));
                    }
                }
            }
            return entitys;
        }


        public bool EventItem_AsyncPush(List<UserEventItemEntity> items, int userid) 
        {
            foreach (UserEventItemEntity item in items)
            {
                item.UserID = userid;

                if (item.Reminders != null)
                {
                    foreach (var re in item.Reminders)
                    {
                        re.UserID = userid;
                        re.EventItemGUID = item.EventItemGUID;
                    }
                }
                else
                {
                    item.Reminders = new List<ReminderEntity>();
                    var re = new ReminderEntity();
                    re.EventItemGUID = item.EventItemGUID;
                    item.Reminders.Add(re);
                }

                if (item.Tags == null)
                {
                    item.Tags = new List<string>();
                }

                if (item.Pics == null)
                {
                    item.Pics = new List<int>();
                }

            }
            return eventitemobj.EventItem_AsyncPush(items);
        }
        #endregion 
    }
}
