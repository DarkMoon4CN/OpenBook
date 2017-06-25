using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO
{
    public class MobileMessageDAO
    {
        /// <summary>
        /// 获取 Message总数
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public OperationResult<int> MobileMessage_Total(string strWhere=null)
        {
            try 
            {
                
                string sql = string.Empty;
                sql = " SELECT COUNT(1) FROM [dbo].[M_MobileMessages]  WHERE 1=1 {0} ";
                sql = string.Format(sql, strWhere);
                int total= int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql).ToString());

                return new OperationResult<int>(OperationResultType.Success, "与我相关总数获取成功！", total);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<int>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        #region 与我相关
        /// <summary>
        ///  获取与我先关的 数据信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="messageID">消息ID</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public OperationResult<List<RelateEntity>> Relate_GetList(int userId, int messageID, int pageIndex, int pageSize) 
        {
            try
            {

                List<RelateEntity> entitys = new List<RelateEntity>();
                SqlParameter[] parms =
                            { 
                                new SqlParameter("@UserID",userId),
                                new SqlParameter("@MessageID",messageID),
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_Relate_GetList", parms).Tables[0];
                entitys = ConvertDataTable<RelateEntity>.ConvertToList(table);
                return new OperationResult<List<RelateEntity>>(OperationResultType.Success, "与我相关数据获取成功！", entitys);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<List<RelateEntity>>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        /// 将用户下与我相关的未读信息设置成已读
        /// </summary>
        /// <param name="userId"></param>
        public void Relate_UpdateRead(int userId)
        {
            try 
            {
                string sql = string.Empty;
                sql = " UPDATE  [dbo].[M_MobileMessages]  SET IsRead=1 WHERE  ToUserID={0}  AND  MessageType IN(1,2) ";
                sql = string.Format(sql,userId);
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
            }
        }

        /// <summary>
        /// 删除与我相关的消息,在删除评论之前删除
        /// </summary>
        /// <param name="commmentID"></param>
        public void Relate_Delete(int commmentID) 
        {
            try
            {
                SqlParameter[] parms = { new SqlParameter("@CommentID", commmentID) };
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_Relate_Del", parms);
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
            }
        
        }
        #endregion 
    }
}
