using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Mars.Server.Entity.Exhibition;
using System.Data.SqlClient;
using Mars.Server.Utils;

namespace Mars.Server.DAO.Exhibition
{
    public class ExhibitorDAO
    {
        /// <summary>
        /// 获取所有展会展位信息
        /// </summary>
        /// <param name="exhibitionID"></param>
        /// <returns></returns>
        public DataTable GetExhibitorLocation(int exhibitionID)
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT el.* FROM dbo.M_Exhibition_Exhibitor_Location AS el
                    INNER JOIN dbo.M_Exhibition_Exhibitors AS ee ON ee.ExhibitorID = el.ExhibitorID
                    WHERE el.StateTypeID = 1 AND ee.StateTypeID =1 AND ee.ExhibitionID = {0}"
                    , exhibitionID.ToString());

            DataSet ds = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
            if (ds != null && ds.Tables.Count>0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    table = ds.Tables[0];
                }
            }

            return table;
        }
        /// <summary>
        /// 修改是否存在书目信息
        /// </summary>
        /// <param name="exhibitorID"></param>
        /// <param name="isHadBookList"></param>
        /// <returns></returns>
        public bool ChangeExhibitorIsHadBookList(int exhibitorID, int isHadBookList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE M_Exhibition_Exhibitors SET IsHadBookList ={1}  WHERE ExhibitorID = {0}"
                , exhibitorID.ToString()
                , isHadBookList.ToString());

            try {
                return SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0;
            } catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public DataSet GetExhibitorDataSet(int exhibitionID) {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT e.* FROM dbo.M_Exhibition_Exhibitors AS e
                INNER JOIN dbo.M_Exhibition_Main AS m ON m.ExhibitionID = e.ExhibitionID
                WHERE e.ExhibitionID={0} AND e.StateTypeID = 1 AND m.StateTypeID=1
                ORDER BY e.ExhibitorPinYin ASC;

                SELECT el.* FROM dbo.M_Exhibition_Exhibitor_Location AS el
                INNER JOIN dbo.M_Exhibition_Exhibitors AS e ON e.ExhibitorID = el.ExhibitorID
                INNER JOIN dbo.M_Exhibition_Main AS m ON m.ExhibitionID = e.ExhibitionID
                WHERE el.StateTypeID=1 AND e.ExhibitionID ={0} AND e.StateTypeID = 1 AND m.StateTypeID=1
                ORDER BY el.ExhibitiorLocationOrder ASC"
                , exhibitionID.ToString());

            return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr,CommandType.Text,strSql.ToString(),null);
        }

        public bool ImportExhibitors(DataTable table, int exhibitionID, string userid)
        {
            bool returnValue = false;
            if (table != null)
            {
                if (table.Columns.Contains("展商名称") && table.Columns.Contains("展位位置"))
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("DECLARE @key INT;");
                    foreach (DataRow dr in table.Rows)
                    {
                        strSql.AppendFormat(@"INSERT INTO dbo.M_Exhibition_Exhibitors( ExhibitionID ,ExhibitorName ,ExhibitorPinYin ,OBCustomerID ,OBCustomerTypeID ,IsHadBookList ,StateTypeID ,CreateUserID ,CreateTime)
                            VALUES  ( {0} ,'{1}' ,'{2}' ,0 ,0 , 0 ,1 , '{3}' , GETDATE());
                            select @key = @@IDENTITY;"
                                , exhibitionID.ToString()
                                , dr["展商名称"].ToString()
                                , Hz2Py.GetWholePinyin(dr["展商名称"].ToString())
                                , userid);
                        string[] locs = dr["展位位置"].ToString().Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                        if (locs != null)
                        {
                            foreach (string loc in locs)
                            {
                                strSql.AppendFormat(@"INSERT INTO dbo.M_Exhibition_Exhibitor_Location( ExhibitorID ,ExhibitorLocation ,StateTypeID ,ExhibitiorLocationOrder ,CreateUserID ,CreateTime)
                                        VALUES  ( @key ,'{0}' , 1 , 1000 ,'{1}' ,GETDATE());"
                                , loc
                                , userid);
                            }
                        }
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

        public DataTable QueryData(ExhibitorSearchEntity info, out int totalcnt)
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
        protected virtual List<SqlParameter> ParseToSqlParameters(ExhibitorSearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(ExhibitorSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@"dbo.M_Exhibition_Exhibitors AS ee
                INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = ee.ExhibitionID");
            
            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(ExhibitorSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ee.ExhibitorPinYin ASC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(ExhibitorSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"em.ExhibitionID
                ,ee.ExhibitorID
                ,ee.ExhibitorName 
                ,ee.ExhibitorPinYin
                ,ee.IsHadBookList");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(ExhibitorSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND em.StateTypeID = 1 AND ee.StateTypeID = 1 ");

            if (sp.ExhibitionID > 0)
            {
                sbwhere.AppendFormat(" AND em.ExhibitionID = {0}", sp.ExhibitionID);
            }

            if (!string.IsNullOrEmpty(sp.ExhibitorName))
            {
                sbwhere.AppendFormat(@" AND (ee.ExhibitorName LIKE '%{0}%' 
                    OR ee.ExhibitorPinYin LIKE '%{0}%') ", sp.ExhibitorName);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
        /// <summary>
        /// 删除展商 假删除
        /// </summary>
        /// <param name="exhibitorID"></param>
        /// <returns></returns>
        public bool DeleteExhibitor(int exhibitorID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE M_Exhibition_Exhibitors SET StateTypeID = 0  WHERE ExhibitorID = {0}"
                , exhibitorID.ToString());

            try
            {
                return SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public DataTable QueryDataConsole(ExhibitorSearchEntity info, out int totalcnt)
        {
            try
            {
                SqlParameter[] prms = ParseToSqlParameters_Console(info).ToArray();
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
        protected virtual List<SqlParameter> ParseToSqlParameters_Console(ExhibitorSearchEntity sp)
        {
            List<SqlParameter> _parameters = new List<SqlParameter>();
            _parameters.Add(CreateParameter_Table_Console(sp));
            _parameters.Add(CreateParameter_Fileds_Console(sp));
            _parameters.Add(CreateParamter_Orderby_Console(sp));
            _parameters.Add(CreateParameter_Where_Console(sp));

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
        protected virtual SqlParameter CreateParameter_Table_Console(ExhibitorSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@" dbo.M_Exhibition_Exhibitors AS ee
                INNER JOIN dbo.M_Exhibition_Main AS em ON em.ExhibitionID = ee.ExhibitionID ");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby_Console(ExhibitorSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ee.ExhibitorPinYin ASC ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds_Console(ExhibitorSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@" ee.* ");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where_Console(ExhibitorSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND em.StateTypeID = 1 AND ee.StateTypeID = 1 ");

            if (sp.ExhibitionID > 0)
            {
                sbwhere.AppendFormat(" AND em.ExhibitionID = {0}", sp.ExhibitionID);
            }

            if (!string.IsNullOrEmpty(sp.ExhibitorName))
            {
                sbwhere.AppendFormat(@" AND (ee.ExhibitorName LIKE '%{0}%' 
                    OR ee.ExhibitorPinYin LIKE '%{0}%') ", sp.ExhibitorName);
            }

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion

        /// <summary>
        /// 添加新展商
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add_Exhibitor(ExhibitorEntity item)
        {
            int returnValue = 0;

            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into M_Exhibition_Exhibitors(");
                    strSql.Append("ExhibitionID,ExhibitorName,ExhibitorPinYin,OBCustomerID,OBCustomerTypeID,IsHadBookList,StateTypeID,CreateUserID,CreateTime)");
                    strSql.Append(" values (");
                    strSql.Append("@ExhibitionID,@ExhibitorName,@ExhibitorPinYin,@OBCustomerID,@OBCustomerTypeID,@IsHadBookList,@StateTypeID,@CreateUserID,@CreateTime)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitorName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ExhibitorPinYin", SqlDbType.NVarChar,500),
                    new SqlParameter("@OBCustomerID", SqlDbType.Int,4),
                    new SqlParameter("@OBCustomerTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsHadBookList", SqlDbType.Bit,1),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                    parameters[0].Value = item.ExhibitionID;
                    parameters[1].Value = item.ExhibitorName;
                    parameters[2].Value = item.ExhibitorPinYin;
                    parameters[3].Value = item.OBCustomerID;
                    parameters[4].Value = item.OBCustomerTypeID;
                    parameters[5].Value = item.IsHadBookList;
                    parameters[6].Value = item.StateTypeID;
                    parameters[7].Value = item.CreateUserID;
                    parameters[8].Value = item.CreateTime;

                    object objrec = SQlHelper.ExecuteScalar(trans, CommandType.Text, strSql.ToString(), parameters);
                    if (objrec != null)
                    {
                        //写入位置
                        if (item.ExhibitorLocationList != null)
                        {
                            foreach (ExhibitorLocationEntity _loc in item.ExhibitorLocationList)
                            {
                                StringBuilder strLocSql = new StringBuilder();
                                strLocSql.Append("insert into M_Exhibition_Exhibitor_Location(");
                                strLocSql.Append("ExhibitorID,ExhibitorLocation,StateTypeID,ExhibitiorLocationOrder,CreateUserID,CreateTime)");
                                strLocSql.Append(" values (");
                                strLocSql.Append("@ExhibitorID,@ExhibitorLocation,@StateTypeID,@ExhibitiorLocationOrder,@CreateUserID,@CreateTime)");
                                strLocSql.Append(";select @@IDENTITY");
                                SqlParameter[] locparameters = {
                                new SqlParameter("@ExhibitorID", SqlDbType.Int,4),
                                new SqlParameter("@ExhibitorLocation", SqlDbType.NVarChar,100),
                                new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                                new SqlParameter("@ExhibitiorLocationOrder", SqlDbType.Int,4),
                                new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                                new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                                locparameters[0].Value = int.Parse(objrec.ToString());
                                locparameters[1].Value = _loc.ExhibitorLocation;
                                locparameters[2].Value = _loc.StateTypeID;
                                locparameters[3].Value = _loc.ExhibitiorLocationOrder;
                                locparameters[4].Value = _loc.CreateUserID;
                                locparameters[5].Value = _loc.CreateTime;

                                SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strLocSql.ToString(), locparameters);
                            }
                        }

                        trans.Commit();
                        returnValue = int.Parse(objrec.ToString());
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                }
            }

            return returnValue;
        }
        /// <summary>
        /// 更新展商
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update_Exhibitor(ExhibitorEntity item)
        {
            bool returnValue = false;
            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    //清除位置记录
                    StringBuilder strDelLocSql = new StringBuilder();
                    strDelLocSql.AppendFormat(@"DELETE FROM dbo.M_Exhibition_Exhibitor_Location WHERE ExhibitorID = {0}", item.ExhibitorID.ToString());
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strDelLocSql.ToString(), null);

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update M_Exhibition_Exhibitors set ");
                    strSql.Append("ExhibitionID=@ExhibitionID,");
                    strSql.Append("ExhibitorName=@ExhibitorName,");
                    strSql.Append("ExhibitorPinYin=@ExhibitorPinYin,");
                    strSql.Append("OBCustomerID=@OBCustomerID,");
                    strSql.Append("OBCustomerTypeID=@OBCustomerTypeID,");
                    strSql.Append("IsHadBookList=@IsHadBookList,");
                    strSql.Append("StateTypeID=@StateTypeID");
                    strSql.Append(" where ExhibitorID=@ExhibitorID");
                    SqlParameter[] parameters = {
                    new SqlParameter("@ExhibitionID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitorName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ExhibitorPinYin", SqlDbType.NVarChar,500),
                    new SqlParameter("@OBCustomerID", SqlDbType.Int,4),
                    new SqlParameter("@OBCustomerTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsHadBookList", SqlDbType.Bit,1),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ExhibitorID", SqlDbType.Int,4)};
                    parameters[0].Value = item.ExhibitionID;
                    parameters[1].Value = item.ExhibitorName;
                    parameters[2].Value = item.ExhibitorPinYin;
                    parameters[3].Value = item.OBCustomerID;
                    parameters[4].Value = item.OBCustomerTypeID;
                    parameters[5].Value = item.IsHadBookList;
                    parameters[6].Value = item.StateTypeID;
                    parameters[7].Value = item.ExhibitorID;

                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql.ToString(), parameters);
                    
                    //写入位置
                    if (item.ExhibitorLocationList != null)
                    {
                        foreach (ExhibitorLocationEntity _loc in item.ExhibitorLocationList)
                        {
                            StringBuilder strLocSql = new StringBuilder();
                            strLocSql.Append("insert into M_Exhibition_Exhibitor_Location(");
                            strLocSql.Append("ExhibitorID,ExhibitorLocation,StateTypeID,ExhibitiorLocationOrder,CreateUserID,CreateTime)");
                            strLocSql.Append(" values (");
                            strLocSql.Append("@ExhibitorID,@ExhibitorLocation,@StateTypeID,@ExhibitiorLocationOrder,@CreateUserID,@CreateTime)");
                            strLocSql.Append(";select @@IDENTITY");
                            SqlParameter[] locparameters = {
                            new SqlParameter("@ExhibitorID", SqlDbType.Int,4),
                            new SqlParameter("@ExhibitorLocation", SqlDbType.NVarChar,100),
                            new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                            new SqlParameter("@ExhibitiorLocationOrder", SqlDbType.Int,4),
                            new SqlParameter("@CreateUserID", SqlDbType.VarChar,20),
                            new SqlParameter("@CreateTime", SqlDbType.DateTime)};
                            locparameters[0].Value = item.ExhibitorID;
                            locparameters[1].Value = _loc.ExhibitorLocation;
                            locparameters[2].Value = _loc.StateTypeID;
                            locparameters[3].Value = _loc.ExhibitiorLocationOrder;
                            locparameters[4].Value = _loc.CreateUserID;
                            locparameters[5].Value = _loc.CreateTime;

                            SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strLocSql.ToString(), locparameters);
                        }
                        
                        trans.Commit();
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                }
            }

            return returnValue;
        }

        public DataSet GetExhibitorEntityDataSet(int exhibitorID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT e.*,0 as rowId FROM dbo.M_Exhibition_Exhibitors AS e
                WHERE e.ExhibitorID={0};

                SELECT el.* FROM dbo.M_Exhibition_Exhibitor_Location AS el
                WHERE el.ExhibitorID={0}"
                , exhibitorID.ToString());

            return SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
        }
    }
}
