using Mars.Server.DAO;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Mars.Server.Entity
{
    public class UserDAO
    {
        public bool UpdateUser(UserEntity info)
        {
            SqlParameter[] prms ={ 
                new SqlParameter("@EMail",info.EMail),
                new SqlParameter("@Name",info.Name),
                new SqlParameter("@Company",info.Company),
                new SqlParameter("@Position",info.Position),
                new SqlParameter("@Department",info.Department),
                new SqlParameter("@Address",info.Address),
                new SqlParameter("@UserID",info.UserID),
                new SqlParameter("@NickName",info.NickName)
            };
            string sql = "update M_User set NickName=@NickName, EMail = @EMail,Name = @Name,Company = @Company,Position = @Position,Department = @Department,Address = @Address where UserID = @UserID";
            try
            {
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public UserEntity QueryUserInfo(int userid)
        {
            SqlParameter[] prms ={ 
                new SqlParameter("@UserID",userid)
            };
            string sql = "select UserID,EMail,Name,Company,Position,Department,Address, NickName from M_User  where UserID= @UserID";
            try
            {
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<UserEntity>(sql, new { UserID = userid }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public UserEntity QueryUserInfo(string loginName)
        {
            SqlParameter[] prms ={ 
                new SqlParameter("@LoginName",loginName)
            };
            string sql = "select UserID,EMail,Name,Company,Position,Department,Address, NickName from M_User  where LoginName= @LoginName";
            try
            {
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<UserEntity>(sql, new { LoginName = loginName }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public UserEntity QueryUserInfo(string loginName,bool isPhone)
        {
            string strWhere = string.Empty;
            if (isPhone)
            {
                strWhere="OR  telphone='{0}'";
                strWhere = string.Format(strWhere,loginName);
            }
            string sql = "select UserID,EMail,Name,Company,Position,Department,Address, NickName from M_User  where LoginName= '{0}' " +strWhere;
            sql = string.Format(sql,loginName);
            try
            {
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<UserEntity>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }


        }




        public bool UpdateMyNickName(int userid, string nickname)
        {
            try
            {
                string sql = "update M_User set NickName=@NickName where UserID=@UserID";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { @UserID = userid, NickName = nickname });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool ChangePwd(string oldpwd, string newpwd, int userid,out string msg)
        {
            msg = "";
            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    string sqlcheck = "select PassWord from M_User where UserID=@UserID ";
                    string pwd = con.Query<string>(sqlcheck, new { UserID = userid }, transaction: trans).FirstOrDefault();
                    if (pwd == oldpwd)
                    {
                        string sql = "update M_User set PassWord=@Pwd where UserID=@UserID ";
                        con.Execute(sql, new { Pwd = newpwd, UserID = userid },transaction:trans);
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        trans.Rollback();
                        msg = "旧密码输入错误！";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    msg = ex.Message;
                    return false;
                }
            }
        }

        public bool SaveUserPicture(int pictureid, int userid)
        {
            try
            {
                string sql = "update M_User set PictureID=@PictureID where UserID=@UserID";
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    con.Execute(sql, new { PictureID = pictureid, UserID = userid });
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool ClearUser(int userid) 
        {
            try
            {
                string sql1 = "DELETE M_User WHERE  UserID="+userid;
                string sql2 = "DELETE M_EventItem WHERE  UserID=" + userid;
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql1);
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql2);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

    }
}
