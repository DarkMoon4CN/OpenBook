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
    /*
     * 模块：订阅
     * 作用：用户的订阅号与，订阅文章操作
     * 作者：
     * 时间：2016-02-01
     * 备注：SubscriptionDAO 将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class SubscriptionDAO
    {
        /// <summary>
        ///  查询订阅结果集
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="subID">末尾订阅ID</param>
        /// <param name="strWhere">查询条件 用于查询是否订阅条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">起始页 默认为1</param>
        /// <param name="type">YES 已订阅  NO 未订阅</param>
        /// <returns></returns>
        public OperationResult<List<SubscriptionEntity>> Subscription_GetList(int userID, int subID, int pageSize, string strWhere, int pageIndex,SubQueryType type) 
        {
            try
            {
                List<SubscriptionEntity> entitys = new List<SubscriptionEntity>();
                SqlParameter[] prms = {
                                      new SqlParameter("@UserID",userID),
                                      new SqlParameter("@SubID",subID),
                                      new SqlParameter("@StrWhere",strWhere),
                                      new SqlParameter("@PageIndex",pageIndex),
                                      new SqlParameter("@PageSize",pageSize),
                                      new SqlParameter("@QueryType",(int)type)
                                      };
                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.StoredProcedure, "M_Subscription_GetList", prms).Tables[0];
                entitys = ConvertDataTable<SubscriptionEntity>.ConvertToList(table);
                return new OperationResult<List<SubscriptionEntity>>(OperationResultType.Success, "订阅列表获取完成！", entitys);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<List<SubscriptionEntity>>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }

        /// <summary>
        ///  用户批量添加订阅
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="subIDs"></param>
        /// <returns></returns>
        public OperationResult<bool> Subscription_Insert(int userID, List<int> subIDs) 
        {
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (var subID in subIDs)
                    {
                        SqlParameter[] prms =
                        { 
                            new SqlParameter("@UserID",userID),
                            new SqlParameter("@SubID",subID),
                            new SqlParameter("@CreateTime",DateTime.Now),
                        };
                        string sql = string.Empty;
                        sql = " INSERT INTO [dbo].[M_User_Subscription] (UserID,SubID,CreateTime)  ";
                        sql += " VALUES( @UserID,@SubID,@CreateTime) ";
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prms); 
                    }
                    trans.Commit();
                    return new OperationResult<bool>(OperationResultType.Success, "添加订阅成功！",true) ;
                }
                catch (Exception ex) 
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return new OperationResult<bool>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
                }
            }
         
        }

        /// <summary>
        /// 用户批量删除订阅
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="subIDs"></param>
        /// <returns></returns>
        public OperationResult<bool> Subscription_Del(int userID, List<int> subIDs)
        {
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                SqlTransaction trans = null;
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    string sids = string.Empty;
                    for (int i = 0; i < subIDs.Count; i++)
                    {
                        if (i < subIDs.Count - 1)
                        {
                            sids += subIDs[i];
                        }
                        else 
                        {
                            sids += subIDs[i]+",";
                        }
                    }
                    string sql = string.Empty;
                    sql = "DELETE FROM [dbo].[M_User_Subscription] ";
                    sql += " WHERE  UserID={0} AND SubID IN( {1} )";
                    sql = string.Format(sql,userID,sids);
                    SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                    trans.Commit();
                    return new OperationResult<bool>(OperationResultType.Success, "取消订阅成功！", true);
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return new OperationResult<bool>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
                }
            }
        }


        /// <summary>
        /// 获取单条订阅明细
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="subID"></param>
        /// <returns></returns>
        public OperationResult<SubscriptionEntity> Subscription_Get(int userID, int subID) 
        {
            try
            {
                string sql = string.Empty;
                sql += " SELECT s.SubID,s.SubName,s.PinYin,s.[Description],s.SubShortName,fp.PicturePath ,fp.Domain,s.CreateTime ";
                sql += " ,(SELECT   CAST(COUNT(1) AS BIT) FROM [dbo].[M_User_Subscription] WHERE USERID={0} AND  SubID =s.SubID)  AS IsAlready ";
                sql += " FROM M_Subscription AS s ";
                sql += " LEFT JOIN [dbo].[M_V_Picture] AS fp ON s.PictureID= fp.PictureID ";
                sql += " WHERE s.SubID={1} ";
                sql = string.Format(sql,userID,subID);
                List<SubscriptionEntity> entitys = new List<SubscriptionEntity>();
                DataTable table = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql).Tables[0];
                entitys = ConvertDataTable<SubscriptionEntity>.ConvertToList(table);

                if (entitys != null && entitys.Count == 1)
                {
                    return new OperationResult<SubscriptionEntity>(OperationResultType.Success,"订阅信息获取完成！",entitys[0]);
                }
                else 
                {
                    return new OperationResult<SubscriptionEntity>(OperationResultType.NoChanged, "请求的订阅号不存在！");
                }
            }
            catch (Exception ex) 
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<SubscriptionEntity>(OperationResultType.NoConnection, Description.EnumDescription(OperationResultType.NoConnection));
            }
        }


        public OperationResult<EventItemEntity> SubscriptionEventItem_GetList(int subID, int pageSize, string strWhere, int pageIndex) 
        {
            
            return null;
        }
    }
}
