using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System.Data.SqlClient;
using System.Data;

namespace Mars.Server.DAO
{
    public class UserMailDAO
    {
        public bool Insert(UserMailEntity entity)
        {
            bool isSuccess = false;
            string insertSQL = "INSERT INTO M_User_Mail(UserID,EventItemID,CreateDate,MailStatus,Remark) VALUES (@UserID,@EventItemID,@CreateDate,@MailStatus,@Remark)";

            SqlParameter[] prms = new SqlParameter[] { 
                new SqlParameter("@UserID", SqlDbType.Int,4),
                new SqlParameter("@EventItemID",SqlDbType.Int,4),
                new SqlParameter("@CreateDate",SqlDbType.DateTime),
                new SqlParameter("@MailStatus",SqlDbType.Int,4),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500)
            };

            prms[0].Value = entity.UserID;
            prms[1].Value = entity.EventItemID;
            prms[2].Value = entity.CreateDate;
            prms[3].Value = entity.MailStatus;
            prms[4].Value = entity.Remark;
 
            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, insertSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        public bool Update(UserMailEntity entity)
        {
            bool isSuccess = false;
            string insertSQL = "UPDATE M_User_Mail SET UserID=@UserID,EventItemID=@EventItemID,CreateDate=@CreateDate,MailStatus=@MailStatus,Remark=@Remark,SendDate=@SendDate WHERE MailID=@MailID";

            SqlParameter[] prms = new SqlParameter[] { 
                new SqlParameter("@UserID", SqlDbType.Int,4),
                new SqlParameter("@EventItemID",SqlDbType.Int,4),
                new SqlParameter("@CreateDate",SqlDbType.DateTime),
                new SqlParameter("@MailStatus",SqlDbType.Int,4),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@SendDate",SqlDbType.DateTime),
                new SqlParameter("@MailID",SqlDbType.Int,4)
            };

            prms[0].Value = entity.UserID;
            prms[1].Value = entity.EventItemID;
            prms[2].Value = entity.CreateDate;
            prms[3].Value = entity.MailStatus;
            prms[4].Value = entity.Remark;
            prms[5].Value = entity.SendDate;
            prms[6].Value = entity.MailID;

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, insertSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="enumMailStatus"></param>
        /// <returns></returns>
        public bool UpdateSendStatus(int mailID,string remark, UserMailStatus enumMailStatus)
        {
            bool isSuccess = false;
            string updateSQL = "Update M_User_Mail Set MailStatus=@MailStatus, SendDate=@SendDate,Remark=@Remark Where MailID=@MailID";
            SqlParameter[] prms = { 
                                     new SqlParameter("@MailStatus", (int)enumMailStatus),
                                     new SqlParameter("@SendDate", DateTime.Now),
                                     new SqlParameter("@MailID",mailID),
                                     new SqlParameter("@Remark",remark)
                                  };

            try
            {
                isSuccess = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, updateSQL, prms) > 0;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 查询等待发送邮件列表
        /// </summary>
        /// <param name="enumMailStatus"></param>
        /// <returns></returns>
        public List<UserMailViewEntity> QueryViewWaitSendMailList()
        {
            List<UserMailViewEntity> mailList = null;      
            string querySQL = @"SELECT UserID,LoginName,Telphone,EMail,UserName,MailID,CreateDate,SendDate,MailStatus,Remark,EventItemID,Title,BookListPath 
                                FROM M_V_User_Mail WHERE MailStatus="+ (int)UserMailStatus.Newadd +" OR MailStatus=" + (int)UserMailStatus.SendFail;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    mailList = conn.Query<UserMailViewEntity>(querySQL).ToList();
                    mailList = mailList.Count > 0 ? mailList : null;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }

            return mailList;
        }

        /// <summary>
        /// 获取公司邮件账号列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyEmailAccountEntity> QueryCoEmailAccountList()
        {
            string selectSQL = "Select * From M_CompanyEmailAccount Where CompanyEmailStatus=1";

            try
            {
                using (SqlConnection conn = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return conn.Query<CompanyEmailAccountEntity>(selectSQL).ToList();
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
