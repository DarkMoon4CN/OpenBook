using Mars.Server.App.Core;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class EventItemModule : ModuleBase
    {
        BCtrl_EventItem eventitemobj = new BCtrl_EventItem();
        BCtrl_CalendarType calendarobj = new BCtrl_CalendarType();
        public EventItemModule()
            : base("/EventItem")
        {
            Post["/Async"] = _ =>
            {
                try
                {
                    List<EventItemEntity> lists = new List<EventItemEntity>();
                    lists = FetchFormData<List<EventItemEntity>>();
                    var tmpGUID=(from p in lists select p.EventItemGUID).Distinct().ToList<Guid>();
                    List<EventItemEntity> tmpList = new List<EventItemEntity>();
                    for (int i = 0; i < tmpGUID.Count; i++)
                    {
                        var dataEntity = lists.Where(p => p.EventItemGUID == tmpGUID[i]).OrderBy(t=>t.StartTime).FirstOrDefault();
                        tmpList.Add(dataEntity);
                    }
                    lists = tmpList;

                    if (lists == null)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "提交参数异常" });
                    }
                    if (lists.Count == 0)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "提交数据为空" });
                    }

                    if (eventitemobj.TrySaveEventItems(lists, CurrentUser.UserID))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "同步成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "同步失败" });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Get["/Async"] = _ =>
            {
                try
                {
                    List<Guid> gids = FecthQueryData<List<Guid>>();
                    List<EventItemEntity> items = eventitemobj.FetchNewEventItems(gids, CurrentUser.UserID);


                    foreach (var item in items)
                    {
                        foreach (var gid in gids)
                        {
                            if (item.EventItemGUID == gid)
                            {
                                items.Remove(item);
                            }
                        }
                    }

                    if (items != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "同步成功", Value = items });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "同步数据失败(Pull)", Value = new List<EventItemEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };

            #region 方法用于Get["/Async"]数据过大时  2015/11/12
            Post["/AsyncTrial"] = _ =>
            {
                try
                {
                    List<Guid> gids = FetchFormData<List<Guid>>();
                    List<EventItemEntity> items = eventitemobj.FetchNewEventItems(gids, CurrentUser.UserID);
                    if (items != null)
                    {
                        if (gids != null)
                        {
                            foreach (var item in items)
                            {
                                foreach (var gid in gids)
                                {
                                    if (item.EventItemGUID == gid)
                                    {
                                        items.Remove(item);
                                    }
                                }
                            }
                            DateTime checkDate = new DateTime(4000, 1, 1);
                            foreach (var item in items) 
                            {
                                if (item.StartTime == checkDate) 
                                {
                                    item.StartTime = null;
                                }
                                if (item.EndTime == checkDate)
                                {
                                    item.EndTime = null;
                                }
                                if (item.StartTime2 == checkDate) 
                                {
                                    item.StartTime2 = null;
                                }
                                if (item.EndTime2 == checkDate)
                                {
                                    item.EndTime2 = null;
                                }
                            }
                        }
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "同步成功", Value = items });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "同步数据失败(Pull)", Value = new List<EventItemEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };
            #endregion 



            Get["/QueryItemsByCTypeID"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int recommend = data.recommend == null ? -1 : data.recommend;
                    int ctype = data.ctypeid;
                    int pagesize = data.pagesize == null ? 20 : data.pagesize;
                    int pageno = data.pageno == null ? 1 : data.pageno;

                    if (pagesize > 50)
                    {
                        pagesize = 50;
                    }

                    List<EventItemEntity> lists = eventitemobj.GetItemListByCalanderTypeID(pageno, pagesize, ctype, recommend, (CurrentUser != null ? CurrentUser.ZoneID : 0));
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<EventItemEntity>() });
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };

            Get["/SearchItems"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    string keyword = data.keyword;
                    int pagesize = data.pagesize == null ? 20 : data.pagesize;
                    if (pagesize > 50)
                        pagesize = 50;
                    int pageno = data.pageno == null ? 1 : data.pageno;
                    List<EventItemEntity> items = eventitemobj.QueryItemList(pageno, pagesize, keyword);
                    List<CalendarTypeEntity> cs = null;
                    if (pageno == 1)
                    {
                        cs = calendarobj.QueryCalendarTypes(keyword, (CurrentUser == null ? 0 : CurrentUser.UserID));
                    }
                    else
                    {
                        cs = new List<CalendarTypeEntity>();
                    }
                    if (items != null && cs != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>() { Status = 1, Msg = "获取成功", Value = items, Value2 = cs });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>() { Status = 0, Msg = "获取失败", Value = new List<EventItemEntity>(), Value2 = new List<CalendarTypeEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>, List<CalendarTypeEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>(), Value2 = new List<CalendarTypeEntity>() });
                }

            };


            Get["/SearchItemsV2"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    string keyword = data.keyword;
                    int pagesize = data.pagesize == null ? 20 : data.pagesize;
                    if (pagesize > 50)
                        pagesize = 50;
                    int pageno = data.pageno == null ? 1 : data.pageno;
                    List<EventItemEntity> items = eventitemobj.QueryItemList(pageno, pagesize, keyword);
                    if (items != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "获取成功", Value = items });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "获取失败", Value = new List<EventItemEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };


            Post["/AddFavor"] = _ =>
            {
                try
                {
                    List<Guid> gids = null;
                    try
                    {
                        gids = FetchFormData<List<Guid>>();
                    }
                    catch { 
                        gids=new List<Guid>();
                        Guid g = FetchFormData().EventItemGUID;
                        gids.Add(g);
                    }

                    if (CurrentUser.SessionID == SessionCenter.CommonSessionID)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "没有客户,不可收藏！" });
                    }


                    if (eventitemobj.AddFavorBatch(gids, CurrentUser.UserID))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "收藏成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "收藏失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/RemoveFavor"] = _ =>
            {
                try
                {
                    List<Guid> gids = null;
                    try
                    {
                        gids = FetchFormData<List<Guid>>();
                    }
                    catch
                    {
                        gids = new List<Guid>();
                        Guid g = FetchFormData().EventItemGUID;
                        gids.Add(g);
                    }


                    if (eventitemobj.RemoveFavorBatch(CurrentUser.UserID,gids))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "操作成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "操作失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };
            Get["/GetMyFavorList"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int pagesize = 20;
                    int pageno = 1;
                    if (data != null)
                    {
                        pagesize = data.pagesize == null ? 20 : data.pagesize;
                        pageno = data.pageno == null ? 1 : data.pageno;
                    }


                    if (pagesize > 50)
                    {
                        pagesize = 50;
                    }

                    List<EventItemEntity> lists = eventitemobj.QueryFavorItemList(pageno, pagesize, CurrentUser.UserID);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<EventItemEntity>() });
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };

            Get["/GetAds"] = _ =>
            {
                try
                {
                    List<EventItemEntity> lists = eventitemobj.GetAds();
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<EventItemEntity>() });
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };

            Get["/GetTextAds"] = _ =>
            {
                try
                {

                    List<EventItemEntity> lists = eventitemobj.GetAds(true);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<EventItemEntity>() });
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };


            Get["/CheckFavorState"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    Guid itemid = data.EventItemGUID;
                    bool state = eventitemobj.CheckFavorState(itemid, CurrentUser.UserID);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "获取成功", Tag = state.ToString() });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "获取失败" });
                }
            };

            Get["/CheckFavorStateV2"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int id = data.EventItemID;
                    var entity = eventitemobj.QueryEntity(id);
                    bool state = eventitemobj.CheckFavorState(entity.EventItemGUID, CurrentUser.UserID);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "获取成功", Tag = state.ToString() });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "获取失败" });
                }
            };

            Get["/GetMyFavorListByDate"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    DateTime dt = data.PublishTime;

                    List<CalendarTypeEntity> list = eventitemobj.QueryMyFavorItemByCalendarType(dt, CurrentUser == null ? 0 : CurrentUser.UserID);
                    return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Value = list, Status = 1, Msg = "获取成功" });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<CalendarTypeEntity>>>.ToJson(new JsonMessageBase<List<CalendarTypeEntity>>() { Value = new List<CalendarTypeEntity>(), Status = 0, Msg = "获取失败" });
                }

            };

            Get["/GetEventGroups"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int pagesize = 10;
                    int pageno = 1;
                    if (data != null)
                    {
                        pagesize = data.pagesize == null ? 10 : data.pagesize;
                        pageno = data.pageno == null ? 1 : data.pageno;
                    }
                    if (pagesize > 50)
                    {
                        pagesize = 50;
                    }

                    List<EventItemGroupEntity> lists = eventitemobj.QueryAllEventItemGroups(pageno, pagesize);
                    if (lists != null)
                    {
                        return JsonObj<JsonMessageBase<List<EventItemGroupEntity>>>.ToJson(new JsonMessageBase<List<EventItemGroupEntity>>() { Status = 1, Msg = "获取成功", Value = lists });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<EventItemGroupEntity>>>.ToJson(new JsonMessageBase<List<EventItemGroupEntity>>() { Status = 0, Msg = "读取数据过程中发生错误", Value = new List<EventItemGroupEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<EventItemEntity>() });
                }
            };

            Get["/QueryFestivalEvents"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    DateTime dt = data.Date;
                    List<EventItemEntity> list = eventitemobj.QueryFestvalEvents(dt);
                    list = list.OrderBy(t => t.StartTime).ToList<EventItemEntity>();
                    list = list.OrderBy(t => t.StartTime2).ToList<EventItemEntity>();
                    List<FestivalEntity> festivalList = eventitemobj.QueryFestvalExceptExistsEvents(dt);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>,List<FestivalEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>,List<FestivalEntity>>() { Value = list, Value2=festivalList, Status = 1, Msg = "获取成功" });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<EventItemEntity>,List<FestivalEntity>>>.ToJson(new JsonMessageBase<List<EventItemEntity>,List<FestivalEntity>>() { Value = new List<EventItemEntity>(), Value2=new List<FestivalEntity>(), Status = 0, Msg = "获取失败" });
                }
            };
            #region 用户日程的同步重构(2)
            Post["AsyncPull"] = _ =>
            {
                try
                {
                    List<EventItemAsyncPullRequest> items = new List<EventItemAsyncPullRequest>();
                    items = FetchFormData<List<EventItemAsyncPullRequest>>();
                    List<UserEventItemEntity> entitys = eventitemobj.EventItem_AsyncPull(items, CurrentUser.UserID);
                    if (entitys != null)
                    {
                        return JsonObj<JsonMessageBase<List<UserEventItemEntity>>>.ToJson(new JsonMessageBase<List<UserEventItemEntity>>() { Status = 1, Msg = "同步成功", Value = entitys });
                    }
                    else 
                    {
                        return JsonObj<JsonMessageBase<List<UserEventItemEntity>>>.ToJson(new JsonMessageBase<List<UserEventItemEntity>>() { Status = 0, Msg = "同步数据失败(Pull)", Value = new List<UserEventItemEntity>() });
                    }
                }
                catch(Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<List<UserEventItemEntity>>>.ToJson(new JsonMessageBase<List<UserEventItemEntity>>() { Status = 0, Msg = ex.Message, Value = new List<UserEventItemEntity>() });
                }
            };

            Post["AsyncPush"] = _ => 
            {
                try
                {
                    List<UserEventItemEntity> lists = new List<UserEventItemEntity>();
                    
                    lists = FetchFormData<List<UserEventItemEntity>>();
                    if (lists == null)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "提交参数异常" });
                    }
                    var tmpGUID = (from p in lists select p.EventItemGUID).Distinct().ToList<Guid>();
                    List<UserEventItemEntity> tmpList = new List<UserEventItemEntity>();
                    for (int i = 0; i < tmpGUID.Count; i++)
                    {
                        var dataEntity = lists.Where(p => p.EventItemGUID == tmpGUID[i]).OrderBy(t => t.StartTime).FirstOrDefault();
                        tmpList.Add(dataEntity);
                    }
                    lists = tmpList;
                    if (lists.Count == 0)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "提交数据为空" });
                    }

                    if (eventitemobj.EventItem_AsyncPush(lists, CurrentUser.UserID))
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "同步成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "同步失败" });
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };
            #endregion
        }
    }
}