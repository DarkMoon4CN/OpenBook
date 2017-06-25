using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.Entity;
using Mars.Server.Utils;
using Dapper;
using System.Data.SqlClient;
using System.Data;
namespace Mars.Server.DAO
{
    public class FestivalDAO
    {
        public List<FestivalEntity> QueryAllFestivalsTillNow(DateTime till_date)
        {
            List<FestivalEntity> items = new List<FestivalEntity>();
            try
            {
                string sql = string.Empty;
                sql += " SELECT  * FROM M_Festival  WHERE   StartTime>=@Date AND   FestivalType  IN(1,2)   ";
                sql += " OR FestivalID  IN (SELECT FestivalID from  M_Festival as mf WHERE mf.FestivalType =3 AND mf.FestivalWeight =999) ";
                sql += " ORDER BY StartTime ASC ";

                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<FestivalEntity>(sql, new { Date = till_date }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public List<FestivalEntity> QueryAllFestivalsTillNow2(DateTime till_date)
        {
            List<FestivalEntity> items = new List<FestivalEntity>();
            try
            {
                string sql = string.Empty;
                sql += " SELECT  * FROM M_Festival  WHERE   StartTime>=@Date ORDER BY StartTime ASC   ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<FestivalEntity>(sql, new { Date = till_date }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }



        public List<FestivalEntity>  QueryAllFestivalsForHash(DateTime till_date)
        { 
             try
            {
                string sql = "select FestivalID,FestivalWeight from M_Festival where FestivalDate>=@Date order by FestivalID";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<FestivalEntity>(sql, new { Date = till_date }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }


        /// <summary>
        /// 添加日历信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string InsertFestival(FestivalEntity entity)
        {
            string flg = "0";
            string sql = "select * from M_Festival where FestivalName=@FestivalName and StartTime=@StartTime and EndTime=@EndTime  and FestivalType=@FestivalType";
            string sql1 = "insert into M_Festival(FestivalName,FestivalShortName,StartTime,EndTime,FestivalType,FestivalWeight) values (@FestivalName,@FestivalShortName,@StartTime,@EndTime,@FestivalType,@FestivalWeight)";
            SqlParameter[] pars = { new SqlParameter("FestivalName",SqlDbType.NVarChar,50),
                                  new SqlParameter("FestivalShortName",SqlDbType.NVarChar,50),
                                  new SqlParameter("StartTime",SqlDbType.DateTime),
                                  new SqlParameter("EndTime",SqlDbType.DateTime),
                                  new SqlParameter("FestivalType",SqlDbType.Int),
                                  new SqlParameter("FestivalWeight",SqlDbType.Int)};
            pars[0].Value = entity.FestivalName;
            pars[1].Value = entity.FestivalShortName;
            pars[2].Value = entity.StartTime;
            pars[3].Value = entity.EndTime;
            pars[4].Value = entity.FestivalType;
            pars[5].Value = entity.FestivalWeight;
            SqlTransaction trans = null;

            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    DataTable db = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql, pars).Tables[0];
                    if (db.Rows.Count == 0)
                    {
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql1, pars);
                        flg = "1";
                        trans.Commit();
                    }
                    else
                    {
                        flg = "2";
                    }
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(e);
                    if (trans != null)
                    {
                        flg = "0";        //数据传输错误！
                        trans.Rollback();
                    }
                }
            }
            return flg;
        }
        /// <summary>
        /// 得到日历信息列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable GetFestivalList(searchFestivalEntity entity, out int totalcnt)
        {

            DataTable table = null;
            totalcnt = 0;
            try
            {
                SqlParameter[] prms = ParseToSqlParameters(entity).ToArray();

                if (entity.UseDBPagination)
                {
                    table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                    totalcnt = int.Parse(prms[prms.Length - 1].Value.ToString());
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                totalcnt = -1;
                LogUtil.WriteLog(ex);
            }

            return table;
        }
        #region 构造查询条件
        /// <summary>
        /// 生成拼接sql参数列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SqlParameter> ParseToSqlParameters(searchFestivalEntity entity)
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
        private SqlParameter CPWhere(searchFestivalEntity entity)
        {
            StringBuilder sbwhere = new StringBuilder("1=1");
            if (!string.IsNullOrEmpty(entity.StartTime))
            {
                sbwhere.Append("and f.StartTime>='" + entity.StartTime + " 00:00:00'"); 
            }
            if (!string.IsNullOrEmpty(entity.EndTime))
            {
                sbwhere.Append("and f.EndTime<='" + entity.EndTime + " 23:59:59'");
            }
            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        private SqlParameter CPOrder(searchFestivalEntity enity)
        {
            StringBuilder sborder = new StringBuilder();

            if (enity.OrderfieldType == OrderFieldType.Desc)
            {
                sborder.Append(" f.EndTime DESC ");
            }
            else
            {
                sborder.Append(" f.EndTime ASC ");
            }

            return new SqlParameter("@OrderField", sborder.ToString());
        }
        /// <summary>
        /// 设置显示字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private SqlParameter CPFields(searchFestivalEntity entity)
        {
            StringBuilder sbfileds = new StringBuilder();
            if (entity.UseDBPagination)
            {
                sbfileds.Append(@" f.FestivalID, f.FestivalName,f.FestivalShortName,convert(varchar(11),f.StartTime,120) as StartTime
                    ,convert(varchar(11),f.EndTime,120) as EndTime,f.FestivalType,f.FestivalWeight
                    ,e.EventItemGUID,e.Title");
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
        private SqlParameter CPTable(searchFestivalEntity entity)
        {
            StringBuilder sbtable = new StringBuilder();
            //基本表
            sbtable.Append(@" M_Festival AS f
                        LEFT JOIN dbo.M_EventItem AS e ON e.FestivalID = f.FestivalID");
            return new SqlParameter("@TableName", sbtable.ToString());
        }
        #endregion
        /// <summary>
        /// 删除日历信息
        /// </summary>
        /// <param name="FestivalID"></param>
        /// <returns></returns>
        public string deleteFestival(FestivalEntity entity)
        {
            SqlTransaction trans = null;
            string flg = "0";
            string sql = "delete  from M_Festival where FestivalID=@FestivalID";
            string sql1 = "update M_EventItem set FestivalID=@Festivalnullid,Title2=@Title2,StartTime2=@StartTime2,EndTime2=@EndTime2,EventItemFlag=@EventItemFlag where FestivalID=@FestivalID";
            string sql2 = "select EventItemID from M_EventItem where FestivalID=@FestivalID";
            SqlParameter[] pars = { new SqlParameter("FestivalID", SqlDbType.UniqueIdentifier) };
            pars[0].Value = entity.FestivalID;
            SqlParameter[] pars1 = { new SqlParameter("FestivalID", SqlDbType.UniqueIdentifier),
                                     new SqlParameter("Festivalnullid",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("Title2",SqlDbType.NVarChar,100),
                                     new SqlParameter("StartTime2",SqlDbType.DateTime),
                                     new SqlParameter("EndTime2",SqlDbType.DateTime),
                                     new SqlParameter("EventItemFlag",SqlDbType.Int)};
            pars1[0].Value = entity.FestivalID; 
            try
            { 
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    DataTable db = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql2, pars).Tables[0];
                    if (db.Rows.Count>0)
                    {
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql1, pars1);
                    } 
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, pars);
                    trans.Commit();
                    flg = "1";
                }
            }
            catch (Exception e)
            {
                flg = "0";
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(e);
            }
            return flg;
        }

        public DataTable GetFestival(FestivalEntity entity)
        {
            DataTable db = new DataTable();
            string sql = "select * from M_Festival where FestivalID=@FestivalID";
            SqlParameter[] pars = { new SqlParameter("FestivalID", SqlDbType.UniqueIdentifier) };
            pars[0].Value = entity.FestivalID;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    db = SQlHelper.ExecuteDataset(conn, CommandType.Text, sql, pars).Tables[0];
                }
            }
            catch (Exception e)
            {
                LogUtil.WriteLog(e);
            }
            return db;

        }
        public string UpdateFestival(FestivalEntity entity)
        {
            string flg = "0";
            string sql = "select * from M_Festival where FestivalName=@FestivalName and StartTime=@StartTime and EndTime=@EndTime and FestivalType=@FestivalType and FestivalID <> @FestivalID";
            string sql3 = "select FestivalName,Convert(varchar(20),StartTime,120) as StartTime,Convert(varchar(20),EndTime,120) as EndTime,FestivalType  from M_Festival where FestivalID=@FestivalID"; 
            string sql1 = "update M_Festival set FestivalName=@FestivalName,FestivalShortName=@FestivalShortName,FestivalType=@FestivalType,StartTime=@StartTime,EndTime=@EndTime,FestivalWeight=@FestivalWeight where FestivalID=@FestivalID";
            string sql2 = "update M_EventItem set Title2=@FestivalName,StartTime2=@StartTime,EndTime2=@EndTime where FestivalID=@FestivalID";
            string sql4 = "update M_EventItem set FestivalID=@Festivalnullid,Title2=@Title2,StartTime2=@StartTime2,EndTime2=@EndTime2,EventItemFlag=@EventItemFlag where FestivalID=@FestivalID";  
            SqlParameter[] pars1 = { new SqlParameter("FestivalID", SqlDbType.UniqueIdentifier),
                                     new SqlParameter("Festivalnullid",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("Title2",SqlDbType.NVarChar,100),
                                     new SqlParameter("StartTime2",SqlDbType.DateTime),
                                     new SqlParameter("EndTime2",SqlDbType.DateTime),
                                     new SqlParameter("EventItemFlag",SqlDbType.Int)};
            pars1[0].Value = entity.FestivalID; 
            SqlTransaction trans = null;
            SqlParameter[] pars = { new SqlParameter("FestivalName",SqlDbType.NVarChar,50),
                                  new SqlParameter("FestivalShortName",SqlDbType.NVarChar,50),
                                  new SqlParameter("StartTime",SqlDbType.DateTime),
                                  new SqlParameter("EndTime",SqlDbType.DateTime),
                                  new SqlParameter("FestivalType",SqlDbType.Int),
                                  new SqlParameter("FestivalWeight",SqlDbType.Int),
                                  new SqlParameter("@FestivalID",SqlDbType.UniqueIdentifier)};
            pars[0].Value = entity.FestivalName;
            pars[1].Value = entity.FestivalShortName;
            pars[2].Value = entity.StartTime;
            pars[3].Value = entity.EndTime;
            pars[4].Value = entity.FestivalType;
            pars[5].Value = entity.FestivalWeight;
            pars[6].Value = entity.FestivalID;
            using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    DataTable db = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql, pars).Tables[0];
                    if (db.Rows.Count == 0)  //无重复性 可以修改
                    {

                        DataTable db1 = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql3, pars).Tables[0];
                        if (db1.Rows.Count>0)
                        {
                            for (int i = 0; i < db1.Rows.Count; i++)  //修改前为日期，改后不为日期，M_EventItem表清空
                            {
                                if (db1.Rows[i]["FestivalType"].ToString().Trim().Equals("3"))
                                {
                                    if (db1.Rows[i]["FestivalType"].ToString().Trim().Equals("3") && entity.FestivalType != 3)
                                    {
                                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql4, pars1);
                                    }
                                    else if (!db1.Rows[i]["FestivalName"].ToString().Trim().Equals(entity.FestivalName) || !db1.Rows[i]["StartTime"].ToString().Trim().Equals(entity.StartTime) || !db1.Rows[i]["EndTime"].ToString().Trim().Equals(entity.EndTime))
                                    {
                                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql2, pars);
                                    } 
                                } 
                            } 
                        } 
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql1, pars);
                        flg = "1";
                        trans.Commit();
                    }
                    else
                    {
                        flg = "2";
                    }
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(e);
                    if (trans != null)
                    {
                        flg = "0";        //数据传输错误！
                        trans.Rollback();
                    }
                }
            }
            return flg;
        }

        /// <summary>
        /// 1班   2休   3节日
        /// </summary>
        /// <param name="fesivalType"></param>
        /// <returns></returns>
        public List<FestivalEntity> GetFestivalList(string fesivalName, int fesivalType=3)
        {
            try
            {
                string sql = "SELECT FestivalID,FestivalName,convert(varchar(10),StartTime,21) AS StartTime,convert(varchar(10),EndTime,21) AS EndTime,FestivalType FROM M_Festival WHERE FestivalType=@FestivalType AND FestivalName LIKE @FestivalName ORDER BY FestivalID DESC";

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<FestivalEntity>(sql, new { FestivalType = fesivalType, FestivalName =  "%" + fesivalName + "%" }).ToList();
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
