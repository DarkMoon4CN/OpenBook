using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
namespace Mars.Server.DAO
{
    public class EventItemDAO
    {

        public List<EventItemEntity> QueryFestvalEvents(DateTime dt)
        {
            try
            {
                string sql = "SELECT EventItemID,EventItemGUID,Title,Title2,Url,EventItemFlag,fp.Domain,fp.PicturePath,StartTime,EndTime  FROM M_EventItem   AS e ";
                sql += " LEFT JOIN  [dbo].[M_V_Picture] AS  fp ON  e.PictureID=fp.PictureID  ";
                sql += " WHERE  StartTime2<=@Date AND EndTime2>=@Date AND EventItemState !=0  ";
                sql += " ORDER BY StartTime  DESC ";
                string sql2 = "SELECT rel.EventItemID,EventItemGUID,Title,Title2,Url,0 as EventItemFlag,fp.Domain,fp.PicturePath,item.StartTime,item.EndTime FROM M_EventItemGroups gr ";
                sql2 += " INNER JOIN M_EventItem_Group_Rel  rel ON rel.EventGroupID=gr.GroupEventID INNER JOIN M_EventItem item ON rel.EventItemID=item.EventItemID ";
                sql2 += " LEFT JOIN  [dbo].[M_V_Picture] AS  fp ON  item.PictureID=fp.PictureID ";
                sql2 += " WHERE gr.PublishTime<=@EndDate AND gr.PublishTime>=@StartDate AND gr.publishtime<= CONVERT(VARCHAR(20),DATEADD(DAY,1,GETDATE()),23) ";
                sql2 += " ORDER BY gr.PublishTime  DESC  ";
                List<EventItemEntity> list = new List<EventItemEntity>();
                List<EventItemEntity> list1 = null;
                List<EventItemEntity> list2 = null;

                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    list1 = con.Query<EventItemEntity>(sql, new { Date = dt }).ToList();
                    DateTime startdate = dt.Date;
                    DateTime enddate = dt.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") ? DateTime.Now : dt.Date.AddDays(1).AddSeconds(-1);
                    list2 = con.Query<EventItemEntity>(sql2, new { StartDate = startdate, EndDate = enddate }).ToList();
                }

                if (list1 != null && list1.Count > 0)
                {
                    list.AddRange(list1);
                }

                if (list2 != null && list2.Count > 0)
                {
                    list.AddRange(list2);
                }
                return list;

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
            try
            {
                string sql = " SELECT fe.* FROM M_Festival fe LEFT JOIN M_EventItem item ON fe.FestivalID=item.FestivalID WHERE item.FestivalID IS NULL AND fe.StartTime <= @Date AND fe.EndTime >=@Date";
                sql += " AND  fe.FestivalType=3  ORDER BY fe.StartTime DESC ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<FestivalEntity>(sql, new { Date = dt }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new List<FestivalEntity>();
            }
        }

        public DataTable QueryAllEventItemGroups(int pageindex, int pagesize)
        {
            try
            {
                SqlParameter[] prms = {
                                      new SqlParameter("@pageIndex",pageindex),
                                      new SqlParameter("@PageSize",pagesize)
                                      };
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_queryEventGroup", prms).Tables[0];
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new DataTable();
            }
        }

