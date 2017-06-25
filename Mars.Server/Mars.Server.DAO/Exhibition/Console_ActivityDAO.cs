using Mars.Server.Entity.Exhibition;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Exhibition
{
    public class Console_ActivityDAO
    {
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

        /// <summary>
        /// 更新活动信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update_Activity(ActivityEntity item)
        {
            StringBuilder strSql = new StringBuilder();
            if (item.ParentID == 0)
            {
                strSql.Append("update M_Exhibition_Activity set ");
                strSql.Append("ParentID=@ParentID,");
                strSql.Append("ExhibitionID=@ExhibitionID,");
                strSql.Append("ExhibitorID=@ExhibitorID,");
                strSql.Append("ActivityTitle=@ActivityTitle,");
                strSql.Append("ActivityStartTime=@ActivityStartTime,");
                strSql.Append("ActivityEndTime=@ActivityEndTime,");
                strSql.Append("ActivityLocation=@ActivityLocation,");
                strSql.Append("ActivityHostUnit=@ActivityHostUnit,");
                strSql.Append("ActivityAbstract=@ActivityAbstract,");
                strSql.Append("ActivityGuest=@ActivityGuest,");
                strSql.Append("StateTypeID=@StateTypeID,");
                strSql.Append("ActivityOrder=@ActivityOrder,");
                strSql.Append("ActivityTypeID=@ActivityTypeID");
                strSql.Append(" where ActivityID=@ActivityID;");

                strSql.Append(@" IF EXISTS(SELECT ActivityID FROM dbo.M_Exhibition_Activity WHERE ParentID = @ActivityID)
                                UPDATE M_Exhibition_Activity SET StateTypeID = @StateTypeID WHERE ParentID = @ActivityID; ");
            }
            else
            {
                strSql.Append("update M_Exhibition_Activity set ");
                strSql.Append("ActivityTitle=@ActivityTitle,");
                strSql.Append("ActivityStartTime=@ActivityStartTime,");
                strSql.Append("ActivityEndTime=@ActivityEndTime,");
                strSql.Append("ActivityLocation=@ActivityLocation,");
                strSql.Append("ActivityGuest=@ActivityGuest,");
                strSql.Append("StateTypeID=@StateTypeID,");
                strSql.Append("ActivityOrder=@ActivityOrder,");
                strSql.Append("ActivityTypeID=@ActivityTypeID");
                strSql.Append(" where ActivityID=@ActivityID;");
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@ParentID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitorID", SqlDbType.Int,4),
                    new SqlParameter("@ActivityTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@ActivityStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ActivityEndTime", SqlDbType.DateTime),
                    new SqlParameter("@ActivityLocation", SqlDbType.NVarChar,100),
                    new SqlParameter("@ActivityHostUnit", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ActivityAbstract", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ActivityGuest", SqlDbType.NVarChar,1000),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ActivityOrder", SqlDbType.Int,4),
                    new SqlParameter("@ActivityTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                    new SqlParameter("@ActivityID", SqlDbType.Int,4)};
            parameters[0].Value = item.ParentID;
            parameters[1].Value = item.ExhibitorID;
            parameters[2].Value = item.ActivityTitle;
            parameters[3].Value = item.ActivityStartTime;
            parameters[4].Value = item.ActivityEndTime;
            parameters[5].Value = item.ActivityLocation;
            parameters[6].Value = item.ActivityHostUnit;
            parameters[7].Value = item.ActivityAbstract;
            parameters[8].Value = item.ActivityGuest;
            parameters[9].Value = item.StateTypeID;
            parameters[10].Value = item.ActivityOrder;
            parameters[11].Value = item.ActivityTypeID;
            parameters[12].Value = item.ExhibitionID;
            parameters[13].Value = item.ActivityID;

            if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ImportActivitys(DataTable table, object exhibitionID, object userid)
        {
            bool returnValue = false;
            if (table != null)
            {
                if (table.Columns.Contains("活动名称") 
                    && table.Columns.Contains("开始时间")
                    && table.Columns.Contains("结束时间")
                    && table.Columns.Contains("活动位置")
                    && table.Columns.Contains("主办单位")
                    && table.Columns.Contains("简介")
                    && table.Columns.Contains("嘉宾"))
                {
                    DateTimeFormatInfo dtfi = new CultureInfo("zh-CN", false).DateTimeFormat;
                    StringBuilder strSql = new StringBuilder();
                    //strSql.Append("DECLARE @key INT;");
                    foreach (DataRow dr in table.Rows)
                    {
                        DateTime stime;
                        DateTime etime;
                        
                        DateTime.TryParseExact(dr["开始时间"].ToString(), "yyyy年M月d日H:m", dtfi, DateTimeStyles.None, out stime);
                        DateTime.TryParseExact(dr["结束时间"].ToString(), "yyyy年M月d日H:m", dtfi, DateTimeStyles.None, out etime);

                        strSql.AppendFormat(@"INSERT INTO [dbo].[M_Exhibition_Activity]
                               ([ParentID],[ExhibitionID],[ExhibitorID],[ActivityTitle],[ActivityStartTime],[ActivityEndTime],[ActivityLocation],[ActivityHostUnit]
                               ,[ActivityAbstract],[ActivityGuest],[StateTypeID],[ActivityOrder],[ActivityTypeID],[CreateUserID] ,[CreateTime])
                         VALUES
                               (0
                               ,{0}
                               ,0
                               ,'{1}'
                               ,'{2}'
                               ,'{3}'
                               ,'{4}'
                               ,'{5}'
                               ,'{6}'
                               ,'{7}'
                               ,1
                               ,1000
                               ,0
                               ,'{8}'
                               ,getdate());
"
                                , exhibitionID.ToString()
                                , dr["活动名称"].ToString()
                                , stime.ToString("yyyy-MM-dd HH:mm:ss")
                                , etime.ToString("yyyy-MM-dd HH:mm:ss")
                                , dr["活动位置"].ToString()
                                , dr["主办单位"].ToString()
                                , dr["简介"].ToString()
                                , dr["嘉宾"].ToString()
                                , userid);
                    }

                    try
                    {
                        SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
                        returnValue = true;
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog(ex);
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 删除活动，假删除，删除时同时会删除所有相关子活动
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public bool DeleteActivity(int activityID)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE dbo.M_Exhibition_Activity SET StateTypeID = 0 WHERE ActivityID = {0};
                        IF EXISTS (SELECT ActivityID FROM dbo.M_Exhibition_Activity WHERE ParentID={0})
                        UPDATE dbo.M_Exhibition_Activity SET StateTypeID = 0 WHERE ParentID ={0};"
                , activityID.ToString());

            try
            {
                if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0)
                {
                    returnValue = true;
                }
            }
            catch (Exception ex) {
                LogUtil.WriteLog(ex);
            }
            return returnValue;
        }

        public DataTable GetTable(int id)
        {
            DataTable table = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.* ");
            strSql.Append(" FROM M_Exhibition_Activity AS a ");
            strSql.Append(" WHERE a.ActivityID =@ActivityID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActivityID", SqlDbType.Int,4)
            };
            parameters[0].Value = id;

            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), parameters);
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
        /// 创建活动信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add_Activity(ActivityEntity item)
        {
            StringBuilder strSql = new StringBuilder();
            if (item.ParentID == 0)
            {
                strSql.Append("insert into M_Exhibition_Activity(");
                strSql.Append("ParentID,ExhibitionID,ExhibitorID,ActivityTitle,ActivityStartTime,ActivityEndTime,ActivityLocation,ActivityHostUnit,ActivityAbstract,ActivityGuest,StateTypeID,ActivityOrder,ActivityTypeID,CreateUserID,CreateTime)");
                strSql.Append(" values (");
                strSql.Append("@ParentID,@ExhibitionID,@ExhibitorID,@ActivityTitle,@ActivityStartTime,@ActivityEndTime,@ActivityLocation,@ActivityHostUnit,@ActivityAbstract,@ActivityGuest,@StateTypeID,@ActivityOrder,@ActivityTypeID,@CreateUserID,@CreateTime)");
                strSql.Append(";select @@IDENTITY");
            }
            else
            {
                strSql.Append(@"insert into M_Exhibition_Activity(ParentID,ExhibitionID,ExhibitorID,ActivityTitle,ActivityStartTime,ActivityEndTime,ActivityLocation,ActivityHostUnit,ActivityAbstract,ActivityGuest,StateTypeID,ActivityOrder,ActivityTypeID,CreateUserID,CreateTime)
                    SELECT @ParentID,@ExhibitionID,@ExhibitorID,@ActivityTitle,@ActivityStartTime,@ActivityEndTime,@ActivityLocation,ActivityHostUnit,@ActivityAbstract,@ActivityGuest,@StateTypeID,@ActivityOrder,ActivityTypeID,@CreateUserID,@CreateTime
                    FROM dbo.M_Exhibition_Activity 
                    WHERE ActivityID = @ParentID 
                    ;select @@IDENTITY");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@ParentID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitorID", SqlDbType.Int,4),
                    new SqlParameter("@ActivityTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@ActivityStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ActivityEndTime", SqlDbType.DateTime),
                    new SqlParameter("@ActivityLocation", SqlDbType.NVarChar,100),
                    new SqlParameter("@ActivityHostUnit", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ActivityAbstract", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ActivityGuest", SqlDbType.NVarChar,1000),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ActivityOrder", SqlDbType.Int,4),
                    new SqlParameter("@ActivityTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = item.ParentID;
            parameters[1].Value = item.ExhibitorID;
            parameters[2].Value = item.ActivityTitle;
            parameters[3].Value = item.ActivityStartTime;
            parameters[4].Value = item.ActivityEndTime;
            parameters[5].Value = item.ActivityLocation;
            parameters[6].Value = item.ActivityHostUnit;
            parameters[7].Value = item.ActivityAbstract;
            parameters[8].Value = item.ActivityGuest;
            parameters[9].Value = item.StateTypeID;
            parameters[10].Value = item.ActivityOrder;
            parameters[11].Value = item.ActivityTypeID;
            parameters[12].Value = item.ExhibitionID;
            parameters[13].Value = item.CreateUserID;
            parameters[14].Value = item.CreateTime;

            object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr,CommandType.Text,strSql.ToString(),parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
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
                INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = a.ExhibitionID
                LEFT JOIN M_Exhibition_Activity ca ON ca.ParentID = a.ActivityID");

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
                ,CONVERT(varchar(30), a.ActivityStartTime, 20) as FormatActivityStartTime
                ,CONVERT(varchar(30), a.ActivityEndTime, 20) as FormatActivityEndTime
                ,a.ActivityLocation
                ,a.ActivityHostUnit
                ,a.ActivityAbstract
                ,a.ActivityGuest
                ,ISNULL(ee.ExhibitorName,'') AS ExhibitorName
                ,ISNULL(ee.ExhibitorPinYin,'') AS ExhibitorPinYin
                ,ActivityIsEnd = CASE WHEN a.ActivityEndTime<GETDATE() THEN 1 ELSE 0 END
                ,COUNT(ca.ActivityID) AS Cnt");
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
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND ((ee.ExhibitorID IS NOT NULL AND ee.StateTypeID=1) OR ee.ExhibitorID IS NULL) AND em.StateTypeID=1 AND a.StateTypeID=1 ");

            if (sp.ParentID > 0)
            {
                sbwhere.AppendFormat(" AND a.ParentID = {0}", sp.ParentID);
            }
            else
            {
                sbwhere.Append(" AND a.ParentID = 0");
            }

            if (sp.ExhibitionID > 0)
            {
                sbwhere.AppendFormat(" AND em.ExhibitionID = {0}", sp.ExhibitionID);
            }

            if (!string.IsNullOrEmpty(sp.SearchName))
            {
                sbwhere.AppendFormat(@" AND a.ActivityTitle LIKE '%{0}%' "
                            , sp.SearchName);
            }

            sbwhere.Append(@" GROUP BY a.ActivityID
                , a.ParentID
                , a.ExhibitorID
                , a.ActivityTitle
                , a.ActivityStartTime
                , a.ActivityEndTime
                , a.ActivityStartTime
                , a.ActivityEndTime
                , a.ActivityLocation
                , a.ActivityHostUnit
                , a.ActivityAbstract
                , a.ActivityGuest
                , ee.ExhibitorName
                , ee.ExhibitorPinYin
                , a.ActivityEndTime ");

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
