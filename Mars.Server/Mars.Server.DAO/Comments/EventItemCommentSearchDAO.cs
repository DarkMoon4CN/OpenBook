using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Mars.Server.Entity.Comments;
using System.Data.SqlClient;
using Mars.Server.Utils;

namespace Mars.Server.DAO.Comments
{
    public class EventItemCommentSearchDAO
    {
        public DataTable QueryData(EventItemCommentSearchEntity entity, out int totalcnt)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] prms = ParseToSqlParameters(entity).ToArray();
                if (entity.IsShare == 1)
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append(" SELECT top 2 ");
                    StrSql.Append(prms[1].Value.ToString());
                    StrSql.Append(" from ");
                    StrSql.Append(prms[0].Value.ToString());
                    StrSql.Append(" where ");
                    StrSql.Append(prms[3].Value.ToString());
                    StrSql.Append(" order by ");
                    StrSql.Append(prms[2].Value.ToString());

                    dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, StrSql.ToString(), null).Tables[0];
                    totalcnt = dt.Rows.Count;
                }
                else
                {
                    dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager05", prms).Tables[0];
                    int.TryParse(prms[prms.Length - 1].Value.ToString(), out totalcnt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                totalcnt = -1;
                LogUtil.WriteLog(ex);
                return null;
            }
        }


        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(EventItemCommentSearchEntity sp)
        {
            List<SqlParameter> _parameters = new List<SqlParameter>();
            _parameters.Add(CreateParameter_Table(sp));
            _parameters.Add(CreateParameter_Fileds(sp));
            _parameters.Add(CreateParamter_Orderby(sp));
            _parameters.Add(CreateParameter_Where(sp));

            _parameters.Add(new SqlParameter("@pageSize", sp.PageSize));
            _parameters.Add(new SqlParameter("@pageIndex", sp.PageIndex));
            _parameters.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = System.Data.ParameterDirection.Output });
            return _parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Table(EventItemCommentSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.AppendFormat(@" dbo.M_EventItem_Comments AS c
            INNER JOIN dbo.M_User AS u ON u.UserID = c.UserID
            INNER JOIN dbo.M_EventItem AS e ON e.EventItemID = c.EventItemID
            LEFT JOIN dbo.M_V_Picture AS up ON up.PictureID = u.PictureID
            LEFT JOIN 
			 (SELECT SUM(LikeCnt) AS LikeCnt,CommentID FROM M_EventItem_Comments_Like_Rel GROUP BY CommentID) AS clr ON clr.CommentID = c.CommentID
			LEFT JOIN 
			 (SELECT COUNT(CLID) AS IsLike,CommentID,UserID FROM M_EventItem_Comments_Like_Rel GROUP BY CommentID,UserID) AS clru ON clru.CommentID = c.CommentID AND clru.UserID = {0}
			LEFT JOIN 
			 (SELECT COUNT(ReplyID) AS ReplyCnt,CommentID FROM M_EventItem_Replies WHERE ViewStateID=1 GROUP BY CommentID) AS rlr ON rlr.CommentID = c.CommentID "
                , sp.UserID.ToString());

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(EventItemCommentSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" c.CommentTime DESC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(EventItemCommentSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@" c.CommentID
                    ,c.CommentContent
                    ,LEN(CommentContent) AS lenContent 
                    ,c.CommentTime
                    ,CONVERT(VARCHAR(2),DATEPART(MONTH,c.CommentTime))+'月'+CONVERT(VARCHAR(2),DATEPART(DAY,c.CommentTime))+'日' AS FormatCommentDate
                    ,CONVERT(VARCHAR(5), c.CommentTime, 8) AS FormatCommentTime
                    ,c.IsAnonymous
                    ,c.UserID
                    ,ISNULL(u.NickName,'昵称未设置') AS NickName
                    ,u.ThirdPictureUrl
                    ,up.Domain AS UserDomain
                    ,up.PicturePath AS UserPictuePath
                    ,ISNULL(clr.LikeCnt,0) AS LikeCnt
                    ,ISNULL(rlr.ReplyCnt,0) AS ReplyCnt
                    ,ISNULL(clru.IsLike,0) AS IsLike ");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(EventItemCommentSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND c.ViewStateID=1 ");

            if (sp.EventItemID > 0)
            {
                sbwhere.AppendFormat(" AND c.EventItemID={0} "
                    , sp.EventItemID);
            }
            else if(!string.IsNullOrEmpty(sp.EventItemGUID))
            {
                sbwhere.AppendFormat(" AND e.EventItemGUID='{0}' "
                    , sp.EventItemGUID);
            }
            else
            {
                sbwhere.AppendFormat(" AND 1<>1 "
                    , sp.EventItemID);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
