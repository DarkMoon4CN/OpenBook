using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
namespace Mars.Server.DAO
{
    /*
     * 模块：文章相关
     * 作用：提供文章列表拉取,评论操作
     * 作者：
     * 时间：2015-01-05
     * 备注：ArticleDAO 将引用OperationResult.cs 作为返回数据结果承载体
     * 备注: 代码中遇到 Reviews 将同译于  Comments 
     */
    public class ArticleDAO
    {
        /// <summary>
        ///  添加文章评论
        /// </summary>
        /// <returns></returns>
        public OperationResult<ReviewCommonEntity> ArticleComments_Insert(EventItemCommentEntity entity)
        {
            try
            {
                ReviewCommonEntity rcEntity=new ReviewCommonEntity();
                SqlParameter[] prms = {
                                      new SqlParameter("@EventItemID",entity.EventItemID),
                                      new SqlParameter("@UserID",entity.UserID),
                                      new SqlParameter("@IsAnonymous",entity.IsAnonymous),
                                      new SqlParameter("@CommentContent",entity.CommentContent),
                                      new SqlParameter("@CommentTime",entity.CommentTime==null ? DateTime.Now:entity.CommentTime),
                                      new SqlParameter("@CheckTypeID",entity.CheckTypeID),
                                      new SqlParameter("@ViewStateID",entity.ViewStateID)
                                      };
                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_EventItemComments_Insert", prms).Tables[0];

                if (table != null && table.Rows.Count != 0)
                {
                    rcEntity.ReviewCount = table.Rows[0]["ReviewCount"].ToInt();
                    rcEntity.ArticleLikeCount = table.Rows[0]["ArticleLikeCount"].ToInt();
                    rcEntity.IsArticleLike = table.Rows[0]["IsArticleLike"].ToInt() == 0 ? false : true;
                }
                return new OperationResult<ReviewCommonEntity>(OperationResultType.Success, "添加文章评论完成！", rcEntity);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<ReviewCommonEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        ///  文章点赞插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult<ReviewCommonEntity> ArticleLike_Insert(EventItemLikeEntity entity) 
        {
            try
            {
                ReviewCommonEntity rcEntity = new ReviewCommonEntity();
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SqlTransaction trans = null;
                    con.Open();
                    trans = con.BeginTransaction();
                    SqlParameter[] prms = {
                                      new SqlParameter("@EventItemID",entity.EventItemID),
                                      new SqlParameter("@UserID",entity.UserID),
                                      new SqlParameter("@LikeCnt",entity.LikeCnt),
                                      new SqlParameter("@LikeTime",entity.LikeTime==null ? DateTime.Now :entity.LikeTime)
                                      };
                    SqlParameter[] commPrms = {
                                      new SqlParameter("@EventItemID",entity.EventItemID),
                                      new SqlParameter("@UserID",entity.UserID)
                                      };
                    int state = SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_EventItem_Like_Insert", prms);
                    trans.Commit();
                    DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_ReviewCommon_Get", commPrms).Tables[0]; 
                    if (table != null && table.Rows.Count != 0)
                    {
                        rcEntity.ReviewCount = table.Rows[0]["ReviewCount"].ToInt();
                        rcEntity.ArticleLikeCount = table.Rows[0]["ArticleLikeCount"].ToInt();
                        rcEntity.IsArticleLike = table.Rows[0]["IsArticleLike"].ToInt() == 0 ? false : true;
                    }
                    if (state == 1)
                    {
                        return new OperationResult<ReviewCommonEntity>(OperationResultType.Success, "文章点赞成功！", rcEntity);
                    }
                    else 
                    {
                        return new OperationResult<ReviewCommonEntity>(OperationResultType.NoChanged, "您已点过赞了！", rcEntity);
                    }
                }
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<ReviewCommonEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }

        }

        /// <summary>
        ///  查询回复列表头信息
        /// </summary>
        /// <param name="reviewsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OperationResult<ReplyHeaderEntity> ArticleReplyHeader_Get(int reviewsId,int userId) 
        {
            ReplyHeaderEntity entity = new ReplyHeaderEntity();
            try
            {
                SqlParameter[] prms = { new SqlParameter("@CommentID",reviewsId),new SqlParameter("@UserID", userId) };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_ReplyHeader_Get", prms).Tables[0];
                if (table != null && table.Rows.Count != 0)
                {
                    entity.CommentID = table.Rows[0]["CommentID"].ToInt();
                    entity.EventItemID = table.Rows[0]["EventItemID"].ToInt();
                    entity.EventItemGUID = Guid.Parse(table.Rows[0]["EventItemGUID"].ToString());
                    entity.UserID = table.Rows[0]["UserID"].ToInt();
                    entity.IsAnonymous = Convert.ToBoolean(table.Rows[0]["IsAnonymous"]);
                    entity.CommentContent = table.Rows[0]["CommentContent"].ToString();
                    entity.CommentTime = table.Rows[0]["CommentTime"].ToDateTime();
                    entity.NickName = table.Rows[0]["NickName"].ToString();
                    entity.ThirdPictureUrl = table.Rows[0]["ThirdPictureUrl"].ToString();
                    entity.Title = table.Rows[0]["Title"].ToString();
                    entity.Url = table.Rows[0]["Url"].ToString();
                    entity.UserPicture = table.Rows[0]["UserPicture"].ToString();
                    entity.UserPictureDomain = table.Rows[0]["UserPictureDomain"].ToString();
                    entity.EventItemPicture = table.Rows[0]["EventItemPicture"].ToString();
                    entity.EventItemPictureDomain = table.Rows[0]["EventItemPictureDomain"].ToString();
                    entity.ReplyCount = table.Rows[0]["ReplyCount"].ToInt();
                    entity.ReviewLikeCount = table.Rows[0]["ReviewLikeCount"].ToInt();
                    entity.Content = table.Rows[0]["Content"].ToString();
                    entity.StartTime = Convert.ToDateTime(table.Rows[0]["StartTime"]);
                    entity.EndTime = Convert.ToDateTime(table.Rows[0]["EndTime"]);
                    entity.IsLike =table.Rows[0]["IsLike"].ToInt()>0;
                }
                return new OperationResult<ReplyHeaderEntity>(OperationResultType.Success, "数据获取完成！", entity);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<ReplyHeaderEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        /// 回复主体列表
        /// </summary>
        /// <param name="reviewsId">评论Id</param>
        /// <param name="replyId">回复Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public OperationResult<List<EventItemReplyEntity>> ArticleReply_GetList(int reviewsId,int replyId,int pageIndex,int pageSize)
        {
            try
            {
                List<EventItemReplyEntity> entitys = new List<EventItemReplyEntity>();
                SqlParameter[] parms =
                            { 
                                new SqlParameter("@CommentID",reviewsId),
                                new SqlParameter("@ReplyID",replyId),
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };

                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_EventItemReply_GetList", parms).Tables[0];
                entitys=ConvertDataTable<EventItemReplyEntity>.ConvertToList(table);
                return new OperationResult<List<EventItemReplyEntity>>(OperationResultType.Success,"回复列表获取完成！",entitys);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<List<EventItemReplyEntity>>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        ///  添加回复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult<AddReplyCommonEntity> ArticleReply_Insert(EventItemReplyEntity entity) 
        {
            try
            {
                AddReplyCommonEntity acEntity = new AddReplyCommonEntity();
                ReplyCommonEntity rcEntity = new ReplyCommonEntity();
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SqlTransaction trans = null;
                    con.Open();
                    trans = con.BeginTransaction();
                    DateTime? nowDate = entity.ReplyTime == null ? DateTime.Now : entity.ReplyTime;
                    SqlParameter[] prms = {
                                      new SqlParameter("@ReplyID",entity.ReplyID),
                                      new SqlParameter("@CommentID",entity.CommentID),
                                      new SqlParameter("@UserID",entity.UserID),
                                      new SqlParameter("@IsAnonymous",entity.IsAnonymous),
                                      new SqlParameter("@ReplyParentID",entity.ReplyParentID),
                                      new SqlParameter("@ReplyContent",entity.ReplyContent),
                                      new SqlParameter("@ReplyTime",nowDate),
                                      new SqlParameter("@CheckTypeID",entity.CheckTypeID),
                                      new SqlParameter("@ViewStateID",entity.ViewStateID)
                                      };
                    SqlParameter[] commPrms = {
                                      new SqlParameter("@CommentID",entity.CommentID),
                                      new SqlParameter("@UserID",entity.UserID),
                                      };

                    prms[0].Direction = ParameterDirection.Output;
                    int state = SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_EventItemReply_Insert", prms);
                    trans.Commit();

                    DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_ReplyCommon_Get", commPrms).Tables[0];
                    if (table != null && table.Rows.Count != 0)
                    {
                        rcEntity = ConvertDataTable<ReplyCommonEntity>.ConvertToList(table)[0];
                    }
                    acEntity.ReplyID = prms[0].Value.ToInt();
                    acEntity.ReplyTime = nowDate;
                    acEntity.ReviewLikeCount = rcEntity.ReviewLikeCount;
                    acEntity.IsReviewLike = rcEntity.IsReviewLike;
                    acEntity.ReplyCount = rcEntity.ReplyCount;
                    if (state > 0)
                    {
                        return new OperationResult<AddReplyCommonEntity>(OperationResultType.Success, "回复完成！", acEntity);
                    }
                    else
                    {
                        return new OperationResult<AddReplyCommonEntity>(OperationResultType.NoChanged, "评论已被删除,无法回复！", acEntity);
                    }
                }
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<AddReplyCommonEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        /// 评论点赞插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult<ReplyCommonEntity> CommentsLike_Insert(CommentLikeEntity entity) 
        {
            try
            {
                ReplyCommonEntity rcEntity = new ReplyCommonEntity();
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SqlTransaction trans = null;
                    con.Open();
                    trans = con.BeginTransaction();

                    SqlParameter[] prms = {
                                      new SqlParameter("@CommentID",entity.CommentID),
                                      new SqlParameter("@UserID",entity.UserID),
                                      new SqlParameter("@LikeCnt",entity.LikeCnt),
                                      new SqlParameter("@LikeTime",entity.LikeTime==null ? DateTime.Now :entity.LikeTime)
                                      };
                    SqlParameter[] commPrms = {
                                      new SqlParameter("@CommentID",entity.CommentID),
                                      new SqlParameter("@UserID",entity.UserID)
                                      };
                    int state = SQlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "M_Comments_Like_Insert", prms);
                    trans.Commit();

                    DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_ReplyCommon_Get", commPrms).Tables[0];
                    if (table != null && table.Rows.Count != 0)
                    {
                        rcEntity = ConvertDataTable<ReplyCommonEntity>.ConvertToList(table)[0];
                    }
                    if (state > 0)
                    {
                        return new OperationResult<ReplyCommonEntity>(OperationResultType.Success, "评论点赞成功！", rcEntity);
                    }
                    else
                    {
                        return new OperationResult<ReplyCommonEntity>(OperationResultType.NoChanged, "您已点过赞了！", rcEntity);
                    }
                }
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<ReplyCommonEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        ///  文章点赞数与评论数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult<ReviewCommonEntity> ReviewCommon_Get(EventItemCommentEntity entity) 
        {
            try
            {
                ReviewCommonEntity rcEntity = new ReviewCommonEntity();
                SqlParameter[] commPrms = {
                                      new SqlParameter("@EventItemID",entity.EventItemID),
                                      new SqlParameter("@UserID",entity.UserID)
                                      };
                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_ReviewCommon_Get", commPrms).Tables[0];
                if (table != null && table.Rows.Count != 0)
                {
                    rcEntity.ReviewCount = table.Rows[0]["ReviewCount"].ToInt();
                    rcEntity.ArticleLikeCount = table.Rows[0]["ArticleLikeCount"].ToInt();
                    rcEntity.IsArticleLike = table.Rows[0]["IsArticleLike"].ToInt() == 0 ? false : true;
                }
                return new OperationResult<ReviewCommonEntity>(OperationResultType.Success, "评论和点赞数抓取完成！", rcEntity);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<ReviewCommonEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }


        /// <summary>
        /// 评论举报
        /// </summary>
        /// <param name="entity">举报实体</param>
        /// <returns></returns>
        public OperationResult<bool> Report_Insert(ReportEntity entity) 
        {
            try 
            {
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    SqlTransaction trans = null;
                    con.Open();
                    trans = con.BeginTransaction();
                    SqlParameter[] prms = {
                                      new SqlParameter("@FromUserID",entity.FromUserID),
                                      new SqlParameter("@ReportTypeID",entity.ReportTypeID),
                                      new SqlParameter("@ReportContent",entity.ReportContent),
                                      new SqlParameter("@ReportInfoTypeID",entity.ReportInfoTypeID),
                                      new SqlParameter("@ReportInfoID",entity.ReportInfoID),
                                      new SqlParameter("@CreateTime",entity.CreateTime==null ? DateTime.Now :entity.CreateTime)
                                          };
                    string insert_sql = string.Empty;

                    string select_sql = string.Empty;
                    select_sql = " SELECT COUNT(1) FROM M_Report WHERE FromUserID={0} AND ReportInfoTypeID={1} AND ReportInfoID={2} ";
                    select_sql = string.Format(select_sql, entity.FromUserID, entity.ReportInfoTypeID, entity.ReportInfoID);
                    int count =int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, select_sql).ToString());
                    if (count > 0) 
                    {
                        return new OperationResult<bool>(OperationResultType.NoChanged, "您已举报过了！", false);
                    }
                    insert_sql = "  INSERT INTO  M_Report(FromUserID,ReportTypeID,ReportContent,ReportInfoTypeID,ReportInfoID,CreateTime) ";
                    insert_sql += " VALUES(@FromUserID,@ReportTypeID,@ReportContent,@ReportInfoTypeID,@ReportInfoID,@CreateTime) ";
                    bool state = SQlHelper.ExecuteNonQuery(trans, CommandType.Text, insert_sql, prms) > 0;
                    trans.Commit();
                    if (state)
                    {
                        return new OperationResult<bool>(OperationResultType.Success, "举报成功！", state);
                    }
                    else 
                    {
                        return new OperationResult<bool>(OperationResultType.Error, "举报失败！", state);
                    }
                }
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<bool>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

    }
}

