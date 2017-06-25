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
    /*
    * 模块：签到短信模板 
    * 表名：M_Exhibition_BookList_Customer_Rel
    * 作用：提供展会表的Email发送操作
    * 作者：
    * 时间：2015-11-30
    * 备注：ExhibitionEmailDAO将引用OperationResult.cs 作为返回数据结果承载体
    */
    public class ExhibitionEmailDAO
    {
        public OperationResult<bool> ExhibitionEmail_Insert(ExhibitionEmailEntity entity)
        {
            SqlTransaction trans = null;
            string sql = string.Empty;
            sql += " INSERT INTO M_Exhibition_BookList_Customer_Rel(ExhibitionID,CustomerID,CustomerToken,CustomerEmail,CreateTime,SendTypeID,SendTime,CustomerName) ";
            sql += "             VALUES(@ExhibitionID,@CustomerID,@CustomerToken,@CustomerEmail,@CreateTime,@SendTypeID,@SendTime,@CustomerName)";

            SqlParameter[] prms = { 
                                      new SqlParameter("@ExhibitionID", SqlDbType.Int),
                                      new SqlParameter("@CustomerID", SqlDbType.Int),
                                      new SqlParameter("@CustomerToken",SqlDbType.NVarChar,64),
                                      new SqlParameter("@CustomerEmail",SqlDbType.NVarChar,64), 
                                      new SqlParameter("@CreateTime",SqlDbType.DateTime),
                                      new SqlParameter("@SendTypeID", SqlDbType.Int),
                                      new SqlParameter("@SendTime", SqlDbType.DateTime),
                                      new SqlParameter("@CustomerName", SqlDbType.NVarChar,64)
                                   };

            prms[0].Value = entity.ExhibitionID;
            prms[1].Value = entity.CustomerID;
            prms[2].Value = entity.CustomerToken;
            prms[3].Value = entity.CustomerEmail;
            prms[4].Value = entity.CreateTime;
            prms[5].Value = entity.SendTypeID;
            prms[6].Value = entity.SendTime;
            prms[7].Value = entity.CustomerName;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                    trans.Commit();
                    return new OperationResult<bool>(OperationResultType.Success, "数据完成查询", true);
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch(Exception  tranex)
                    {
                        LogUtil.WriteLog(tranex);
                    }
                }
                return new OperationResult<bool>(OperationResultType.Error, ex.Message,false);
            }
           
        }

        public OperationResult<IList<ExhibitionEmailEntity>> ExhibitionEmail_GetWhere(string strWhere = null)
        {
            IList<ExhibitionEmailEntity> items = new List<ExhibitionEmailEntity>();
            try
            {
                string sql = " SELECT * FROM M_Exhibition_BookList_Customer_Rel  WHERE  1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    items = con.Query<ExhibitionEmailEntity>(sql).ToList();
                    return new OperationResult<IList<ExhibitionEmailEntity>>(OperationResultType.Success, "数据完成查询", items);
                }
            }
            catch(Exception ex)
            {
                  return new OperationResult<IList<ExhibitionEmailEntity>>(OperationResultType.Error,ex.Message);
            }
        }

        public OperationResult<bool> ExhibitionEmail_UpdateSendTypeID(int bookListCustomerID, int sendTypeID)
        {
            try
            {
                string sql = " UPDATE M_Exhibition_BookList_Customer_Rel  SET SendTypeID={0} WHERE  BookListCustomerID={1} ";
                sql = string.Format(sql, sendTypeID, bookListCustomerID);
                bool state= int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
                return new OperationResult<bool>(OperationResultType.Success, "数据完成查询", state);
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(OperationResultType.Error, ex.Message, false);
            }
        }


        public OperationResult<bool> ExhibitionEmail_UpdateSendTypeID(string customerEmail, int sendTypeID)
        {
            try
            {
                string sql = " UPDATE M_Exhibition_BookList_Customer_Rel  SET SendTypeID={0} WHERE  CustomerEmail='{1}' ";
                sql = string.Format(sql, sendTypeID, customerEmail);
                bool state = int.Parse(SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString()) > 0;
                return new OperationResult<bool>(OperationResultType.Success, "数据完成查询", state);
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(OperationResultType.Error, ex.Message, false);
            }
        }

    }
}
