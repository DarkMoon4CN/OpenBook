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
     * 模块：用户提醒模块更新
     * 作用：提供发送短信时依照模板发送
     * 作者：
     * 时间：2015-12-11
     * 备注：UserRemindDAO将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class UserRemindDAO
    {
        public OperationResult<bool> UserRemind_Insert(UserRemindEntity  entity) 
        {
            SqlTransaction trans = null;
            try
            {
                string sql = string.Empty;
                sql += " INSERT INTO M_User_Remind(UserID,RemindTypeID,Data,UpdateTime)  ";
                sql += " VALUES(@UserID,@RemindTypeID,@Data,@UpdateTime)  ";

                SqlParameter[] prms = { 
                                      new SqlParameter("@UserID", SqlDbType.Int),
                                      new SqlParameter("@RemindTypeID", SqlDbType.Int),
                                      new SqlParameter("@Data",SqlDbType.NVarChar,128),
                                      new SqlParameter("@UpdateTime",SqlDbType.DateTime),

                                   };
                prms[0].Value = entity.UserID;
                prms[1].Value = entity.RemindTypeID;
                prms[2].Value = entity.Data;
                prms[3].Value = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                    trans.Commit();
                }
                return new OperationResult<bool>(OperationResultType.Success, "数据加入成功！", true);
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
                catch { }
                return new OperationResult<bool>(OperationResultType.Error, "异常结果："+ex.Message,false);
            }
            
        }

        public OperationResult<bool> UserRemind_Update(UserRemindEntity entity) 
        {
            SqlTransaction trans = null;
            try
            {
                string sql = string.Empty;
                sql += " UPDATE M_User_Remind SET Data=@Data,UpdateTime=@UpdateTime ";
                sql += " WHERE UserID=@UserID AND RemindTypeID=@RemindTypeID ";
                SqlParameter[] prms = { 
                                      new SqlParameter("@UserID", SqlDbType.Int),
                                      new SqlParameter("@RemindTypeID", SqlDbType.Int),
                                      new SqlParameter("@Data",SqlDbType.NVarChar,128),
                                      new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                                   };
                prms[0].Value = entity.UserID;
                prms[1].Value = entity.RemindTypeID;
                prms[2].Value = entity.Data;
                prms[3].Value = DateTime.Now;
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms);
                    trans.Commit();
                }
                return new OperationResult<bool>(OperationResultType.Success, "数据更新成功！", true);
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
                catch { }
                return new OperationResult<bool>(OperationResultType.Error, "异常结果:" + ex.Message, false);
            }
        }

        public OperationResult<IList<UserRemindEntity>> UserRemind_GetWhere(string strWhere = null)
        {
            IList<UserRemindEntity> items = new List<UserRemindEntity>();
            try
            {
                string sql = " SELECT * FROM M_User_Remind  WHERE  1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    items = con.Query<UserRemindEntity>(sql).ToList();
                    return new OperationResult<IList<UserRemindEntity>>(OperationResultType.Success, "数据完成查询", items);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<IList<UserRemindEntity>>(OperationResultType.Success, "异常结果：" + ex.Message);
            }
        }

        /// <summary>
        ///  此方法查询出提醒类型列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public OperationResult<IList<UserRemindTypeEntity>> UserRemindType_GetWhere(string strWhere = null) 
        {
            IList<UserRemindTypeEntity> items = new List<UserRemindTypeEntity>();
            try
            {
                string sql = " SELECT * FROM M_User_RemindType_Rel  WHERE  1=1 " + strWhere;
                sql += "  ORDER BY  RemindTypeID  ASC  ";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    items = con.Query<UserRemindTypeEntity>(sql).ToList();
                    return new OperationResult<IList<UserRemindTypeEntity>>(OperationResultType.Success, "数据完成查询", items);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<IList<UserRemindTypeEntity>>(OperationResultType.Success, "异常结果：" + ex.Message);
            }
        }
    }
}
