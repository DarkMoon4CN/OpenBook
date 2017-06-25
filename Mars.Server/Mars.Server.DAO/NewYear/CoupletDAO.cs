using Mars.Server.Entity.NewYear;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.NewYear
{
    public class CoupletDAO
    {
        public DataTable GetCoupletTable() {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM NY_Couplet
                    WHERE StateTypeID = 1 AND CoupletTypeID = 1
                    ORDER BY OrderBy ASC");

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

        public DataTable GetCoupletTable(int cid)
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM NY_Couplet
                    WHERE StateTypeID = 1 AND CoupletID={0}
                    ORDER BY OrderBy ASC"
                ,cid.ToString());

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
        /// 添加一条
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        public int Add(string up, string down, string horizontal)
        {
            int returnValue = 0;

            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                con.Open();
                trans = con.BeginTransaction();

                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"SELECT MAX(CoupletID) AS MaxID FROM dbo.NY_Couplet");

                    object obj = SQlHelper.ExecuteScalar(trans, CommandType.Text, strSql.ToString(), null);
                    if (obj != null)
                    {
                        returnValue = obj.ToInt()+1;

                        StringBuilder strCouplet = new StringBuilder();
                        strCouplet.AppendFormat(@"INSERT INTO dbo.NY_Couplet( CoupletID ,CoupletContentTypeID ,CoupletContent ,OrderBy ,StateTypeID ,CoupletTypeID)
                            VALUES  ( {0} ,0 ,'{1}' ,1000 ,1 ,0);
                            INSERT INTO dbo.NY_Couplet( CoupletID ,CoupletContentTypeID ,CoupletContent ,OrderBy ,StateTypeID ,CoupletTypeID)
                            VALUES  ( {0} ,1 ,'{2}' ,1000 ,1 ,0);
                            INSERT INTO dbo.NY_Couplet( CoupletID ,CoupletContentTypeID ,CoupletContent ,OrderBy ,StateTypeID ,CoupletTypeID)
                            VALUES  ( {0} ,2 ,'{3}' ,1000 ,1 ,0);"
                            , returnValue.ToString(), up, down, horizontal);

                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strCouplet.ToString(), null);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    returnValue = 0;
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }

            return returnValue;
        }

        public DataTable GetFuImageTable()
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT * FROM NY_FuImages ");

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

        public DataTable GetFuImageTable(int iId)
        {
            DataTable table = null;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@" SELECT * FROM NY_FuImages where ImageID = {0} "
                            ,iId.ToString());

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

        public int AddShareLog(ShareLogEntity item) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into NY_ShareLog(");
            strSql.Append("CreateTime,ShareUserID,MachineCode,ShareTypeID,SystemName,Verson,IPAddress,ExInfo,CoupletID,ImageID,IsView)");
            strSql.Append(" values (");
            strSql.Append("@CreateTime,@ShareUserID,@MachineCode,@ShareTypeID,@SystemName,@Verson,@IPAddress,@ExInfo,@CoupletID,@ImageID,@IsView)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@ShareUserID", SqlDbType.Int,4),
                    new SqlParameter("@MachineCode", SqlDbType.VarChar,100),
                    new SqlParameter("@ShareTypeID", SqlDbType.Int,4),
                    new SqlParameter("@SystemName", SqlDbType.VarChar,20),
                    new SqlParameter("@Verson", SqlDbType.VarChar,10),
                    new SqlParameter("@IPAddress", SqlDbType.VarChar,20),
                    new SqlParameter("@ExInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@CoupletID", SqlDbType.Int,4),
                    new SqlParameter("@ImageID", SqlDbType.Int,4),
                    new SqlParameter("@IsView", SqlDbType.Int,4)};
            parameters[0].Value = item.CreateTime;
            parameters[1].Value = item.ShareUserID;
            parameters[2].Value = item.MachineCode;
            parameters[3].Value = item.ShareTypeID;
            parameters[4].Value = item.SystemName;
            parameters[5].Value = item.Verson;
            parameters[6].Value = item.IPAddress;
            parameters[7].Value = item.ExInfo;
            parameters[8].Value = item.CoupletID;
            parameters[9].Value = item.ImageID;
            parameters[10].Value = item.IsView;

            try
            {
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
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 返回分享总数
        /// </summary>
        /// <returns></returns>
        public int GetShareCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT COUNT(0) AS Cnt FROM NY_ShareLog WITH(NOLOCK)");

            object obj = SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
    }

}
