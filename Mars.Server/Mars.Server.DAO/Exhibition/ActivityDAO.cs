using Mars.Server.Entity.Exhibition;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Exhibition
{
    public class ActivityDAO
    {
        /// <summary>
        /// 根据展场id 获取展场相关活动信息
        /// </summary>
        /// <param name="exhibitionID">展场id</param>
        /// <returns></returns>
        public DataTable GetActivityDataTable(int exhibitionID)
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT 
                        a.ActivityID
                        ,a.ParentID
                        ,a.ExhibitorID
                        ,a.ActivityTitle
                        ,a.ActivityStartTime
                        ,a.ActivityEndTime
                        ,a.ActivityLocation
                        ,a.ActivityHostUnit
                        ,a.ActivityAbstract
                        ,a.ActivityGuest
                        ,ISNULL(ee.ExhibitorName,'') AS ExhibitorName
                        ,ISNULL(ee.ExhibitorPinYin,'') AS ExhibitorPinYin
                        ,ActivityIsEnd = CASE WHEN a.ActivityEndTime<GETDATE() THEN 1 ELSE 0 END
                        FROM dbo.M_Exhibition_Activity AS a
                        LEFT JOIN dbo.M_Exhibition_Exhibitors AS ee ON ee.ExhibitorID = a.ExhibitorID
                        INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = a.ExhibitionID
                        WHERE ((ee.ExhibitorID IS NOT NULL AND ee.StateTypeID=1) OR ee.ExhibitorID IS NULL) AND a.StateTypeID=1 AND em.StateTypeID=1 AND em.ExhibitionID={0}"
                , exhibitionID.ToString());

            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(ActivitySearchEntity info, out int totalcnt)
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

        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(ActivitySearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(ActivitySearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"dbo.M_Exhibition_Activity AS a
                LEFT JOIN dbo.M_Exhibition_Exhibitors AS ee ON ee.ExhibitorID = a.ExhibitorID
                INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = a.ExhibitionID");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(ActivitySearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" CASE WHEN a.ActivityEndTime<GETDATE() THEN 1 ELSE 0 END ASC,a.ActivityOrder ASC,a.ActivityStartTime ASC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(ActivitySearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"a.ActivityID
                ,a.ParentID
                ,a.ExhibitorID
                ,a.ActivityTitle
                ,a.ActivityStartTime
                ,a.ActivityEndTime
                ,a.ActivityLocation
                ,a.ActivityHostUnit
                ,a.ActivityAbstract
                ,a.ActivityGuest
                ,ISNULL(ee.ExhibitorName,'') AS ExhibitorName
                ,ISNULL(ee.ExhibitorPinYin,'') AS ExhibitorPinYin
                ,ActivityIsEnd = CASE WHEN a.ActivityEndTime<GETDATE() THEN 1 ELSE 0 END");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }
        //,CONVERT(varchar(30), a.ActivityStartTime, 20) as ActivityStartTime
        //,CONVERT(varchar(30), a.ActivityEndTime, 20) as ActivityEndTime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(ActivitySearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND ((ee.ExhibitorID IS NOT NULL AND ee.StateTypeID=1) OR ee.ExhibitorID IS NULL) AND a.StateTypeID=1 AND em.StateTypeID=1 ");

            if (sp.ExhibitionID > 0)
            {
                sbwhere.AppendFormat(" AND em.ExhibitionID = {0}", sp.ExhibitionID);
            }

            if (!string.IsNullOrEmpty(sp.SearchName))
            {
                sbwhere.AppendFormat(@" AND (a.ActivityHostUnit LIKE '%{0}%'  
                            OR a.ActivityTitle LIKE '%{0}%') "
                            , sp.SearchName);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
