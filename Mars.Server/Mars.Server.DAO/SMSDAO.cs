using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
namespace Mars.Server.DAO
{
    public class SMSDAO
    {
        public IList<SMSEntity> SMS_GetALL(string strWhere = null)
        {
            IList<SMSEntity> items = new List<SMSEntity>();
            try
            {
                string sql = " SELECT * FROM M_SignSMS  WHERE  1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<SMSEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public DataTable SMS_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            DataTable table = null;
            try
            {
                List<SqlParameter> prmslist = new List<SqlParameter>();
                prmslist.Add(new SqlParameter("@TableName", " M_SignSMS "));

                prmslist.Add(new SqlParameter("@Fields", "*"));

                prmslist.Add(new SqlParameter("@OrderField", orderType));

                prmslist.Add(new SqlParameter("@sqlWhere", strWhere));

                prmslist.Add(new SqlParameter("@pageIndex", pageIndex));

                prmslist.Add(new SqlParameter("@pageSize", pageSize));

                prmslist.Add(new SqlParameter() { ParameterName = "@Records", Value = 0, Direction = ParameterDirection.Output });

                SqlParameter[] prms = prmslist.ToArray();

                table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "sp_pager06", prms).Tables[0];
                recordCount = Convert.ToInt32(prms[prms.Length - 1].Value);

            }
            catch (Exception ex)
            {
                recordCount = 0;
                LogUtil.WriteLog(ex);
                return null;
            }
            return table;
        }

        public bool SMS_Insert(SMSEntity entity)
        {
            bool isSuccess = false;
            SqlTransaction trans = null;
            string sql = string.Empty;
            sql += " INSERT INTO M_SignSMS(Customer,Moblie,IsSend,Content,SendTime,SysUserID) ";
            sql += "             VALUES(@Customer,@Moblie,@IsSend,@Content,@SendTime,@SysUserID)";

            SqlParameter[] prms = { 
                                      new SqlParameter("@Customer", SqlDbType.NVarChar,128),
                                      new SqlParameter("@Moblie", SqlDbType.NVarChar,64),
                                      new SqlParameter("@IsSend",SqlDbType.Int),
                                      new SqlParameter("@Content",SqlDbType.NVarChar,1024),
                                      new SqlParameter("@SendTime",SqlDbType.NVarChar,48),
                                      new SqlParameter("@SysUserID",SqlDbType.Int),
                                      new SqlParameter("@ModelKey",SqlDbType.VarChar,24)
                                   };
            prms[0].Value = entity.Customer;
            prms[1].Value = entity.Moblie;
            prms[2].Value = entity.IsSend;
            prms[3].Value = entity.Content;
            prms[4].Value = entity.SendTime;
            prms[5].Value = entity.SysUserID;
            prms[6].Value = entity.ModelKey;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                    isSuccess = true;
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                }
                catch 
                {
                    LogUtil.WriteLog(ex);
                    return false;
                }
                
                return false;
            }
            return isSuccess;
        }

        public bool SMS_Update(string content, int isSend,string modelKey,int smsID)
        {
            try
            {
                string sql = " UPDATE M_SignSMS  SET Content='{0}',IsSend={1},SendTime='{2}',ModelKey='{3}' WHERE  SmsID='{4}' ";
                sql = string.Format(sql, content, isSend, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), modelKey, smsID);
                return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }


        public bool SMS_Delete(int smsID)
        {
            try
            {
                string sql = " DELETE FROM M_SignSMS WHERE  SmsID={0} ";
                sql = string.Format(sql, smsID);
                return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        
        }


        /// <summary>
        /// 按条件删除 谨慎使用
        /// </summary>
        /// <param name="isDelete">判定是否删除,设定为使调用者注意</param>
        /// <param name="sysUserId">系统操作员</param>
        /// <param name="IsSend">发送状态</param>
        /// <returns></returns>
        public bool SMS_Delete(int sysUserID, int isSend = 0, bool isDelete=false)
        {
            if (isDelete)
            {
                try
                {
                    string sql = " DELETE FROM M_SignSMS WHERE 1=1  AND SysUserID ={0} AND isSend={1} ";
                    sql = string.Format(sql, sysUserID,isSend);
                    return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return true;
                }
            }
            else //执行无实际含义
            {
                return true;
            }

            

        }
    }
}
