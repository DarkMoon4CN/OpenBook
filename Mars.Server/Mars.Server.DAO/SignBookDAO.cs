using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
namespace Mars.Server.DAO
{
    public class SignBookDAO
    {
        public bool IsAllowCustomerKey(string customerKey) 
        {   
            try
            {
                string sql = " SELECT COUNT(1) FROM M_SignBook WHERE  CustomerKey='{0}' ";
                sql = string.Format(sql,customerKey);
                return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        public IList<SignBookEntity> SignBook_Get(string customerKey) 
        {
            IList<SignBookEntity> items = new List<SignBookEntity>();
            try
            {
                string sql = " SELECT * FROM M_SignBook WHERE  CustomerKey='{0}' ";
                sql = string.Format(sql, customerKey);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<SignBookEntity>(sql, new { CustomerKey = customerKey }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public IList<SignBookEntity> SignBook_Get(int signID) 
        {
            IList<SignBookEntity> items = new List<SignBookEntity>();
            try
            {
                string sql = " SELECT * FROM M_SignBook WHERE  SignID={0} ";
                sql = string.Format(sql, signID);
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<SignBookEntity>(sql, new { SignID=signID }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public bool SignBook_Update(string customerKey, int state) 
        {
            try
            {
                string sql = " UPDATE M_SignBook  SET IsSign={0} WHERE  CustomerKey='{1}' ";
                sql = string.Format(sql,state ,customerKey);
                return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        public bool SignBook_Update(SignBookEntity entity) 
        {
            try
            {
                string sql = " UPDATE M_SignBook  SET  Company='{0}',Department='{1}',Position='{2}',Email='{3}' WHERE  SignID='{4}' ";
                sql = string.Format(sql, entity.Company, entity.Department,entity.Position,entity.Email,entity.SignID);
                return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        public DataTable SignBook_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            DataTable table = null;
            try
            {
                List<SqlParameter> prmslist = new List<SqlParameter>();
                prmslist.Add(new SqlParameter("@TableName", " M_SignBook "));

                prmslist.Add(new SqlParameter("@Fields", "*"));

                prmslist.Add(new SqlParameter("@OrderField", orderType));

                prmslist.Add(new SqlParameter("@sqlWhere", strWhere));

                prmslist.Add(new SqlParameter("@pageIndex", pageIndex));

                prmslist.Add(new SqlParameter("@pageSize",pageSize));

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

        public bool SignBook_Delete(int signID) 
        {
            try
            {
                string sql = " DELETE FROM M_SignBook WHERE  SignID='{0}' ";
                sql = string.Format(sql, signID);
                return int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        public bool IsExistLuckyNumber(string luckyNumber) 
        {
            try
            {
                string sql = " SELECT COUNT(1) FROM M_SignBook WHERE  LuckyNumber='{0}' ";
                sql = string.Format(sql, luckyNumber);
                return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        public IList<SignBookEntity> SignBook_GetALL(string strWhere=null) 
        {
            IList<SignBookEntity> items = new List<SignBookEntity>();
            try
            {
                string sql = " SELECT *,(SELECT Count(1) FROM M_user WHERE   LoginName=Moblie OR  Telphone=Moblie) AS IsRegister  FROM M_SignBook  WHERE 1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<SignBookEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        

        public bool SignBook_Insert(SignBookEntity entity) 
        {
            bool isSuccess = false;
            SqlTransaction trans = null;
            string sql = string.Empty;
            sql += " INSERT INTO M_SignBook(Customer,Moblie,Company,Department,Position,LuckyNumber,IsSign,Remarks,CustomerKey,SalesName,SignURL,SalesDepartment,CreateTime,Email) ";
            sql += "             VALUES(@Customer,@Moblie,@Company,@Department,@Position,@LuckyNumber,@IsSign,@Remarks,@CustomerKey,@SalesName,@SignURL,@SalesDepartment,@CreateTime,@Email)";

            SqlParameter[] prms = { 
                                      new SqlParameter("@Customer", SqlDbType.NVarChar,128),
                                      new SqlParameter("@Moblie", SqlDbType.NVarChar,64),
                                      new SqlParameter("@Company",SqlDbType.NVarChar,128),
                                      new SqlParameter("@Department",SqlDbType.NVarChar,64),
                                      new SqlParameter("@Position",SqlDbType.NVarChar,64), 
                                      new SqlParameter("@LuckyNumber",SqlDbType.NVarChar,20),

                                      new SqlParameter("@IsSign", SqlDbType.Int),
                                      new SqlParameter("@Remarks", SqlDbType.NVarChar,256),
                                      new SqlParameter("@CustomerKey",SqlDbType.NVarChar,128),
                                      new SqlParameter("@SalesName",SqlDbType.NVarChar,64),
                                      new SqlParameter("@SignURL",SqlDbType.NVarChar,256), 
                                      new SqlParameter("@SalesDepartment",SqlDbType.NVarChar,64),

                                      new SqlParameter("@CreateTime",SqlDbType.DateTime), 
                                      new SqlParameter("@Email",SqlDbType.NVarChar,256),
                                   };

            prms[0].Value = entity.Customer;
            prms[1].Value = entity.Moblie;
            prms[2].Value = entity.Company;
            prms[3].Value = entity.Department;
            prms[4].Value = entity.Position;
            prms[5].Value = entity.LuckyNumber;

            prms[6].Value = entity.IsSign;
            prms[7].Value = entity.Remarks;
            prms[8].Value = entity.CustomerKey;
            prms[9].Value = entity.SalesName;
            prms[10].Value = entity.SignURL;
            prms[11].Value = entity.SalesDepartment;

            prms[12].Value = entity.CreateTime;
            prms[13].Value = entity.Email;
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
                if (trans != null)
                {
                    trans.Rollback();
                }

                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        public int SignBook_Count(string strWhere = null) 
        {
            try
            {
                string sql = " SELECT COUNT(1) FROM M_SignBook WHERE 1=1 " +strWhere;
                return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString());
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return 0;
            }
        }
    }
}
