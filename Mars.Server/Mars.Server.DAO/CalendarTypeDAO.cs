using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Mars.Server.DAO
{
    public class CalendarTypeDAO
    {
        public List<CalendarTypeEntity> QueryCalendarTypes(int pageno,int pagesize,int parentid,int ctype,int userid=0)
        {
            try
            {
                int startno = (pageno - 1) * pagesize + 1;
                int endno = pageno * pagesize;
                string sql = string.Format(@" {7}  a as ( select t.*,s.Domain,p.PicturePath,ROW_NUMBER() OVER(ORDER BY t.CalendarTypeID ) AS rowid {6} from M_CalendarType t left join M_Pictures p on p.PictureID=t.PictureID
                left join M_PictureServer s on s.PictureServerID=p.PictureServerID {4}
                where CalendarTypeKind={0} and isnull(ParentCalendarTypeID,0)={1} {5} ) select * from a where rowid between {2} and {3}  ", ctype, parentid,startno,endno,
                                                                                                                                         (userid==0 ? "" : " left join  ff on ff.CalendarTypeID=t.CalendarTypeID "),
                                                                                                                                         (userid==0 ? "" : string.Format(" and  isnull(ff.UserID,{0})={0}",userid)),
                                                                                                                                             (userid == 0 ? "" : " , isnull(ff.CalendarTypeID,0) as  IsFavor "),
                                                                                                                                             (userid == 0 ? "with " : " with ff as ( select * from M_User_CalendarType_Rel where UserID="+userid.ToString()+" ) ,")
                                                                                                                                         );

                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<CalendarTypeEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<CalendarTypeEntity> QueryCalendarTypes(string key,int userid=0)
        {
            try
            {
                string sql = string.Format(@"SELECT t.*,s.Domain,p.PicturePath {3} FROM dbo.M_CalendarType t
left JOIN dbo.M_Pictures p ON p.PictureID = t.PictureID
left JOIN dbo.M_PictureServer s ON s.PictureServerID = p.PictureServerID {1}
 WHERE CalendarTypeKind = 2 AND ParentCalendarTypeID> 0
AND CalendarTypeName LIKE '%{0}%' {2} ", key,
                                         (userid == 0 ? "" : " left join M_User_CalendarType_Rel ff on ff.CalendarTypeID=t.CalendarTypeID "),
                                           (userid == 0 ? "" : string.Format(" and  isnull(ff.UserID,{0})={0}", userid)),
                                        (userid == 0 ? "" : " , isnull(ff.CalendarTypeID,0) as  IsFavor ")
                                       );
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<CalendarTypeEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public bool AddFavor(int ctypeid, int userid)
        {
            try
            {
                string sql = "insert into M_User_CalendarType_Rel(UserID,CalendarTypeID,FavorTime) values (@UserID,@CalendarTypeID,getdate())";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { UserID = userid, CalendarTypeID = ctypeid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }
        public bool RemoveFavor(int ctypeid, int userid)
        {
            try
            {
                string sql = "delete from M_User_CalendarType_Rel where UserID=@UserID and CalendarTypeID=@CalendarTypeID ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { UserID = userid, CalendarTypeID = ctypeid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public List<CalendarTypeEntity> QueryFavorCalendarTypes(int pageno, int pagesize, int userid)
        {
            try
            {
                int startno = (pageno - 1) * pagesize + 1;
                int endno = pageno * pagesize;
                string sql = string.Format(@" with a as ( select t.*,s.Domain,p.PicturePath,ROW_NUMBER() OVER(ORDER BY rr.FavorTime desc ) AS rowid from M_CalendarType t inner join M_User_CalendarType_Rel rr on rr.CalendarTypeID=t.CalendarTypeID left join M_Pictures p on p.PictureID=t.PictureID
                left join M_PictureServer s on s.PictureServerID=p.PictureServerID
                where CalendarTypeKind=2 and rr.UserID={0} ) select * from a where rowid between {1} and {2}  ", userid, startno, endno);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<CalendarTypeEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询一级分类
        /// </summary>      
        /// <param name="calendarTypeKind"></param>
        /// <returns></returns>
        public List<CalendarTypeEntity> QueryFirstCalendarType(int calendarTypeKind=2)
        {
            string sql = "Select CalendarTypeID, CalendarTypeName From M_CalendarType Where CalendarTypeKind=@CalendarTypeKind  And ParentCalendarTypeID = -1";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<CalendarTypeEntity>(sql, new { CalendarTypeKind = calendarTypeKind }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询一级分类下的二级分类
        /// </summary>
        /// <param name="parentCalendarTypeID"></param>
        /// <param name="calendarTypeKind"></param>
        /// <returns></returns>
        public List<CalendarTypeEntity> QuerySecondCalendarType(int parentCalendarTypeID, int calendarTypeKind=2)
        {
            string sql = "Select CalendarTypeID, CalendarTypeName From M_CalendarType Where CalendarTypeKind=@CalendarTypeKind AND ParentCalendarTypeID=@ParentCalendarTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<CalendarTypeEntity>(sql, new { CalendarTypeKind = calendarTypeKind, ParentCalendarTypeID = parentCalendarTypeID }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }
    }
}
