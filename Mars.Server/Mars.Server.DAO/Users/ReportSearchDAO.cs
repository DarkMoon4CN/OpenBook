using Mars.Server.Entity.Users;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Users
{
    public class ReportSearchDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(ReportSearchEntity info, out int totalcnt)
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

        public bool Delete(int reportID)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM M_Report WHERE ReportID = {0}"
                , reportID.ToString());

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
        protected virtual List<SqlParameter> ParseToSqlParameters(ReportSearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(ReportSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"[dbo].[M_Report] AS r
            INNER JOIN dbo.M_Dict_ReportType AS drt ON drt.ReportTypeID = r.ReportTypeID
            INNER JOIN dbo.M_Dict_ReportInfoType AS drit ON drit.ReportInfoTypeID = r.ReportInfoTypeID
            LEFT JOIN dbo.M_User AS u ON r.FromUserID = u.UserID
            LEFT JOIN dbo.M_EventItem_Comments AS ec ON ec.CommentID = r.ReportInfoID AND r.ReportInfoTypeID = 1
            LEFT JOIN dbo.M_EventItem_Replies AS er ON er.ReplyID = r.ReportInfoID AND r.ReportInfoTypeID= 2
            LEFT JOIN dbo.M_User AS tu ON tu.UserID = ec.UserID OR tu.UserID=er.UserID");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(ReportSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" r.CreateTime DESC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(ReportSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"r.*
                ,drt.ReportTypeDesc
                ,drit.ReportInfoTypeDesc
                ,u.NickName AS FormUserName
                ,tu.NickName AS ToUserName
                ,ISNULL(ec.CommentContent,'')+ ISNULL(er.ReplyContent,'') AS ReportInfo
                ,CONVERT(NVARCHAR(20),r.CreateTime,20) AS FormatCreateTime");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }
        //,CONVERT(varchar(30), a.ActivityStartTime, 20) as ActivityStartTime
        //,CONVERT(varchar(30), a.ActivityEndTime, 20) as ActivityEndTime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(ReportSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(sp.StartTime) && !string.IsNullOrEmpty(sp.EndTime))
            {
                sbwhere.AppendFormat(" AND r.CreateTime BETWEEN '{0} 00:00:00' AND '{1} 23:59:59' "
                    , sp.StartTime, sp.EndTime);
            }
            else if (string.IsNullOrEmpty(sp.StartTime) && !string.IsNullOrEmpty(sp.EndTime))
            {
                sbwhere.AppendFormat(" AND r.CreateTime < '{0} 23:59:59' "
                   , sp.EndTime);
            }
            else if (!string.IsNullOrEmpty(sp.StartTime) && string.IsNullOrEmpty(sp.EndTime))
            {
                sbwhere.AppendFormat(" AND r.CreateTime > '{0} 00:00:00' "
                    , sp.StartTime);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
