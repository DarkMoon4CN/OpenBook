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
    public class ReplySearchhDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(ReplySearchEntity info, out int totalcnt)
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

        public bool ChangeViewState(int replyID, int viewStateID)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE M_EventItem_Replies SET ViewStateID = {1} WHERE ReplyID = {0}"
                , replyID.ToString()
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
        protected virtual List<SqlParameter> ParseToSqlParameters(ReplySearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(ReplySearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"M_EventItem_Replies AS er
                INNER JOIN dbo.M_EventItem_Comments AS ec ON ec.CommentID = er.CommentID
                INNER JOIN dbo.M_User AS u ON u.UserID = er.UserID
                INNER JOIN dbo.M_User AS u2 ON u2.UserID = ec.UserID
                INNER JOIN dbo.M_EventItem AS e ON e.EventItemID = ec.EventItemID
                INNER JOIN M_Dict_CheckType AS dct ON dct.CheckTypeID=er.CheckTypeID");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(ReplySearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" er.ReplyTime desc ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(ReplySearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"er.*
                ,CONVERT(varchar(30),er.ReplyTime, 20) as FormatReplyTime
                ,ec.CommentContent
                ,e.EventItemGUID,e.Title
                ,u.NickName,u2.UserID AS UserID2,u2.NickName AS NickName2
                ,dct.CheckTypeDesc");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(ReplySearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(sp.Title))
            {
                sbwhere.AppendFormat(" AND e.Title LIKE '%{0}%' "
                    , sp.Title);
            }

            if (!string.IsNullOrEmpty(sp.CheckType))
            {
                sbwhere.AppendFormat(" AND er.CheckTypeID = {0}"
                    , sp.CheckType);
            }

            if (!string.IsNullOrEmpty(sp.ViewType))
            {
                sbwhere.AppendFormat(" AND er.ViewStateID = {0} "
                    , sp.ViewType);
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
