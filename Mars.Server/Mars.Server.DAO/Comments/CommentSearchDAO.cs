using Mars.Server.Entity.Comments;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Comments
{
    public class CommentSearchDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(CommentSearchEntity info, out int totalcnt)
        {
            try
            {
                SqlParameter[] prms = ParseToSqlParameters(info).ToArray();
                DataTable dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager05", prms).Tables[0];
                totalcnt = int.Parse(prms[prms.Length - 1].Value.ToString());

                return dt;
            }
            catch (Exception ex)
            {
                totalcnt = -1;
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public bool ChangeViewState(int commentID, int viewStateID)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE dbo.M_EventItem_Comments SET ViewStateID = {1} WHERE CommentID = {0}"
                , commentID.ToString()
                , viewStateID.ToString());

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
            return returnValue;
        }

        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(CommentSearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(CommentSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"dbo.M_EventItem_Comments AS ec
                INNER JOIN dbo.M_User AS u ON u.UserID = ec.UserID
                INNER JOIN dbo.M_EventItem AS e ON e.EventItemID = ec.EventItemID
                INNER JOIN M_Dict_CheckType AS dct ON dct.CheckTypeID=ec.CheckTypeID
                LEFT JOIN 
                (SELECT SUM(LikeCnt) AS LikeCnt,CommentID FROM dbo.M_EventItem_Comments_Like_Rel GROUP BY CommentID) AS eclr ON eclr.CommentID = ec.CommentID
                LEFT JOIN
                (SELECT COUNT(ReplyID) AS ReplyCnt,CommentID FROM M_EventItem_Replies GROUP BY CommentID) AS er ON er.CommentID = ec.CommentID");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(CommentSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ec.CommentTime desc ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(CommentSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"ec.CommentID,ec.EventItemID,ec.UserID,ec.IsAnonymous,ec.CommentContent,ec.CommentTime,ec.CheckTypeID,ec.ViewStateID
                    ,CONVERT(varchar(30),ec.CommentTime, 20) as FormatCommentTime
                    ,e.EventItemGUID,e.Title
                    ,u.NickName
                    ,ISNULL(eclr.LikeCnt,0) AS LikeCnt
                    ,ISNULL(er.ReplyCnt,0) AS ReplyCnt
                    ,dct.CheckTypeDesc
                    ,ec.WordsInfo");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(CommentSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(sp.Title))
            {
                sbwhere.AppendFormat(" AND e.Title LIKE '%{0}%' "
                    , sp.Title);
            }

            if (!string.IsNullOrEmpty(sp.CheckType))
            {
                sbwhere.AppendFormat(" AND ec.CheckTypeID = {0}"
                    , sp.CheckType);
            }

            if (!string.IsNullOrEmpty(sp.ViewType))
            {
                sbwhere.AppendFormat(" AND ec.ViewStateID = {0} "
                    , sp.ViewType);
            }


            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
