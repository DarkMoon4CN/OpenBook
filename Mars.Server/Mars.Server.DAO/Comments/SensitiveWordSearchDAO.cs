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
    public class SensitiveWordSearchDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryData(SensitiveWordSearchEntity info, out int totalcnt)
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

        public bool ChangeStateType(int swID, int stateTypeID)
        {
            bool returnValue = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE M_System_SensitiveWords SET StateTypeID = {1} WHERE SWID = {0}"
                , swID.ToString()
                , stateTypeID.ToString());

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

        public int AddSensitiveWord(SensitiveWordEntity item)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into M_System_SensitiveWords(");
            strSql.Append("SensitiveWords,StateTypeID,IsNeedRecheck,CreateUserID,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@SensitiveWords,@StateTypeID,@IsNeedRecheck,@CreateUserID,@CreateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SensitiveWords", SqlDbType.NVarChar,100),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsNeedRecheck", SqlDbType.Bit,1),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = item.SensitiveWords;
            parameters[1].Value = item.StateTypeID;
            parameters[2].Value = item.IsNeedRecheck;
            parameters[3].Value = item.CreateUserID;
            parameters[4].Value = item.CreateTime;

            object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public bool UpdateSensitiveWord(SensitiveWordEntity item)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update M_System_SensitiveWords set ");
            strSql.Append("SensitiveWords=@SensitiveWords,");
            strSql.Append("StateTypeID=@StateTypeID,");
            strSql.Append("IsNeedRecheck=@IsNeedRecheck,");
            strSql.Append("CreateUserID=@CreateUserID,");
            strSql.Append("CreateTime=@CreateTime");
            strSql.Append(" where SWID=@SWID");
            SqlParameter[] parameters = {
                    new SqlParameter("@SensitiveWords", SqlDbType.NVarChar,100),
                    new SqlParameter("@StateTypeID", SqlDbType.Int,4),
                    new SqlParameter("@IsNeedRecheck", SqlDbType.Bit,1),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SWID", SqlDbType.Int,4)};
            parameters[0].Value = item.SensitiveWords;
            parameters[1].Value = item.StateTypeID;
            parameters[2].Value = item.IsNeedRecheck;
            parameters[3].Value = item.CreateUserID;
            parameters[4].Value = item.CreateTime;
            parameters[5].Value = item.SWID;

            if (SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 构造SQL分页参数
        /// <summary>
        /// 核心方法。查询条件转换成sql参数
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual List<SqlParameter> ParseToSqlParameters(SensitiveWordSearchEntity sp)
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
        protected virtual SqlParameter CreateParameter_Table(SensitiveWordSearchEntity sp)
        {
            StringBuilder sbtable = new StringBuilder();
            sbtable.Append(@" M_System_SensitiveWords ");

            return new SqlParameter("@TableName", sbtable.ToString());
        }

        protected virtual SqlParameter CreateParamter_Orderby(SensitiveWordSearchEntity sp)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SWID desc ");

            return new SqlParameter("@OrderField", sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Fileds(SensitiveWordSearchEntity sp)
        {
            StringBuilder sbfileds = new StringBuilder();
            sbfileds.Append(@"* ");
            return new SqlParameter("@Fields", sbfileds.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        protected virtual SqlParameter CreateParameter_Where(SensitiveWordSearchEntity sp)
        {
            StringBuilder sbwhere = new StringBuilder(" 1=1 AND StateTypeID = 1");

            return new SqlParameter("@sqlWhere", sbwhere.ToString());
        }

        #endregion
    }
}