        public DataTable FetchAllItemGUID(int userid, DateTime limittime)
        {
            try
            {
                SqlParameter[] prms = { 
                                        new SqlParameter("@UserID",userid),
                                        new SqlParameter("@LimitTime",limittime)
                                      };
                string sql = @"SELECT * FROM (
SELECT EventItemGUID FROM [dbo].[M_EventItem] WHERE RepeatTypeID>0 and UserID=@UserID and EventItemState=1
UNION all
SELECT EventItemGUID FROM [dbo].[M_EventItem] WHERE RepeatTypeID=0 and UserID=@UserID AND StartTime>=LimitTime and EventItemState=1)
ORDER BY EventItemGUID";
                return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<EventItemEntity> FetchNewEventItems(List<Guid> gids, int userid, out List<KeyValuePairEntity<Guid>> tags, out List<KeyValuePairEntity<Guid>> Pics, out List<ReminderEntity> reminders)
        {
            tags = new List<KeyValuePairEntity<Guid>>();
            Pics = new List<KeyValuePairEntity<Guid>>();
            reminders = new List<ReminderEntity>();

            try
            {
                StringBuilder sb = new StringBuilder();
                if (gids.Count > 0)
                {
                    sb.Append(" with m as (");
                    for (int i = 0; i < gids.Count; i++)
                    {
                        sb.AppendFormat("select '{0}' ", gids[i]);
                        if (i == 0)
                        {
                            sb.Append(" as id ");
                        }
                        if (i < gids.Count - 1)
                        {
                            sb.Append(" union all ");
                        }
                    }
                    sb.Append(")");
                }

                StringBuilder sbsql = new StringBuilder();
                sbsql.Append(sb.ToString());
                sbsql.AppendFormat(@" SELECT t.* FROM [dbo].[M_User_EventItem] t  {0} WHERE t.CalendarTypeID<=5   and t.UserID={1} and t.EventItemState=1 {2} ", (gids.Count > 0 ? "left JOIN  m ON t.EventItemGUID=m.id" : ""), userid,
                    gids.Count > 0 ? " and m.id is null " : "" );

               
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    // List<EventItemEntity> items = con.Query<EventItemEntity, ReminderEntity, EventItemEntity>(sbsql.ToString(), (item, reminder) => { item.Reminder = reminder; return item; }, splitOn: "ReminderID").ToList();
                    List<EventItemEntity> items = con.Query<EventItemEntity>(sbsql.ToString()).ToList();
                    if (gids.Count > 0)
                    {
                        StringBuilder otherParms = new StringBuilder();
                        otherParms.Append(" with m as (");
                        for (int i = 0; i < items.Count; i++)
                        {
                            otherParms.AppendFormat("select '{0}' ", items[i].EventItemGUID);
                            if (i == 0)
                            {
                                otherParms.Append(" as id ");
                            }
                            if (i < items.Count - 1)
                            {
                                otherParms.Append(" union all ");
                            }
                        }
                        otherParms.Append(")");

                        if (items.Count != 0)
                        {
                            string remindersql = string.Format("{0} select r.* from M_Reminder r inner join m on r.EventItemGUID=m.id ", otherParms.ToString());
                            reminders = con.Query<ReminderEntity>(remindersql).ToList();
                            string tagsql = string.Format("{0} select g.EventItemGUID as T1,g.Name as V from M_Tags g inner join m on g.EventItemGUID=m.id ", otherParms.ToString());
                            tags = con.Query<KeyValuePairEntity<Guid>>(tagsql).ToList();
                            string picsql = string.Format("{0} select p.EventItemGUID as T1,p.PictureID as V from M_EventItem_Pictures p inner join m on p.EventItemGUID=m.id ", otherParms.ToString());
                            Pics = con.Query<KeyValuePairEntity<Guid>>(picsql).ToList();
                        }
                    }
                    else
                    {
                        string remindersql = string.Format("select r.* from M_Reminder r inner join M_User_EventItem m on r.EventItemGUID=m.EventItemGUID where m.UserID={0} ", userid);
                        reminders = con.Query<ReminderEntity>(remindersql).ToList();
                        string tagsql = string.Format(" select g.EventItemGUID as T1,g.Name as V from M_Tags g  inner join M_User_EventItem m on g.EventItemGUID=m.EventItemGUID where m.UserID={0} ", userid);
                        tags = con.Query<KeyValuePairEntity<Guid>>(tagsql).ToList();
                        string picsql = string.Format(" select p.EventItemGUID as T1,p.PictureID as V from M_EventItem_Pictures p inner join M_User_EventItem m  on p.EventItemGUID=m.EventItemGUID where m.UserID={0} ", userid);
                        Pics = con.Query<KeyValuePairEntity<Guid>>(picsql).ToList();
                    }

                    return items;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public bool TrySaveEventItems(List<EventItemEntity> items)
        {
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    DateTime nowDate = DateTime.Now;
                    DateTime maxDate = DateTime.Parse("4000-01-01 00:00:00");
                    foreach (var info in items)
                    {
                        //添加 或 更新 M_User_EventItem 实体
                        SqlParameter[] prms =
                        { 
                            new SqlParameter("@EventItemGUID",info.EventItemGUID),
                            new SqlParameter("@UserID",info.UserID),
                            new SqlParameter("@Title",info.Title),
                            new SqlParameter("@StartTime",info.StartTime==null ?maxDate:info.StartTime),
                            new SqlParameter("@EndTime",info.EndTime==null ? maxDate:info.EndTime),
                            new SqlParameter("@CreateTime",info.CreateTime ==null ? nowDate:info.CreateTime),
                            new SqlParameter("@EditTime",nowDate),
                            new SqlParameter("@CalendarTypeID",info.CalendarTypeID),
                            new SqlParameter("@RepeatTypeID",info.RepeatTypeID),
                            new SqlParameter("@StartYear",info.StartYear),
                            new SqlParameter("@StartMonth",info.StartMonth),
                            new SqlParameter("@StartDay",info.StartDay),
                            new SqlParameter("@StartWeek",info.StartWeek),
                            new SqlParameter("@StartHour",info.StartHour),
                            new SqlParameter("@StartMinutes",info.StartMinutes),
                            new SqlParameter("@Remark",info.Remark),
                            new SqlParameter("@Locate",info.Locate),
                            new SqlParameter("@EventItemState",Convert.ToInt32(info.EventItemState)),
                            new SqlParameter("@Finished",info.Finished),
                            new SqlParameter("@Tag",info.Tag),
                            new SqlParameter("@Longitude",info.Longitude),
                            new SqlParameter("@Latitude",info.Latitude),
                            new SqlParameter("@StartTimeNeedConvertLunar",info.StartTimeNeedConvertLunar),
                            new SqlParameter("@BitTags",info.BitTags),
                            new SqlParameter("@Version","1")
                        };

                        SqlParameter[] delUserItems =
                            { 
                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                new SqlParameter("@EditTime",nowDate)
                            };
                        SqlParameter[] delAllUserItems =
                            { 
                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                               
                            };
                        SqlParameter[] prms_tag ={
                                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                new SqlParameter("@Name",string.Empty)
                                                };
                        SqlParameter[] prms_pic ={
                                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                new SqlParameter("@PictureID",Convert.ToInt32(0))
                                                };

                        if (info.EventItemState == 0)  //判定是否需要删除
                        {
                            SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_Del", delUserItems);
                        }
                        else// 新增或者更新
                        {
                            //新增或者更新 M_User_EventItem 主表
                            if (SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_Update", prms) == 0)
                            {
                                SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_Insert", prms);
                            }

                            //清理相关表数据后,写入新的相关表数据
                            //步骤 1  清理
                            SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_DelRelation", delAllUserItems);

                            //步骤 2  插入提醒信息
                            foreach (var re_item in info.Reminders)
                            {
                                if (re_item.ReminderPreTime > -1)
                                {
                                    SqlParameter[] prms_reminder = { 
                                                        new SqlParameter("@ReminderGUID",re_item.EventItemGUID),
                                                        new SqlParameter("@UserID",info.UserID),
                                                        new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                        new SqlParameter("@ReminderPreTime",re_item.ReminderPreTime),
                                                        new SqlParameter("@ReminderState",Convert.ToInt32(re_item.ReminderState)),
                                                        new SqlParameter("@DeletedFlag",Convert.ToInt32(re_item.DeletedFlag))
                                                       };
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_Reminder_Insert", prms_reminder);
                                }
                            }

                            //步骤 3 插入tag
                            if (info.Tags.Count > 0)
                            {
                                foreach (var tag in info.Tags)
                                {
                                    prms_tag[1].Value = tag;
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_Tags_Insert", prms_tag);
                                }
                            }

                            //步骤 4  插入图片
                            if (info.Pics.Count > 0)
                            {
                                foreach (int pic in info.Pics)
                                {
                                    prms_pic[1].Value = pic;
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_EventItem_Pictures_Insert", prms_pic);
                                }
                            }
                        }
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return false;
                }
            }
        }

        public List<EventItemEntity> GetItemListByCalanderTypeID(int pageno, int pagesize, int typeid, int recommandflag, int zoneid)
        {
            try
            {
                int startno = (pageno - 1) * pagesize + 1;
                int endno = pageno * pagesize;
                string sql = string.Format(@" 
                            with a as (
                            select EventItemID,EventItemGUID,Title,Content,Remark,Url,isnull(Recommend,0) as Recommend ,ROW_NUMBER() OVER(ORDER BY CreateTime desc) AS rowid,pp.Domain,pp.PicturePath,t.CalendarTypeName from M_EventItem e inner join M_CalendarType t
                            on e.CalendarTypeID=t.CalendarTypeID left join M_V_Picture pp on pp.PictureID=e.PictureID
                            where e.EventItemState=1  {0} {1}  and ISNULL(e.PublishTime,GETDATE())<=GETDATE() and (PublishAreaID=0 or PublishAreaID={4}) )
                            select * from a where RowID between {2} and {3}
                            ", typeid > 0 ? " and e.CalendarTypeID= " + typeid.ToString() : " and t.CalendarTypeKind=2 ", recommandflag < 0 ? "" : string.Format(" and isnull(Recommend,0)={0}", recommandflag), startno, endno, zoneid);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<EventItemEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<EventItemEntity> QueryItemList(int pageno, int pagesize, string keyword)
        {
            try
            {
                int startno = (pageno - 1) * pagesize + 1;
                int endno = pageno * pagesize;
                string sql = string.Format(@" 
                            with a as (
                            select EventItemID,EventItemGUID,Title,Content, ROW_NUMBER() OVER(ORDER BY CreateTime desc) AS rowid,pp.Domain,pp.PicturePath,t.CalendarTypeName,e.Url from M_EventItem e inner join M_CalendarType t
                            on e.CalendarTypeID=t.CalendarTypeID left join M_V_Picture pp on pp.PictureID=e.PictureID
                            where e.EventItemState=1 and  t.CalendarTypeKind=2 and ISNULL(e.PublishTime,GETDATE())<=GETDATE() and (Title like '%{0}%' OR [Content] like '%{0}%'))
                            select * from a where RowID between {1} and {2}
                            ", keyword, startno, endno);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<EventItemEntity>(sql).ToList();
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<EventItemEntity> QueryFavorItemList(int pageno, int pagesize, int userid)
        {
            try
            {
                int startno = (pageno - 1) * pagesize + 1;
                int endno = pageno * pagesize;
                string sql = string.Format(@" 
                            with a as (
                            select EventItemID,e.EventItemGUID,Title,ROW_NUMBER() OVER(ORDER BY rr.FavorTime desc) AS rowid,pp.Domain,pp.PicturePath,e.Url from M_EventItem e inner join M_User_EventItem_Rel rr on rr.EventItemGUID=e.EventItemGUID  inner join M_CalendarType t
                            on e.CalendarTypeID=t.CalendarTypeID left join M_V_Picture pp on pp.PictureID=e.PictureID
                            where e.EventItemState=1 and  t.CalendarTypeKind=2 and ISNULL(e.PublishTime,GETDATE())<=GETDATE() and rr.UserID={0} )
                            select * from a where RowID between {1} and {2}
                            ", userid, startno, endno);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<EventItemEntity>(sql).ToList();
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public bool AddFavor(Guid itemguid, int userid)
        {
            try
            {
                string sql = "insert into M_User_EventItem_Rel(UserID,EventItemGUID,FavorTime) values (@UserID,@EventItemGUID,getdate())";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { UserID = userid, EventItemGUID = itemguid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

        }

        public bool AddFavorBatch(List<Guid> itemguid, int userid)
        {
            SqlTransaction trans = null;
            try
            {
                string sql = "insert into M_User_EventItem_Rel(UserID,EventItemGUID,FavorTime) values (@UserID,@EventItemGUID,getdate())";
                string sqlupdate = "update M_User_EventItem_Rel set FavorTime=getdate() where UserID=@UserID and EventItemGUID=@EventItemGUID";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (var g in itemguid)
                    {
                        if (con.Execute(sqlupdate, new { UserID = userid, EventItemGUID = g }, transaction: trans) == 0)
                        {
                            con.Execute(sql, new { UserID = userid, EventItemGUID = g }, transaction: trans);
                        }

                    }
                    trans.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }

        }

        public bool RemoveFavor(int userid, Guid itemguid)
        {
            try
            {
                string sql = "delete from M_User_EventItem_Rel where UserID=@UserID and EventItemGUID=@EventItemGUID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { UserID = userid, EventItemGUID = itemguid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool RemoveFavor(Guid itemguid)
        {
            try
            {
                string sql = "delete from M_User_EventItem_Rel where EventItemGUID=@EventItemGUID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { EventItemGUID = itemguid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool RemoveFavorBatch(int userid, List<Guid> itemguid)
        {
            SqlTransaction trans = null;
            try
            {
                string sql = "delete from M_User_EventItem_Rel where UserID=@UserID and EventItemGUID=@EventItemGUID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (var g in itemguid)
                    {
                        con.Execute(sql, new { UserID = userid, EventItemGUID = g }, transaction: trans);
                    }
                    trans.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool RemoveFavorBatch(List<Guid> itemguid)
        {
            SqlTransaction trans = null;
            try
            {
                string sql = "delete from M_User_EventItem_Rel where  EventItemGUID=@EventItemGUID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (var g in itemguid)
                    {
                        con.Execute(sql, new {  EventItemGUID = g }, transaction: trans);
                    }
                    trans.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool CheckFavorState(Guid itemguid, int userid)
        {
            try
            {
                string sql = "select count(1) from M_User_EventItem_Rel where EventItemGUID=@EventItemGUID and UserID=@UserID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<int>(sql, new { EventItemGUID = itemguid, UserID = userid }).FirstOrDefault() > 0;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public List<EventItemEntity> GetAds(bool ishome = false)
        {
            try
            {
                string sql = string.Format(@"select EventItemID,EventItemGUID,Content, e.Url, Title,ROW_NUMBER() OVER(ORDER BY CreateTime desc) AS rowid,pp.Domain,pp.PicturePath,t.CalendarTypeName,t.CalendarTypeID from M_EventItem e inner join M_CalendarType t
                            on e.CalendarTypeID=t.CalendarTypeID left join M_V_Picture pp on pp.PictureID=e.CarouselPictureID
                            where e.EventItemState=1 and  t.CalendarTypeKind=2 and ISNULL(e.PublishTime,GETDATE())<=GETDATE() and Advert&{0}={0} AND ((Advert&{0}={0} AND e.AdsEndTime>GETDATE()) OR (Advert&{0}={0} AND e.DiscoverAdsEndTime>GETDATE()))  order by AdvertOrder ", ishome ? 1 : 2);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<EventItemEntity>(sql).ToList();
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<EventItemEntity> QueryMyFavorItemByCalendarType(DateTime dt, int userid)
        {
            try
            {


                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    string sql = string.Empty;
                    if (userid > 0)
                    {
                        sql = string.Format(@"SELECT e.EventItemID,e.EventItemGUID,e.StartTime,e.EndTime,e.PublishTime, e.Title,e.Url,p.PicturePath,p.Domain,e.CalendarTypeID,t.CalendarTypeName 
FROM dbo.M_User_CalendarType_Rel r INNER JOIN [dbo].[M_EventItem] e ON r.CalendarTypeID=e.CalendarTypeID
INNER JOIN dbo.M_CalendarType t ON t.CalendarTypeID=e.CalendarTypeID
LEFT JOIN dbo.M_V_Picture p ON p.PictureID=e.PictureID
WHERE e.EventItemState=1 and  r.UserID=@UserID AND convert(varchar(10),ISNULL(e.PublishTime,e.StartTime),120)=convert(varchar(10),@PDate,120) and ISNULL(e.PublishTime,e.StartTime)<getdate() order by e.CalendarTypeID ");
                        return con.Query<EventItemEntity>(sql, new { UserID = userid, PDate = dt }).ToList();
                    }
                    else
                    {
                        sql = string.Format(@"SELECT e.EventItemID,e.EventItemGUID,e.StartTime,e.EndTime,e.PublishTime, e.Title,e.Url,p.PicturePath,p.Domain,e.CalendarTypeID,t.CalendarTypeName 
FROM [dbo].[M_EventItem] e  
INNER JOIN dbo.M_CalendarType t ON t.CalendarTypeID=e.CalendarTypeID
LEFT JOIN dbo.M_V_Picture p ON p.PictureID=e.PictureID
WHERE e.EventItemState=1 and  convert(varchar(10),ISNULL(e.PublishTime,e.StartTime),120)=convert(varchar(10),@PDate,120) and ISNULL(e.PublishTime,e.StartTime)<getdate() order by e.CalendarTypeID ");
                        return con.Query<EventItemEntity>(sql, new { PDate = dt }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }


        #region 2015-12-23  LM For更新同步函数
        /// <summary>
        ///  更新同步函数  服务端往客户端同步
        /// </summary>
        /// <param name="items">GUID与修改时间</param>
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        public List<UserEventItemEntity> EventItem_AsyncPull(List<EventItemAsyncPullRequest> items, int userid
                                     , out List<KeyValuePairEntity<Guid>> tags
                                     , out List<KeyValuePairEntity<Guid>> pics
                                     , out List<ReminderEntity> reminders)
        {
            tags = new List<KeyValuePairEntity<Guid>>();
            pics = new List<KeyValuePairEntity<Guid>>();
            reminders = new List<ReminderEntity>();
            List<UserEventItemEntity> entitys = new List<UserEventItemEntity>();

            //sqlserver datetime类型范围 1753-01-01  9999-12-31
            DateTime minDate=DateTime.Parse("1753-01-01 00:00:00");
            DateTime maxDate = DateTime.Parse("4000-01-01 00:00:00");
            try
            {
                StringBuilder sb = new StringBuilder();
                if (items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        sb.AppendFormat(" SELECT '{0}' ", items[i].EventItemGUID);
                        if (i == 0)
                        {
                            sb.Append(" AS id ");
                        }
                        sb.AppendFormat(",'{0}' ", items[i].EditTime == null ? minDate : items[i].EditTime);
                        if (i == 0)
                        {
                            sb.Append(" AS EditTime ");
                        }
                        if (i < items.Count - 1)
                        {
                            sb.Append(" UNION ALL  ");
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    var  parms= new { UserID = userid, StrWhere=string.Empty };
                    entitys = con.Query<UserEventItemEntity>("M_User_EventItem_AsyncPull"
                              , parms
                              , null, true, null, CommandType.StoredProcedure).ToList();
                    #region 过滤数据
                    List<UserEventItemEntity> tmpEntitys = new List<UserEventItemEntity>();
                    if (items.Count > 0)
                    {
                        foreach (var entity in entitys)
                        {
                            //时间忽略输出
                            if (entity.StartTime >= maxDate)
                            {
                                entity.StartTime = null;
                            }
                            if (entity.EndTime >= maxDate)
                            {
                                entity.EndTime = null;
                            }
                        }

                        for (int i = 0; i < items.Count; i++)
			            {
                            var trueEntity = entitys.Where(p => p.EventItemGUID == items[i].EventItemGUID && items[i].EditTime < p.EditTime).FirstOrDefault();
                            if (trueEntity != null)
                            {
                                if (trueEntity.EditTime !=null &&
                                      trueEntity.EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") != items[i].EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                                {
                                    if (trueEntity.EventItemState == 1)
                                    {
                                        trueEntity.EventItemState = 2;
                                    }
                                    tmpEntitys.Add(trueEntity);
                                }
                            }
			            }
                        var otherEntity = entitys.Where(x => x.EventItemState!= 0).ToList();
                        for (int j = 0; j < items.Count; j++)
                        {
                            otherEntity.Remove(entitys.Where(p => p.EventItemGUID == items[j].EventItemGUID).FirstOrDefault());
                        }
                        tmpEntitys.AddRange(otherEntity);
                        entitys = tmpEntitys;
                    }
                    else
                    {
                        foreach (var entity in entitys)
                        {
                            if (entity.StartTime >= maxDate)
                            {
                                entity.StartTime = null;
                            }
                            if (entity.EndTime >= maxDate)
                            {
                                entity.EndTime = null;
                            }
                            if (entity.EventItemState == 1)
                            {
                                tmpEntitys.Add(entity);
                            }
                        }
                        entitys = tmpEntitys;
                    }
                    #endregion 
                    StringBuilder tpr = new StringBuilder();
                    for (int i = 0; i < entitys.Count; i++) 
                    {
                        tpr.AppendFormat(" SELECT '{0}' ", entitys[i].EventItemGUID);
                        if (i == 0)
                        {
                            tpr.Append(" AS id ");
                        }
                        tpr.AppendFormat(",'{0}' ", entitys[i].EditTime == null ? minDate : entitys[i].EditTime);
                        if (i == 0)
                        {
                            tpr.Append(" AS EditTime ");
                        }
                        if (i < entitys.Count - 1)
                        {
                            tpr.Append(" UNION ALL  ");
                        }
                    }

                    var otherParms = new { UserID = userid, StrWhere = tpr.ToString() };
                    tags = con.Query<KeyValuePairEntity<Guid>>("M_User_EventItem_AsyncPullTags"
                              , otherParms
                              , null, true, null, CommandType.StoredProcedure).ToList();
                    
                    pics = con.Query<KeyValuePairEntity<Guid>>("M_User_EventItem_AsyncPullPics"
                              , otherParms
                              , null, true, null, CommandType.StoredProcedure).ToList();

                    reminders = con.Query<ReminderEntity>("M_User_EventItem_AsyncPullReminder"
                                , otherParms
                                , null, true, null, CommandType.StoredProcedure).ToList();
                    }
                return entitys;
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }


        /// <summary>
        ///  更新同步函数  客户端往服务端同步
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool EventItem_AsyncPush(List<UserEventItemEntity> items) 
        {
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    DateTime nowDate = DateTime.Now;
                    DateTime maxDate = DateTime.Parse("4000-01-01 00:00:00");
                    foreach (var info in items)
                    {
                        //添加 或 更新 M_User_EventItem 实体
                        SqlParameter[] prms =
                        { 
                            new SqlParameter("@EventItemGUID",info.EventItemGUID),
                            new SqlParameter("@UserID",info.UserID),
                            new SqlParameter("@Title",info.Title),
                            new SqlParameter("@StartTime",info.StartTime==null ?maxDate:info.StartTime),
                            new SqlParameter("@EndTime",info.EndTime==null ? maxDate:info.EndTime),
                            new SqlParameter("@CreateTime",info.CreateTime ==null ? nowDate:info.CreateTime),
                            new SqlParameter("@EditTime",info.EditTime==null ? nowDate:info.EditTime),
                            new SqlParameter("@CalendarTypeID",info.CalendarTypeID),
                            new SqlParameter("@RepeatTypeID",info.RepeatTypeID),
                            new SqlParameter("@StartYear",info.StartYear),
                            new SqlParameter("@StartMonth",info.StartMonth),
                            new SqlParameter("@StartDay",info.StartDay),
                            new SqlParameter("@StartWeek",info.StartWeek),
                            new SqlParameter("@StartHour",info.StartHour),
                            new SqlParameter("@StartMinutes",info.StartMinutes),
                            new SqlParameter("@Remark",info.Remark),
                            new SqlParameter("@Locate",info.Locate),
                            new SqlParameter("@EventItemState",Convert.ToInt32(info.EventItemState)),
                            new SqlParameter("@Finished",info.Finished),
                            new SqlParameter("@Tag",info.Tag),
                            new SqlParameter("@Longitude",info.Longitude),
                            new SqlParameter("@Latitude",info.Latitude),
                            new SqlParameter("@StartTimeNeedConvertLunar",info.StartTimeNeedConvertLunar),
                            new SqlParameter("@BitTags",info.BitTags),
                            new SqlParameter("@Version","2")
                        };

                        SqlParameter[] delUserItems =
                            { 
                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                new SqlParameter("@EditTime",info.EditTime==null ? maxDate:info.EditTime)
                            };
                         SqlParameter[] delAllUserItems =
                            { 
                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                               
                            };
                        SqlParameter[] prms_tag ={
                                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                new SqlParameter("@Name",string.Empty)
                                                };
                        SqlParameter[] prms_pic ={
                                                new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                new SqlParameter("@PictureID",Convert.ToInt32(0))
                                                };

                        if (info.EventItemState == 0)  //判定是否需要删除
                        {
                            SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_Del", delUserItems);
                        }
                        else// 新增或者更新
                        {
                            //新增或者更新 M_User_EventItem 主表
                            SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_InsertOrUpdate", prms);

                            //清理相关表数据后,写入新的相关表数据
                            //步骤 1  清理
                            SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_User_EventItem_DelRelation", delAllUserItems);

                            //步骤 2  插入提醒信息
                            foreach (var re_item in info.Reminders)
                            {
                                if (re_item.ReminderPreTime > -1)
                                {
                                    SqlParameter[] prms_reminder = { 
                                                        new SqlParameter("@ReminderGUID",re_item.EventItemGUID),
                                                        new SqlParameter("@UserID",info.UserID),
                                                        new SqlParameter("@EventItemGUID",info.EventItemGUID),
                                                        new SqlParameter("@ReminderPreTime",re_item.ReminderPreTime),
                                                        new SqlParameter("@ReminderState",Convert.ToInt32(re_item.ReminderState)),
                                                        new SqlParameter("@DeletedFlag",Convert.ToInt32(re_item.DeletedFlag))
                                                       };
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_Reminder_Insert", prms_reminder);
                                }
                            }

                            //步骤 3 插入tag
                            if (info.Tags.Count > 0)
                            {
                                foreach (var tag in info.Tags)
                                {
                                    prms_tag[1].Value = tag;
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_Tags_Insert", prms_tag);
                                }
                            }

                            //步骤 4  插入图片
                            if (info.Pics.Count > 0)
                            {
                                foreach (int pic in info.Pics)
                                {
                                    prms_pic[1].Value = pic;
                                    SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_EventItem_Pictures_Insert", prms_pic);
                                }
                            }
                        }
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return false;
                }
            }
          
        }
        #endregion

        #region 后台发布相关操作
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="searchEntity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryViewEventItemTable(EventItemSearchEntity searchEntity, out int totalcnt)
        {
            DataTable table = null;
            totalcnt = 0;

            try
            {
                SqlParameter[] prms = ParseToSqlParameters(searchEntity).ToArray();

                if (searchEntity.UseDBPagination)
                {
                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                    totalcnt = Convert.ToInt32(prms[prms.Length - 1].Value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
            return table;
        }


        public DataTable QueryViewEventItemTable(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        { 
            DataTable table = new DataTable();
            try
            {
                List<SqlParameter> prmslist = new List<SqlParameter>();
                prmslist.Add(new SqlParameter("@TableName", " M_V_EventItemPublish "));

                prmslist.Add(CPFields());

                prmslist.Add(new SqlParameter("@OrderField", orderType));

                prmslist.Add(new SqlParameter("@sqlWhere", strWhere));

                prmslist.Add(new SqlParameter("@pageIndex", pageIndex));

                prmslist.Add(new SqlParameter("@pageSize", pageSize));

                prmslist.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

                SqlParameter[] prms = prmslist.ToArray();

                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                recordCount = Convert.ToInt32(prms[prms.Length - 1].Value);
                return  table;
            }
            catch
            {
                recordCount = 0;
                return table;
            }
        }


        /// <summary>
        /// 根据IDs查询文章
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryViewEventItemTableByIDs(string ids, int pageIndex, int pageSize, out int totalcnt)
        {
            DataTable table = null;
            totalcnt = 0;

            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    List<SqlParameter> prmslist = new List<SqlParameter>();
                    prmslist.Add(new SqlParameter("@TableName", " M_V_EventItemPublish "));

                    string fields = @" EventItemID, EventItemGUID,Title, [Content], StartTime, EndTime,CreateTime,CalendarTypeID,
                                   SecondTypeName, FirstTypeID,FirstTypName, PublishAreaID, ZoneName,Html,CreateOpID, BrowseCnt,BookListPath,ActiveApply,
                                   EventItemState,Url,Recommend,CONVERT(VARCHAR(10), PublishTime,121) AS PublishTime,PictureID,Domain,PicturePath, Advert,AdvertOrder,DiscoverAdvert,CarouselPictureID,IsSingleGroupState ";
                    prmslist.Add(new SqlParameter("@Fields", fields));

                    prmslist.Add(new SqlParameter("@OrderField", "EventItemID DESC"));

                    List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    string[] querysql = idList.Select(id => " EventItemID=" + id).ToArray();
                    string strquerysql = string.Join(" OR ", querysql);
                    string wheresql = " EventItemState=1 AND " + "(" + strquerysql + ")";
                    prmslist.Add(new SqlParameter("@sqlWhere", wheresql));

                    prmslist.Add(new SqlParameter("@pageIndex", pageIndex));

                    prmslist.Add(new SqlParameter("@pageSize", pageSize));

                    prmslist.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

                    SqlParameter[] prms = prmslist.ToArray();

                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                    totalcnt = Convert.ToInt32(prms[prms.Length - 1].Value);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return table;
        }

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public DataTable QueryViewOnlyEventItemTable(int eventItemID)
        {
            DataTable table = null;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Select");
            sbSql.Append(" EventItemID, EventItemGUID,Title, [Content], StartTime, EndTime,CreateTime,CalendarTypeID,");
            sbSql.Append(" SecondTypeName, FirstTypeID,FirstTypName, PublishAreaID, ZoneName,Html,CreateOpID, BrowseCnt,");
            sbSql.Append(" EventItemState,Url,Recommend,CONVERT(VARCHAR(10), PublishTime,121) AS PublishTime,PictureID,Domain,PicturePath, Advert,AdvertOrder,DiscoverAdvert,AdsEndTime,DiscoverAdsEndTime");
            sbSql.Append(" FROM M_V_EventItemPublish");
            sbSql.Append(" Where EventItemState=1 AND EventItemID=@EventItemID");

            try
            {
                SqlParameter[] prms = {
                                          new SqlParameter("@EventItemID",eventItemID)
                                      };

                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sbSql.ToString(), prms).Tables[0];
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return table;
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public EventItemViewEntity QueryViewEntity(int eventItemID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Select");
            sbSql.Append(" EventItemID, EventItemGUID,Title, [Content], StartTime, EndTime,CreateTime,CalendarTypeID,");
            sbSql.Append(" SecondTypeName, FirstTypeID,FirstTypName, PublishAreaID, ZoneName,Html,CreateOpID, BrowseCnt,EventItemState,Url,");
            sbSql.Append(" Recommend,PublishTime,PictureID,Domain,PicturePath, Advert,AdvertOrder, ActivePlace, PublishSource,ThemePicturePath,BookListPath,");
            sbSql.Append(" Title2,StartTime2,EndTime2,CarouselPictureID,EventItemFlag, CarouselPicturePath, DiscoverAdvert,IsSingleGroupState,FestivalID,AdsEndTime,DiscoverAdsEndTime");
            sbSql.Append(" From M_V_EventItemPublish");
            sbSql.Append(" Where EventItemState=1 And EventItemID=@EventItemID");
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemViewEntity>(sbSql.ToString(), new { EventItemID = eventItemID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询视图实体(HTML5新闻页调用)
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public EventItemViewEntity QueryViewEntity(Guid eventItemGUID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Select");
            sbSql.Append(" EventItemGUID,Title, [Content],Convert(varchar(10),PublishTime,121) AS PublishTime,Html,BrowseCnt,PicturePath,");
            sbSql.Append(" FirstTypName, ActivePlace, PublishSource,ThemePicturePath, StartTime, EndTime, Domain, BookListPath");
            sbSql.Append(" From M_V_EventItemPublish");
            sbSql.Append(" Where EventItemState=1 And EventItemGUID=@EventItemGUID");
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemViewEntity>(sbSql.ToString(), new { EventItemGUID = eventItemGUID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断文章标题是否可用
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsUseableTitle(int eventItemID, string title)
        {
            bool isUseable = false;
            string sql = "";

            try
            {
                SqlParameter[] prms = {
                                          new SqlParameter("@EventItemID", eventItemID),
                                          new SqlParameter("@Title", title)
                                       };

                if (eventItemID > 0)
                {
                    sql = "Select count(EventItemGUID) From M_EventItem Where EventItemID != @EventItemID AND Title=@Title";

                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    int num = 0;
                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
                else
                {
                    sql = "Select count(EventItemGUID) From M_EventItem Where Title=@Title";

                    object result = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);

                    int num = 0;
                    if (int.TryParse(result.ToString(), out num) && num == 0)
                    {
                        isUseable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isUseable;
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
            bool isSuccess = false;
            SqlTransaction trans = null;

            #region EventItem表操作
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Insert Into M_EventItem(");
            sbSql.Append(" EventItemGUID,UserID,Title,Content,StartTime,EndTime,CalendarTypeID,EventItemState,Remark,Url,Recommend,PublishTime,PictureID,Advert,AdvertOrder,PublishAreaID,Html,CreateOpID,CreateTime,ThemePictureID, PublishSource, ActivePlace,BookListPath,Title2,StartTime2,EndTime2,CarouselPictureID,EventItemFlag,FestivalID,AdsEndTime,DiscoverAdsEndTime");
            sbSql.Append(" ) Values(");
            sbSql.Append(" @EventItemGUID,@UserID,@Title,@Content,@StartTime,@EndTime,@CalendarTypeID,@EventItemState,@Remark,@Url,@Recommend,@PublishTime,@PictureID,@Advert,@AdvertOrder,@PublishAreaID,@Html,@CreateOpID,@CreateTime,@ThemePictureID, @PublishSource, @ActivePlace,@BookListPath,@Title2,@StartTime2,@EndTime2,@CarouselPictureID,@EventItemFlag,@FestivalID,@AdsEndTime,@DiscoverAdsEndTime");
            sbSql.Append("  );");
            sbSql.Append(" SELECT @@IDENTITY");

            SqlParameter[] itemprms = {
                                          new SqlParameter("@EventItemGUID", SqlDbType.UniqueIdentifier,16),
                                          new SqlParameter("@UserID", SqlDbType.Int),
                                          new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                          new SqlParameter("@Content",SqlDbType.VarChar,150),
                                          new SqlParameter("@StartTime",SqlDbType.DateTime),
                                          new SqlParameter("@EndTime",SqlDbType.DateTime),                                         
                                          new SqlParameter("@CalendarTypeID",SqlDbType.Int),
                                          new SqlParameter("@EventItemState",SqlDbType.Int),
                                          new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                                          new SqlParameter("@Url",SqlDbType.NVarChar,300),
                                          new SqlParameter("@Recommend",SqlDbType.Int),
                                          new SqlParameter("@PublishTime",SqlDbType.DateTime),
                                          new SqlParameter("@PictureID",SqlDbType.Int),
                                          new SqlParameter("@Advert",SqlDbType.Int),
                                          new SqlParameter("@AdvertOrder",SqlDbType.Int),
                                          new SqlParameter("@PublishAreaID",SqlDbType.Int),
                                          new SqlParameter("@Html",SqlDbType.NText),
                                          new SqlParameter("@CreateOpID",SqlDbType.Int),
                                          new SqlParameter("@CreateTime",SqlDbType.DateTime),
                                          new SqlParameter("@ThemePictureID",SqlDbType.Int),
                                          new SqlParameter("@PublishSource", SqlDbType.NVarChar,100),
                                          new SqlParameter("@ActivePlace",SqlDbType.NVarChar,100),
                                          new SqlParameter("@BookListPath",SqlDbType.NVarChar,200),
                                          new SqlParameter("@Title2",SqlDbType.NVarChar,100),
                                          new SqlParameter("@StartTime2",SqlDbType.DateTime),
                                          new SqlParameter("@EndTime2",SqlDbType.DateTime),
                                          new SqlParameter("@CarouselPictureID",SqlDbType.Int),
                                          new SqlParameter("@EventItemFlag",SqlDbType.Int), 
                                          new SqlParameter("@FestivalID",SqlDbType.UniqueIdentifier),
                                          new SqlParameter("@AdsEndTime",SqlDbType.DateTime),
                                          new SqlParameter("@DiscoverAdsEndTime",SqlDbType.DateTime)
                                      };

            itemprms[0].Value = entity.EventItemGUID;
            itemprms[1].Value = entity.UserID;
            itemprms[2].Value = entity.Title;
            itemprms[3].Value = entity.Content;
            itemprms[4].Value = entity.StartTime;
            itemprms[5].Value = entity.EndTime;
            itemprms[6].Value = entity.CalendarTypeID;
            itemprms[7].Value = entity.EventItemState;
            itemprms[8].Value = entity.Remark;
            itemprms[9].Value = entity.Url;
            itemprms[10].Value = entity.Recommend;
            itemprms[11].Value = entity.PublishTime;
            itemprms[12].Value = entity.PictureID;
            itemprms[13].Value = entity.Advert;
            itemprms[14].Value = entity.AdvertOrder;
            itemprms[15].Value = entity.PublishAreaID;
            itemprms[16].Value = entity.Html;
            itemprms[17].Value = entity.CreateOpID;
            itemprms[18].Value = entity.CreateTime;
            itemprms[19].Value = entity.ThemePictureID;
            itemprms[20].Value = entity.PublishSource;
            itemprms[21].Value = entity.ActivePlace;
            itemprms[22].Value = entity.BookListPath;
            itemprms[23].Value = entity.Title2;
            itemprms[24].Value = entity.StartTime2;
            itemprms[25].Value = entity.EndTime2;
            itemprms[26].Value = entity.CarouselPictureID;
            itemprms[27].Value = entity.EventItemFlag;
            itemprms[28].Value = entity.FestivalID.ToString() != "00000000-0000-0000-0000-000000000000" ? (object)entity.FestivalID : DBNull.Value;
            itemprms[29].Value = entity.AdsEndTime;
            itemprms[30].Value = entity.DiscoverAdsEndTime;
            #endregion

            #region 图片表相关表操作
            string insertPicSql = "Insert Into M_Pictures(PictureServerID, PicturePath, PictureState) Values(@PictureServerID, @PicturePath, @PictureState);Select @@IDENTITY;";
            string insertItemPicSql = "Insert Into M_EventItem_Pictures(EventItemGUID,PictureID) Values(@EventItemGUID,@PictureID)";
            #endregion

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    #region 封面图片保存
                    if (converPicEntity != null)
                    {
                        SqlParameter[] picPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                        picPrms[0].Value = converPicEntity.PictureServerID;
                        picPrms[1].Value = converPicEntity.PicturePath;
                        picPrms[2].Value = converPicEntity.PictureState;
                        object picnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertPicSql, picPrms);
                        int pictureID = Convert.ToInt32(picnumobj);

                        SqlParameter[] itemPicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                        itemPicPrms[0].Value = entity.EventItemGUID;
                        itemPicPrms[1].Value = pictureID;
                        int itepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemPicSql, itemPicPrms);

                        itemprms[12].Value = pictureID;
                    }
                    #endregion

                    #region 主题图片保存
                    if (themePicEntity != null)
                    {
                        SqlParameter[] picthemePrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                        picthemePrms[0].Value = themePicEntity.PictureServerID;
                        picthemePrms[1].Value = themePicEntity.PicturePath;
                        picthemePrms[2].Value = themePicEntity.PictureState;

                        object themepicnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertPicSql, picthemePrms);
                        int themepictureID = Convert.ToInt32(themepicnumobj);

                        SqlParameter[] itemthemePicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                        itemthemePicPrms[0].Value = entity.EventItemGUID;
                        itemthemePicPrms[1].Value = themepictureID;
                        int itethemepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemPicSql, itemthemePicPrms);

                        itemprms[19].Value = themepictureID;
                    }

                    #endregion

                    #region 轮播图片保存
                    if (carouselPicEntity != null)
                    {
                        SqlParameter[] piccarouselPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                        piccarouselPrms[0].Value = carouselPicEntity.PictureServerID;
                        piccarouselPrms[1].Value = carouselPicEntity.PicturePath;
                        piccarouselPrms[2].Value = carouselPicEntity.PictureState;

                        object carouselpicnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertPicSql, piccarouselPrms);
                        int carouselpictureID = Convert.ToInt32(carouselpicnumobj);

                        SqlParameter[] itemcarouselPicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                        itemcarouselPicPrms[0].Value = entity.EventItemGUID;
                        itemcarouselPicPrms[1].Value = carouselpictureID;
                        int itethemepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemPicSql, itemcarouselPicPrms);

                        itemprms[26].Value = carouselpictureID;
                    }

                    #endregion

                    int itemID = Convert.ToInt32(SQlHelper.ExecuteScalar(trans, CommandType.Text, sbSql.ToString(), itemprms));

                    #region 保存单篇文章到专题分组
                    if (entity.Singlegroup == 1)
                    {
                        string insertGroupSQL = "INSERT INTO M_EventItemGroups(GroupEventName,PublishTime,CreatedTime,GroupState) VALUES (@GroupEventName,@PublishTime,@CreatedTime,@GroupState);SELECT @@IDENTITY;";
                        string insertRelSQL = "INSERT INTO M_EventItem_Group_Rel(EventGroupID,EventItemID,DisplayOrder) VALUES(@EventGroupID,@EventItemID,@DisplayOrder);";

                        SqlParameter[] groupprms = {
                                         new SqlParameter("@GroupEventName", SqlDbType.VarChar, 100),
                                         new SqlParameter("@PublishTime", SqlDbType.DateTime),
                                         new SqlParameter("@CreatedTime", SqlDbType.DateTime),
                                         new SqlParameter("@GroupState",SqlDbType.Int)
                                      };
                        groupprms[0].Value = entity.Title;
                        groupprms[1].Value = entity.PublishTime;
                        groupprms[2].Value = DateTime.Now;
                        groupprms[3].Value = 0;
                        int groupEventID = Convert.ToInt32(SQlHelper.ExecuteScalar(trans, CommandType.Text, insertGroupSQL, groupprms));

                        SqlParameter[] grouprelPrms = {
                                                      new SqlParameter("@EventGroupID", groupEventID),
                                                      new SqlParameter("@EventItemID",itemID),
                                                      new SqlParameter("@DisplayOrder",999)
                                                  };
                        int grouprelnum = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, insertRelSQL, grouprelPrms);
                    }
                    #endregion

                    isSuccess = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
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
            bool isSuccess = false;
            SqlTransaction trans = null;

            #region EventItem表操作
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Update M_EventItem");
            sbSql.Append(" Set");
            sbSql.Append(" EventItemGUID=@EventItemGUID,UserID=@UserID,Title=@Title,Content=@Content,StartTime=@StartTime,EndTime=@EndTime,CalendarTypeID=@CalendarTypeID,");
            sbSql.Append(" EventItemState=@EventItemState,Remark=@Remark,Url=@Url,Recommend=@Recommend,PublishTime=@PublishTime,PictureID=@PictureID,Advert=@Advert,AdvertOrder=@AdvertOrder,");
            sbSql.Append(" PublishAreaID=@PublishAreaID,Html=@Html,CreateOpID=@CreateOpID,CreateTime=@CreateTime,ThemePictureID=@ThemePictureID, PublishSource=@PublishSource, ActivePlace=@ActivePlace,");
            sbSql.Append(" BookListPath=@BookListPath,Title2=@Title2,StartTime2=@StartTime2,EndTime2=@EndTime2,CarouselPictureID=@CarouselPictureID,EventItemFlag=@EventItemFlag,FestivalID=@FestivalID,AdsEndTime=@AdsEndTime,DiscoverAdsEndTime=@DiscoverAdsEndTime");
            sbSql.Append(" Where EventItemID=@EventItemID");

            SqlParameter[] itemprms = {
                                          new SqlParameter("@EventItemGUID", SqlDbType.UniqueIdentifier,16),
                                          new SqlParameter("@UserID", SqlDbType.Int),
                                          new SqlParameter("@Title",SqlDbType.NVarChar,100),
                                          new SqlParameter("@Content",SqlDbType.VarChar,150),
                                          new SqlParameter("@StartTime",SqlDbType.DateTime),
                                          new SqlParameter("@EndTime",SqlDbType.DateTime),                                         
                                          new SqlParameter("@CalendarTypeID",SqlDbType.Int),
                                          new SqlParameter("@EventItemState",SqlDbType.Int),
                                          new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                                          new SqlParameter("@Url",SqlDbType.NVarChar,300),
                                          new SqlParameter("@Recommend",SqlDbType.Int),
                                          new SqlParameter("@PublishTime",SqlDbType.DateTime),
                                          new SqlParameter("@PictureID",SqlDbType.Int),
                                          new SqlParameter("@Advert",SqlDbType.Int),
                                          new SqlParameter("@AdvertOrder",SqlDbType.Int),
                                          new SqlParameter("@PublishAreaID",SqlDbType.Int),
                                          new SqlParameter("@Html",SqlDbType.NText),
                                          new SqlParameter("@CreateOpID",SqlDbType.Int),
                                          new SqlParameter("@CreateTime",SqlDbType.DateTime),
                                          new SqlParameter("@EventItemID",SqlDbType.Int),
                                          new SqlParameter("@ThemePictureID",SqlDbType.Int),
                                          new SqlParameter("@PublishSource", SqlDbType.NVarChar,100),
                                          new SqlParameter("@ActivePlace",SqlDbType.NVarChar,100),
                                          new SqlParameter("@BookListPath",SqlDbType.NVarChar,200),
                                          new SqlParameter("@Title2",SqlDbType.NVarChar,100),
                                          new SqlParameter("@StartTime2",SqlDbType.DateTime),
                                          new SqlParameter("@EndTime2",SqlDbType.DateTime),
                                          new SqlParameter("@CarouselPictureID",SqlDbType.Int),
                                          new SqlParameter("@EventItemFlag",SqlDbType.Int),
                                          new SqlParameter("@FestivalID",SqlDbType.UniqueIdentifier),
                                          new SqlParameter("@AdsEndTime",SqlDbType.DateTime),
                                          new SqlParameter("@DiscoverAdsEndTime",SqlDbType.DateTime)
                                      };

            itemprms[0].Value = entity.EventItemGUID;
            itemprms[1].Value = entity.UserID;
            itemprms[2].Value = entity.Title;
            itemprms[3].Value = entity.Content;
            itemprms[4].Value = entity.StartTime;
            itemprms[5].Value = entity.EndTime;
            itemprms[6].Value = entity.CalendarTypeID;
            itemprms[7].Value = entity.EventItemState;
            itemprms[8].Value = entity.Remark;
            itemprms[9].Value = entity.Url;
            itemprms[10].Value = entity.Recommend;
            itemprms[11].Value = entity.PublishTime;
            itemprms[12].Value = entity.PictureID;
            itemprms[13].Value = entity.Advert;
            itemprms[14].Value = entity.AdvertOrder;
            itemprms[15].Value = entity.PublishAreaID;
            itemprms[16].Value = entity.Html;
            itemprms[17].Value = entity.CreateOpID;
            itemprms[18].Value = entity.CreateTime;
            itemprms[19].Value = entity.EventItemID;
            itemprms[20].Value = entity.ThemePictureID;
            itemprms[21].Value = entity.PublishSource;
            itemprms[22].Value = entity.ActivePlace;
            itemprms[23].Value = entity.BookListPath;
            itemprms[24].Value = entity.Title2;
            itemprms[25].Value = entity.StartTime2;
            itemprms[26].Value = entity.EndTime2;
            itemprms[27].Value = entity.CarouselPictureID;
            itemprms[28].Value = entity.EventItemFlag;
            itemprms[29].Value = entity.FestivalID.ToString() != "00000000-0000-0000-0000-000000000000" ? (object)entity.FestivalID : DBNull.Value;
            itemprms[30].Value = entity.AdsEndTime;
            itemprms[31].Value = entity.DiscoverAdsEndTime;
            #endregion

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    #region 封面图片表相关表操作

                    if (pictureEntity != null)
                    {
                        if (entity.PictureID == 0)
                        {
                            #region 添加
                            string insertConverSql = "Insert Into M_Pictures(PictureServerID, PicturePath, PictureState) Values(@PictureServerID, @PicturePath, @PictureState);Select @@IDENTITY;";
                            string insertItemConverSql = "Insert Into M_EventItem_Pictures(EventItemGUID,PictureID) Values(@EventItemGUID,@PictureID)";


                            SqlParameter[] picConverPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                            picConverPrms[0].Value = pictureEntity.PictureServerID;
                            picConverPrms[1].Value = pictureEntity.PicturePath;
                            picConverPrms[2].Value = pictureEntity.PictureState;

                            object converpicnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertConverSql, picConverPrms);
                            int converpictureID = Convert.ToInt32(converpicnumobj);

                            SqlParameter[] itemthemePicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                            itemthemePicPrms[0].Value = entity.EventItemGUID;
                            itemthemePicPrms[1].Value = converpictureID;
                            int itethemepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemConverSql, itemthemePicPrms);

                            itemprms[12].Value = converpictureID;
                            #endregion
                        }
                        else if (entity.PictureID > 0)
                        {
                            #region 修改
                            SqlParameter[] picPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int),
                                         new SqlParameter("@PictureID", SqlDbType.Int)
                                      };
                            picPrms[0].Value = pictureEntity.PictureServerID;
                            picPrms[1].Value = pictureEntity.PicturePath;
                            picPrms[2].Value = pictureEntity.PictureState;
                            picPrms[3].Value = pictureEntity.PictureID;

                            string updatePicSql = "Update M_Pictures Set PictureServerID=@PictureServerID, PicturePath=@PicturePath, PictureState=@PictureState Where PictureID=@PictureID";
                            int picnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, updatePicSql, picPrms);
                            #endregion
                        }
                    }
                    #endregion

                    #region 主题图片操作 非必填项
                    if (themePicEntity != null)
                    {
                        if (entity.ThemePictureID == 0)
                        {
                            #region 添加
                            string insertPicSql = "Insert Into M_Pictures(PictureServerID, PicturePath, PictureState) Values(@PictureServerID, @PicturePath, @PictureState);Select @@IDENTITY;";
                            string insertItemPicSql = "Insert Into M_EventItem_Pictures(EventItemGUID,PictureID) Values(@EventItemGUID,@PictureID)";


                            SqlParameter[] picthemePrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                            picthemePrms[0].Value = themePicEntity.PictureServerID;
                            picthemePrms[1].Value = themePicEntity.PicturePath;
                            picthemePrms[2].Value = themePicEntity.PictureState;

                            object themepicnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertPicSql, picthemePrms);
                            int themepictureID = Convert.ToInt32(themepicnumobj);

                            SqlParameter[] itemthemePicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                            itemthemePicPrms[0].Value = entity.EventItemGUID;
                            itemthemePicPrms[1].Value = themepictureID;
                            int itethemepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemPicSql, itemthemePicPrms);

                            itemprms[20].Value = themepictureID;
                            #endregion
                        }
                        else if (entity.ThemePictureID > 0)
                        {
                            #region 修改
                            SqlParameter[] themepicPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int),
                                         new SqlParameter("@PictureID", SqlDbType.Int)
                                      };
                            themepicPrms[0].Value = themePicEntity.PictureServerID;
                            themepicPrms[1].Value = themePicEntity.PicturePath;
                            themepicPrms[2].Value = themePicEntity.PictureState;
                            themepicPrms[3].Value = themePicEntity.PictureID;

                            string updatethemePicSql = "Update M_Pictures Set PictureServerID=@PictureServerID, PicturePath=@PicturePath, PictureState=@PictureState Where PictureID=@PictureID";
                            int themepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, updatethemePicSql, themepicPrms);
                            #endregion
                        }
                    }

                    #endregion

                    #region 轮播图片操作 非必填项
                    if (carouselPicEntity != null)
                    {
                        if (entity.CarouselPictureID == 0)
                        {
                            #region 添加
                            string insertPicSql = "Insert Into M_Pictures(PictureServerID, PicturePath, PictureState) Values(@PictureServerID, @PicturePath, @PictureState);Select @@IDENTITY;";
                            string insertItemPicSql = "Insert Into M_EventItem_Pictures(EventItemGUID,PictureID) Values(@EventItemGUID,@PictureID)";

                            SqlParameter[] picCarouselPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int)
                                      };
                            picCarouselPrms[0].Value = carouselPicEntity.PictureServerID;
                            picCarouselPrms[1].Value = carouselPicEntity.PicturePath;
                            picCarouselPrms[2].Value = carouselPicEntity.PictureState;

                            object carouselpicnumobj = SQlHelper.ExecuteScalar(trans, CommandType.Text, insertPicSql, picCarouselPrms);
                            int carouselpictureID = Convert.ToInt32(carouselpicnumobj);

                            SqlParameter[] itemCarouselPicPrms = {
                                                     new SqlParameter("@EventItemGUID",SqlDbType.UniqueIdentifier,16),
                                                     new SqlParameter("@PictureID",SqlDbType.Int)
                                                 };
                            itemCarouselPicPrms[0].Value = entity.EventItemGUID;
                            itemCarouselPicPrms[1].Value = carouselpictureID;
                            int itecarouselpicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insertItemPicSql, itemCarouselPicPrms);

                            itemprms[27].Value = carouselpictureID;
                            #endregion
                        }
                        else if (entity.CarouselPictureID > 0)
                        {
                            #region 修改
                            SqlParameter[] carouselpicPrms = {
                                         new SqlParameter("@PictureServerID", SqlDbType.Int),
                                         new SqlParameter("@PicturePath", SqlDbType.NVarChar,200),
                                         new SqlParameter("@PictureState", SqlDbType.Int),
                                         new SqlParameter("@PictureID", SqlDbType.Int)
                                      };
                            carouselpicPrms[0].Value = carouselPicEntity.PictureServerID;
                            carouselpicPrms[1].Value = carouselPicEntity.PicturePath;
                            carouselpicPrms[2].Value = carouselPicEntity.PictureState;
                            carouselpicPrms[3].Value = carouselPicEntity.PictureID;

                            string updateCarouselPicSql = "Update M_Pictures Set PictureServerID=@PictureServerID, PicturePath=@PicturePath, PictureState=@PictureState Where PictureID=@PictureID";
                            int themepicnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, updateCarouselPicSql, carouselpicPrms);
                            #endregion
                        }
                    }

                    #endregion

                    #region 处理单篇成组
                    string selectGrouprelSQL = "WITH singGroup AS (SELECT rel.EventItemID,rel.EventGroupID FROM M_EventItemGroups gr INNER JOIN M_EventItem_Group_Rel rel ON rel.EventGroupID=gr.GroupEventID WHERE gr.GroupState = 0 AND rel.EventItemID=@EventItemID) SELECT sg.EventGroupID FROM M_EventItem item INNER JOIN singGroup sg ON sg.EventItemID=item.EventItemID";
                    SqlParameter[] selGroupPrms = {
                                                     new SqlParameter("@EventItemID", entity.EventItemID)
                                                  };
                    object groupObj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, selectGrouprelSQL, selGroupPrms);
                    int groupEventID = 0;


                    if (groupObj != null && int.TryParse(groupObj.ToString(), out groupEventID) && groupEventID > 0)
                    {
                        //此篇文章已设置为单篇成组

                        if (entity.Singlegroup == 0)
                        {
                            #region 删除单篇成组
                            string delGroupSQL = "DELETE FROM M_EventItemGroups WHERE GroupState=0 AND GroupEventID=@GroupEventID";
                            string delGroupRelSQL = "DELETE FROM M_EventItem_Group_Rel WHERE EventGroupID=@GroupEventID";
                            SqlParameter[] delGroupPrms = {
                                                      new SqlParameter("@GroupEventID", groupEventID)
                                                  };

                            int groupnum = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delGroupSQL, delGroupPrms);
                            int grouprelnum = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delGroupRelSQL, delGroupPrms);
                            #endregion
                        }
                        else
                        {
                            #region 修改单篇成组
                            string updateGroupSQL = "UPDATE M_EventItemGroups SET GroupEventName=@GroupEventName, PublishTime=@PublishTime WHERE GroupState=0 AND GroupEventID=@GroupEventID";
                            SqlParameter[] updateGroupPrms = {
                                                                 new SqlParameter("@GroupEventName",entity.Title),
                                                                 new SqlParameter("@PublishTime",entity.PublishTime),
                                                                 new SqlParameter("@GroupEventID",groupEventID)
                                                             };
                            int groupupdatenum = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateGroupSQL, updateGroupPrms);
                            #endregion
                        }
                    }
                    else
                    {
                        //此篇文章未设置为单篇成组

                        if (entity.Singlegroup == 1)
                        {
                            #region 新加单篇文章到专题分组
                            string insertGroupSQL = "INSERT INTO M_EventItemGroups(GroupEventName,PublishTime,CreatedTime,GroupState) VALUES (@GroupEventName,@PublishTime,@CreatedTime,@GroupState);SELECT @@IDENTITY;";
                            string insertRelSQL = "INSERT INTO M_EventItem_Group_Rel(EventGroupID,EventItemID,DisplayOrder) VALUES(@EventGroupID,@EventItemID,@DisplayOrder);";

                            SqlParameter[] groupprms = {
                                         new SqlParameter("@GroupEventName", SqlDbType.VarChar, 100),
                                         new SqlParameter("@PublishTime", SqlDbType.DateTime),
                                         new SqlParameter("@CreatedTime", SqlDbType.DateTime),
                                         new SqlParameter("@GroupState",SqlDbType.Int)
                                      };
                            groupprms[0].Value = entity.Title;
                            groupprms[1].Value = entity.PublishTime;
                            groupprms[2].Value = DateTime.Now;
                            groupprms[3].Value = 0;
                            int eventGroupID = Convert.ToInt32(SQlHelper.ExecuteScalar(trans, CommandType.Text, insertGroupSQL, groupprms));

                            SqlParameter[] grouprelPrms = {
                                                      new SqlParameter("@EventGroupID", eventGroupID),
                                                      new SqlParameter("@EventItemID",entity.EventItemID),
                                                      new SqlParameter("@DisplayOrder",999)
                                                  };
                            int grouprelnum = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, insertRelSQL, grouprelPrms);
                            #endregion
                        }
                    }

                    #endregion

                    int itemnum = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString(), itemprms);
                    isSuccess = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 设置推荐状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        public bool ChangeRecommendState(int eventItemID, int recommend)
        {
            bool isSuccess = false;
            string sql = "UPDATE M_EventItem SET Recommend=@Recommend WHERE EventItemID=@EventItemID";
            SqlParameter[] prms = {
                                      new SqlParameter("@Recommend",recommend),
                                      new SqlParameter("@EventItemID", eventItemID)
                                  };

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
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
            bool isSuccess = false;

            string selectSQL = "SELECT Advert,DiscoverAdvert FROM M_V_EventItemPublish  WHERE EventItemID=@EventItemID";
            string sql = "UPDATE M_EventItem SET Advert=@Advert,AdvertOrder=@AdvertOrder WHERE EventItemID=@EventItemID";          

            try
            {
                SqlParameter[] prms = {
                                      new SqlParameter("@Advert",advert),
                                      new SqlParameter("@AdvertOrder",advertOrder),
                                      new SqlParameter("@EventItemID",eventItemID)
                                  };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms).Tables[0];             
                EventItemPictureState state1 = int.Parse(table.Rows[0]["DiscoverAdvert"].ToString()) == 1 ? EventItemPictureState.DiscoverPicture : EventItemPictureState.None;
                EventItemPictureState state2 = advert == 1 ? EventItemPictureState.CoverPicture : EventItemPictureState.None;
                prms[0].Value = (int)state1 | (int)state2;

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 设置首页轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool ChangeAdvertState(int eventItemID, int advert, int advertOrder, DateTime dateTime)
        {
            bool isSuccess = false;

            string selectSQL = "SELECT Advert,DiscoverAdvert FROM M_V_EventItemPublish  WHERE EventItemID=@EventItemID";
            string sql = "UPDATE M_EventItem SET Advert=@Advert,AdvertOrder=@AdvertOrder,AdsEndTime=@AdsEndTime WHERE EventItemID=@EventItemID";

            try
            {
                SqlParameter[] prms = {
                                      new SqlParameter("@Advert",advert),
                                      new SqlParameter("@AdvertOrder",advertOrder),
                                      new SqlParameter("@EventItemID",eventItemID),
                                      new SqlParameter("@AdsEndTime",dateTime)
                                  };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms).Tables[0];
                EventItemPictureState state1 = int.Parse(table.Rows[0]["DiscoverAdvert"].ToString()) == 1 ? EventItemPictureState.DiscoverPicture : EventItemPictureState.None;
                EventItemPictureState state2 = advert == 1 ? EventItemPictureState.CoverPicture : EventItemPictureState.None;
                prms[0].Value = (int)state1 | (int)state2;

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 设置发现轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>      
        /// <returns></returns>
        public bool ChangeCarouselState(int eventItemID, int advert)
        {
            bool isSuccess = false;

            string selectSQL = "SELECT Advert,DiscoverAdvert FROM M_V_EventItemPublish  WHERE EventItemID=@EventItemID";
            string sql = "UPDATE M_EventItem SET Advert=@Advert WHERE EventItemID=@EventItemID";        

            try
            {              

                SqlParameter[] prms = {
                                      new SqlParameter("@Advert",advert),                                  
                                      new SqlParameter("@EventItemID",eventItemID)
                                  };             

              DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text,selectSQL,prms).Tables[0];                      
              EventItemPictureState state1 = int.Parse(table.Rows[0]["Advert"].ToString()) == 1 ? EventItemPictureState.CoverPicture : EventItemPictureState.None;

              EventItemPictureState state2 = advert == 2 ? EventItemPictureState.DiscoverPicture : EventItemPictureState.None;
               prms[0].Value = (int)state1 | (int)state2;
               isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;               
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        public bool ChangeCarouselState(int eventItemID, int advert, DateTime dateTime)
        {
            bool isSuccess = false;

            string selectSQL = "SELECT Advert,DiscoverAdvert FROM M_V_EventItemPublish  WHERE EventItemID=@EventItemID";
            string sql = "UPDATE M_EventItem SET Advert=@Advert,DiscoverAdsEndTime=@DiscoverAdsEndTime WHERE EventItemID=@EventItemID";

            try
            {

                SqlParameter[] prms = {
                                      new SqlParameter("@Advert",advert),
                                      new SqlParameter("@EventItemID",eventItemID),
                                      new SqlParameter("@DiscoverAdsEndTime",dateTime)
                                  };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms).Tables[0];
                EventItemPictureState state1 = int.Parse(table.Rows[0]["Advert"].ToString()) == 1 ? EventItemPictureState.CoverPicture : EventItemPictureState.None;

                EventItemPictureState state2 = advert == 2 ? EventItemPictureState.DiscoverPicture : EventItemPictureState.None;
                prms[0].Value = (int)state1 | (int)state2;
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 设置首页轮播状态
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advert"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool ChangeCarouselState(int eventItemID, int advert, int advertOrder, DateTime dateTime)
        {
            bool isSuccess = false;

            string selectSQL = "SELECT Advert,DiscoverAdvert FROM M_V_EventItemPublish  WHERE EventItemID=@EventItemID";
            string sql = "UPDATE M_EventItem SET Advert=@Advert,AdvertOrder=@AdvertOrder,DiscoverAdsEndTime=@DiscoverAdsEndTime WHERE EventItemID=@EventItemID";

            try
            {
                SqlParameter[] prms = {
                                      new SqlParameter("@Advert",advert),
                                      new SqlParameter("@AdvertOrder",advertOrder),
                                      new SqlParameter("@EventItemID",eventItemID),
                                      new SqlParameter("@DiscoverAdsEndTime",dateTime)
                                  };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, selectSQL, prms).Tables[0];
                EventItemPictureState state1 = int.Parse(table.Rows[0]["DiscoverAdvert"].ToString()) == 1 ? EventItemPictureState.DiscoverPicture : EventItemPictureState.None;
                EventItemPictureState state2 = advert == 1 ? EventItemPictureState.CoverPicture : EventItemPictureState.None;
                prms[0].Value = (int)state1 | (int)state2;

                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }
        public bool Delete(int eventItemID)
        {
            bool isSuccess = false;
            string sql = "Update M_EventItem Set EventItemState=0 WHERE EventItemID=@EventItemID";
            SqlParameter[] prms = {
                                      new SqlParameter("@EventItemID", eventItemID)
                                  };

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        public EventItemEntity QueryEntity(int eventItemID)
        {
            try
            {
                string sql = "Select * from M_EventItem Where EventItemID=@EventItemID";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<EventItemEntity>(sql, new { EventItemID = eventItemID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询已设置文章轮播的数量
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="canadvertnum">配置中一共可以设置轮播数量</param>
        /// <returns></returns>
        public bool IsCanSetAdvert(int eventItemID, int canadvertnum)
        {
            bool isCan = false;
            string sqltotal = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND Advert & 1 >0";

            try
            {
                object total = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqltotal);
                int totalnum = Convert.ToInt32(total);

                if (eventItemID > 0)
                {
                    string sqlnow = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND (Advert & 1 >0) AND EventItemID=@EventItemID";
                    SqlParameter[] prms = {
                                      new SqlParameter("@EventItemID", eventItemID)
                                   };
                    object now = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqlnow, prms);
                    int num = Convert.ToInt32(now);
                    if (num == 1)
                    {
                        isCan = true;
                    }
                    else if (totalnum < canadvertnum)
                    {
                        isCan = true;
                    }
                }
                else
                {
                    if (totalnum < canadvertnum)
                    {
                        isCan = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isCan;
        }

        /// <summary>
        /// 是否可以设置当前轮播序号
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="advertOrder"></param>
        /// <returns></returns>
        public bool IsCanSetOrderNum(int eventItemID, int advertOrder)
        {
            bool isCan = false;
            string sqltotal = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND Advert=1 AND AdvertOrder=@AdvertOrder";

            if (eventItemID > 0)
            {
                sqltotal = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND Advert=1 AND EventItemID!=@EventItemID AND AdvertOrder=@AdvertOrder";
            }

            SqlParameter[] prms = {
                  new SqlParameter("@AdvertOrder", advertOrder),
                  new SqlParameter("@EventItemID", eventItemID)
            };

            try
            {
                object total = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqltotal, prms);
                int totalnum = Convert.ToInt32(total);

                if (totalnum == 0)
                {
                    isCan = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isCan;
        }

        /// <summary>
        /// 删除主题图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteThemePicture(int eventItemID)
        {
            bool isSuccess = false;
            string selSql = "Select ThemePictureID From M_EventItem  Where EventItemID=@EventItemID";


            SqlParameter[] selPrms = {
                                         new SqlParameter("@EventItemID", eventItemID)
                                     };

            SqlTransaction trans = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    object num1 = SQlHelper.ExecuteScalar(trans, CommandType.Text, selSql, selPrms);

                    int themePicturedID = 0;
                    if (int.TryParse(num1.ToString(), out themePicturedID) && themePicturedID > 0)
                    {
                        string updateSql = "Update M_EventItem Set ThemePictureID=0 Where EventItemID=@EventItemID";
                        string delthemePic = "Delete From M_Pictures WHERE PictureID=@PictureID";
                        string delItemthemePic = "Delete From M_EventItem_Pictures WHERE PictureID=@PictureID";

                        SqlParameter[] themePrms = {
                                                        new SqlParameter("@PictureID", themePicturedID)
                                                    };

                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSql, selPrms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delthemePic, themePrms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delItemthemePic, themePrms);
                    }

                    trans.Commit();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除发现图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteCarouselPicture(int eventItemID)
        {
            bool isSuccess = false;
            string selSql = "Select CarouselPictureID From M_EventItem  Where EventItemID=@EventItemID";


            SqlParameter[] selPrms = {
                                         new SqlParameter("@EventItemID", eventItemID)
                                     };

            SqlTransaction trans = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    object num1 = SQlHelper.ExecuteScalar(trans, CommandType.Text, selSql, selPrms);

                    int carouselPicturedID = 0;
                    if (int.TryParse(num1.ToString(), out carouselPicturedID) && carouselPicturedID > 0)
                    {
                        string updateSql = "Update M_EventItem Set CarouselPictureID=0 Where EventItemID=@EventItemID";
                        string delthemePic = "Delete From M_Pictures WHERE PictureID=@PictureID";
                        string delItemthemePic = "Delete From M_EventItem_Pictures WHERE PictureID=@PictureID";

                        SqlParameter[] carouselPrms = {
                                                        new SqlParameter("@PictureID", carouselPicturedID)
                                                    };

                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSql, selPrms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delthemePic, carouselPrms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delItemthemePic, carouselPrms);
                    }

                    trans.Commit();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除封面图片
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteConverPicture(int eventItemID)
        {
            bool isSuccess = false;
            string selSql = "Select PictureID From M_EventItem  Where EventItemID=@EventItemID";


            SqlParameter[] selPrms = {
                                         new SqlParameter("@EventItemID", eventItemID)
                                     };

            SqlTransaction trans = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    object num1 = SQlHelper.ExecuteScalar(trans, CommandType.Text, selSql, selPrms);

                    int carouselPicturedID = 0;
                    if (int.TryParse(num1.ToString(), out carouselPicturedID) && carouselPicturedID > 0)
                    {
                        string updateSql = "Update M_EventItem Set PictureID=0 Where EventItemID=@EventItemID";
                        string delPic = "Delete From M_Pictures WHERE PictureID=@PictureID";
                        string delItemPic = "Delete From M_EventItem_Pictures WHERE PictureID=@PictureID";

                        SqlParameter[] Prms = {
                                                        new SqlParameter("@PictureID", carouselPicturedID)
                                                    };

                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSql, selPrms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delPic, Prms);
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, delItemPic, Prms);
                    }

                    trans.Commit();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除书单文件路径
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns></returns>
        public bool DeleteBookListPath(int eventItemID)
        {
            bool isSuccess = false;
            string updateSql = "UPDATE M_EventItem SET BookListPath=''  Where EventItemID=@EventItemID";
            SqlParameter[] prms = {
                                         new SqlParameter("@EventItemID", eventItemID)
                                     };

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSql, prms) >= 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 设置活动报名开关
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetActiveApply(int eventItemID, bool state)
        {
            bool isSuccess = false;
            string sql = "Update M_EventItem SET ActiveApply=@ActiveApply WHERE EventItemID=@EventItemID";
            SqlParameter[] prms = {
                                      new SqlParameter("@ActiveApply",SqlDbType.Bit),
                                      new SqlParameter("@EventItemID",SqlDbType.Int,4)
                                  };
            prms[0].Value = state;
            prms[1].Value = eventItemID;

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 查询已设置文章发现轮播的数量
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <param name="canadvertnum">配置中一共可以设置轮播数量</param>
        /// <returns></returns>
        public bool IsCanSetCarousel(int eventItemID, int cancarouselnum)
        {
            bool isCan = false;
            string sqltotal = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND Advert & 2 >0";

            try
            {
                object total = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqltotal);
                int totalnum = Convert.ToInt32(total);

                if (eventItemID > 0)
                {
                    string sqlnow = "Select count(Advert) AS Advert From M_EventItem WHERE EventItemState=1 AND (Advert & 2 > 0) AND EventItemID=@EventItemID";
                    SqlParameter[] prms = {
                                      new SqlParameter("@EventItemID", eventItemID)
                                   };
                    object now = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqlnow, prms);
                    int num = Convert.ToInt32(now);
                    if (num == 1)
                    {
                        isCan = true;
                    }
                    else if (totalnum < cancarouselnum)
                    {
                        isCan = true;
                    }
                }
                else
                {
                    if (totalnum < cancarouselnum)
                    {
                        isCan = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isCan;
        }

        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(EventItemSearchEntity entity)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            //table
            paraList.Add(CPTable(entity));

            //fields
            paraList.Add(CPFields(entity));

            //filter_SqlWhere
            paraList.Add(CPWhere(entity));

            //order
            paraList.Add(CPOrder(entity));

            //pagesize
            paraList.Add(new SqlParameter("@pageSize", entity.PageSize));

            //pageindex
            paraList.Add(new SqlParameter("@pageIndex", entity.PageIndex));

            paraList.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

            return paraList;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPWhere(EventItemSearchEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder(" EventItemState=1 ");

            if (!string.IsNullOrEmpty(entity.Title))
            {
                sbwhere.Append(" AND Title like '%" + entity.Title + "%'");
            }

            if (entity.SubarticleState == 0)
            {
                sbwhere.Append(" AND (CalendarTypeID != 0 OR FirstTypeID !=0) ");
            }
            else if (entity.SubarticleState == 1)
            {
                sbwhere.Append(" AND CalendarTypeID = 0 AND FirstTypeID =0 ");
            }
            else
            {
                if (entity.SecondTypeID > -1)
                {
                    sbwhere.Append(" AND CalendarTypeID = " + entity.SecondTypeID + "");
                }

                if (entity.FirstTypeID > -1)
                {
                    sbwhere.Append(" AND FirstTypeID = " + entity.FirstTypeID + "");
                }
            }

            if (entity.StartTime.HasValue)
            {
                sbwhere.Append(" AND StartTime >= '" + entity.StartTime.Value.Date + "'");
            }

            if (entity.EndTime.HasValue)
            {
                sbwhere.Append(" AND EndTime <= '" + entity.EndTime.Value.AddDays(1).Date.AddSeconds(-1) + "' ");
            }

            if (entity.DayTime.HasValue)
            {
                string endt = "";
                if (entity.Advert >= 0)
                {
                    endt = " AND AdsEndTime>='" + entity.DayTime.Value.ToString("yyyy-MM-dd 00:00:00") + "' ";
                } else if (entity.DiscoverAdvert >= 0) {
                    endt = " AND DiscoverAdsEndTime>='" + entity.DayTime.Value.ToString("yyyy-MM-dd 00:00:00") + "' ";
                }
                sbwhere.Append(" AND (PublishTime <= '" + entity.DayTime.Value.ToString("yyyy-MM-dd 23:59:59") + "' "+ endt + ")");
            }

            if (entity.CreateOpID > 0)
            {
                sbwhere.Append(" AND CreateOpID=" + entity.CreateOpID + "");
            }

            if (entity.Recommend >= 0)
            {
                sbwhere.Append(" AND Recommend=" + entity.Recommend + "");
            }

            if (entity.Advert >= 0)
            {
                sbwhere.Append(" AND Advert= " + entity.Advert + "");
            }

            if (entity.DiscoverAdvert >= 0)
            {
                sbwhere.Append(" AND DiscoverAdvert=" + entity.DiscoverAdvert);
            }

            if (entity.SingleGroup >= 0)
            {
                sbwhere.Append(" AND IsSingleGroupState=" + entity.SingleGroup);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(EventItemSearchEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.IsEnableAdvertOrder)
            {
                sborder.Append(" AdvertOrder ASC ");
                if (enity.OrderfieldType == OrderFieldType.Desc)
                {
                    sborder.Append(" ,EventItemID DESC ");
                }
                else
                {
                    sborder.Append(" ,EventItemID ASC ");
                }
            }
            else
            {
                if (enity.OrderfieldType == OrderFieldType.Desc)
                {
                    sborder.Append(" EventItemID DESC ");
                }
                else
                {
                    sborder.Append(" EventItemID ASC ");
                }
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }

        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(EventItemSearchEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" EventItemID, EventItemGUID,Title, [Content], StartTime, EndTime,CreateTime,CalendarTypeID,
                                   SecondTypeName, FirstTypeID,FirstTypName, PublishAreaID, ZoneName,Html,CreateOpID, BrowseCnt,BookListPath,ActiveApply,
                                   EventItemState,Url,Recommend,CONVERT(VARCHAR(30), PublishTime,20) AS PublishTime,PictureID,Domain,PicturePath, Advert,AdvertOrder,DiscoverAdvert,CarouselPictureID,IsSingleGroupState
                                    ,LikeCnt,CommentCnt,CONVERT(VARCHAR(30), AdsEndTime,20) AS AdsEndTime,CONVERT(VARCHAR(30), DiscoverAdsEndTime,20) AS DiscoverAdsEndTime");
            }
            else
            {
                throw new NotImplementedException();
            }
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(bool useDBPagination=true)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (useDBPagination)
            {
                sbfileds.Append(@" EventItemID, EventItemGUID,Title, [Content], StartTime, EndTime,CreateTime,CalendarTypeID,
                                   SecondTypeName, FirstTypeID,FirstTypName, PublishAreaID, ZoneName,Html,CreateOpID, BrowseCnt,BookListPath,ActiveApply,
                                   EventItemState,Url,Recommend,CONVERT(VARCHAR(30), PublishTime,20) AS PublishTime,PictureID,Domain,PicturePath, Advert,AdvertOrder,DiscoverAdvert,CarouselPictureID,IsSingleGroupState
                                    ,LikeCnt,CommentCnt,CONVERT(VARCHAR(30), AdsEndTime,20) AS AdsEndTime,CONVERT(VARCHAR(30), DiscoverAdsEndTime,20) AS DiscoverAdsEndTime");
            }
            else
            {
                throw new NotImplementedException();
            }
            return new SqlParameter("@Fields", sbfileds.ToString());
        }


        /// <summary>
        /// 设置表关联 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPTable(EventItemSearchEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(" M_V_EventItemPublish ");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
        #endregion
    }
}
