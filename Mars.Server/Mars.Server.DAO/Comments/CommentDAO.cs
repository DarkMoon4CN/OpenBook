using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Mars.Server.Entity;

namespace Mars.Server.DAO.Comments
{
    public class CommentDAO
    {
        /// <summary>
        /// 评论用户点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int ILikeThis(int id, int uid)
        {
            int returnValue = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"IF NOT EXISTS(SELECT CLID FROM dbo.M_EventItem_Comments_Like_Rel WHERE CommentID={0} AND UserID={1})
                                BEGIN
                                INSERT INTO dbo.M_EventItem_Comments_Like_Rel( CommentID ,LikeCnt ,UserID ,LikeTime)
                                VALUES  
                                ( {0},1,{1},GETDATE());
                                SELECT @@IDENTITY;
                                END
                                ELSE
                                SELECT -2"
                , id.ToString()
                , uid.ToString());

            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                con.Open();
                trans = con.BeginTransaction();

                try
                {
                    object obj = SQlHelper.ExecuteScalar(trans, CommandType.Text, strSql.ToString(), null);
                    if (obj != null)
                    {
                        returnValue = obj.ToInt();
                    }

                    if (returnValue>0)
                    {
                        int toUserId = 0;
                        StringBuilder strUser = new StringBuilder();
                        strUser.AppendFormat(@"SELECT UserID FROM dbo.M_EventItem_Comments WHERE CommentID = {0}"
                            , id.ToString());

                        object objToUser = SQlHelper.ExecuteScalar(trans, CommandType.Text, strUser.ToString(), null);
                        if (objToUser != null)
                        {
                            toUserId = objToUser.ToInt();
                        }

                        SqlParameter[] parameters = {
                            new SqlParameter("@MessageType", SqlDbType.Int),
                            new SqlParameter("@Contents", SqlDbType.VarChar,1024),
                            new SqlParameter("@Url", SqlDbType.VarChar,128),
                            new SqlParameter("@Title", SqlDbType.VarChar,128),
                            new SqlParameter("@IsRead", SqlDbType.Bit),
                            new SqlParameter("@ToUserID", SqlDbType.Int),
                            new SqlParameter("@FromUserID", SqlDbType.Int),
                            new SqlParameter("@Tag", SqlDbType.VarChar,128),
                            new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                            parameters[0].Value = 1;
                            parameters[1].Value = "赞了您的评论";
                            parameters[2].Value = "";
                            parameters[3].Value = "";
                            parameters[4].Value = false;
                            parameters[5].Value = toUserId;
                            parameters[6].Value = uid;
                            parameters[7].Value = id.ToString();
                            parameters[8].Value = DateTime.Now;
                            
                        SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_MobileMessages_Insert", parameters);
                    }
                        
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    returnValue = 0;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            return returnValue;
        }

        public bool UpdateNewWordState(Dictionary<int, string> wordList)
        {
            bool returnValue = false;
            if (wordList != null)
            {
                string ids = "";
                foreach (int _id in wordList.Keys)
                {
                    ids += _id + ",";
                }
                ids = ids.TrimEnd(',');

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"UPDATE dbo.M_System_SensitiveWords SET IsNeedRecheck=0 WHERE SWID IN ({0})"
                    , ids);

                try
                {
                    if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0)
                    {
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
            return returnValue;
        }

        public bool DeleteComment(int cid, int userid)
        {
            bool returnValue = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"IF EXISTS (SELECT CommentID FROM dbo.M_EventItem_Comments WHERE CommentID={0} AND UserID={1})
                            BEGIN
                            DELETE FROM dbo.M_EventItem_Replies WHERE CommentID = {0};
                            DELETE FROM dbo.M_EventItem_Comments_Like_Rel WHERE CommentID= {0};
                            DELETE FROM dbo.M_EventItem_Comments WHERE CommentID = {0};
                            END;"
                , cid.ToString()
                ,userid.ToString());

            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                con.Open();
                trans = con.BeginTransaction();

                try
                {
                    returnValue = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql.ToString(), null) > 0;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    trans.Rollback();
                }
                finally {
                    trans.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }

            return returnValue;
        }

        public bool UpdateCheckType()
        {
            bool returnValue = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE c SET c.CheckTypeID=cw.CheckTypeID,
                        c.ViewStateID = CASE WHEN cw.CheckTypeID=-1 THEN 0 ELSE c.ViewStateID END,
                        c.WordsInfo=cw.WordsInfo
                        FROM [M_EventItem_Comments] AS c
                        INNER JOIN M_Calc_WaitToUpdateCheckType AS cw ON cw.ID=c.CommentID AND cw.ContentType=1;
                        with tab as
                        (
                         select ReplyID,ReplyParentID,cw.CheckTypeID,cw.WordsInfo from dbo.M_EventItem_Replies AS r -- AND ReplyParentID=176--父节点
                         INNER JOIN M_Calc_WaitToUpdateCheckType AS cw ON cw.ID=r.ReplyID AND cw.ContentType=2
                         union all
                         select b.ReplyID,b.ReplyParentID,a.CheckTypeID,a.WordsInfo
                         from
                          tab a,
                          M_EventItem_Replies b
                         where b.ReplyParentID=a.ReplyID
                        )
                        UPDATE r SET r.CheckTypeID=cw.CheckTypeID,
						r.ViewStateID = CASE WHEN cw.CheckTypeID=-1 THEN 0 ELSE r.ViewStateID END,
						r.WordsInfo=cw.WordsInfo
                        FROM dbo.M_EventItem_Replies AS r
                        INNER JOIN tab AS cw ON cw.ReplyID=r.ReplyID;
                        TRUNCATE TABLE M_Calc_WaitToUpdateCheckType;");
            try
            {
                returnValue = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                returnValue = false;
            }

            return returnValue;
        }

        public bool WriteBack(DataTable returnTable)
        {
            bool returnValue = false;
            try
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(SQlHelper.MyConnectStr))
                {
                    copy.DestinationTableName = "M_Calc_WaitToUpdateCheckType";
                    copy.WriteToServer(returnTable);
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                returnValue = false;
            }

            return returnValue;
        }

        public SqlDataReader GetWillCheckCommentReader(bool isAll=false)
        {
            StringBuilder strSql = new StringBuilder();
            if (isAll)
            {
                strSql.Append(@" SELECT CommentID AS ID, CommentContent AS Content, 1 AS ContentType FROM dbo.M_EventItem_Comments WITH(NOLOCK) WHERE CheckTypeID >-1
                    UNION ALL
                    SELECT ReplyID AS ID, ReplyContent AS Content, 2 AS ContentType FROM dbo.M_EventItem_Replies WITH(NOLOCK) WHERE CheckTypeID > -1");
            }
            else
            { 
                strSql.Append(@" SELECT CommentID AS ID, CommentContent AS Content, 1 AS ContentType FROM dbo.M_EventItem_Comments WITH(NOLOCK) WHERE CheckTypeID = 0
                    UNION ALL
                    SELECT ReplyID AS ID, ReplyContent AS Content, 2 AS ContentType FROM dbo.M_EventItem_Replies WITH(NOLOCK) WHERE CheckTypeID = 0");
            }
            return SQlHelper.ExecuteReader(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString());
        }
    }
}
