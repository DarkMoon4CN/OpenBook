using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Mars.Server.Entity;
namespace Mars.Server.DAO
{
    public class AuthDAO
    {
        public UserSessionEntity TryLogin(string loginname, string pwd, out string msg)
        {  
            try
            {
                UserSessionEntity info = new UserSessionEntity();
                SqlParameter[] prms = { 
                                        new SqlParameter("@LoginName",loginname)
                                      };
                msg = string.Empty;
                string sql = "select UserID, PassWord,AreaID,Telphone,ThirdWxUserName,ThirdWbUserName,ThirdQqUserNameW,NickName,ThirdPictureUrl,p.PicturePath,p.Domain from M_User m left join M_V_Picture p on m.PictureID=p.PictureID where LoginName=@LoginName ";
                DataTable dt = SQlHelper.ExecuteDataset(SQlHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                    {
                        if (dt.Rows[0]["PassWord"].ToString() == pwd)
                        {
                            msg = "登录成功";
                            msg = dt.Rows[0]["UserID"].ToString();
                            int userid= int.Parse(dt.Rows[0]["UserID"].ToString());
                            int areaid = dt.Rows[0]["AreaID"].ToString() == "" ? 0 : int.Parse(dt.Rows[0]["AreaID"].ToString());
                            info.Telphone = dt.Rows[0]["Telphone"].ToString();
                            info.ThirdWxUserName = dt.Rows[0]["ThirdWxUserName"].ToString();
                            info.ThirdWbUserName = dt.Rows[0]["ThirdWbUserName"].ToString();
                            info.ThirdQqUserNameW = dt.Rows[0]["ThirdQqUserNameW"].ToString();
                            info.NickName = dt.Rows[0]["NickName"].ToString();
                            info.ThirdPictureUrl = dt.Rows[0]["ThirdPictureUrl"].ToString();
                            info.PicturePath = dt.Rows[0]["PicturePath"].ToString();
                            info.Domain = dt.Rows[0]["Domain"].ToString();
                            info.UserID = userid;
                            info.ZoneID = areaid;
                            return info;
                        }
                        else
                        {
                            msg = "用户名或密码错误！";
                            return null ;
                        }
                    }
                    else
                    {
                        msg = "您的账户异常(RepeatEvent)";
                        return null;
                    }
                }
                else
                {
                    msg = "用户名或者密码错误！";
                    return null;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogUtil.WriteLog(ex);
                return null;
            }
        }

        public UserSessionEntity IsThereExistUser(string thridpart_username, string thirdpartuserid, int logintype, string url)
        {
            UserSessionEntity info = new UserSessionEntity();
            SqlParameter[] prms = { 
                                        new SqlParameter("@LoginID",thirdpartuserid),
                                        new SqlParameter("@LoginName",thridpart_username),
                                        new SqlParameter("@LoginType",logintype),
                                        new SqlParameter("@Url",url)
                                      };
            string targetname = string.Empty;
            string targetid = string.Empty;
            switch (logintype)
            {
                case 1:
                    targetname = "ThirdWxUserName";
                    targetid = "ThirdWxID";
                    break;
                case 2:
                    targetname = "ThirdWbUserName";
                    targetid = "ThirdWbID";
                    break;
                case 3:
                    targetname = "ThirdQqUserNameW";
                    targetid = "ThirdQqID";
                    break;
            }
            SqlTransaction trans = null;
            string sql = string.Format("select UserID,AreaID,Telphone,ThirdWxUserName,ThirdWbUserName,ThirdQqUserNameW,ThirdPictureUrl,p.PicturePath,p.Domain,NickName from M_User m left join M_V_Picture p on m.PictureID=p.PictureID where {0}=@LoginID and LoginType=@LoginType ", targetid);
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    DataTable dt = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql, prms).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        int userid = int.Parse(dt.Rows[0]["UserID"].ToString());

                        info.UserID = userid;
                        info.ZoneID = dt.Rows[0]["AreaID"].ToString() == "" ? 0 : int.Parse(dt.Rows[0]["AreaID"].ToString());
                        info.Telphone = dt.Rows[0]["Telphone"].ToString();
                        info.ThirdWxUserName = dt.Rows[0]["ThirdWxUserName"].ToString();
                        info.ThirdWbUserName = dt.Rows[0]["ThirdWbUserName"].ToString();
                        info.ThirdQqUserNameW = dt.Rows[0]["ThirdQqUserNameW"].ToString();
                        info.NickName = dt.Rows[0]["NickName"].ToString();
                        info.ThirdPictureUrl = dt.Rows[0]["ThirdPictureUrl"].ToString();
                        info.PicturePath = dt.Rows[0]["PicturePath"].ToString();
                        info.Domain = dt.Rows[0]["Domain"].ToString();
                        trans.Commit();
                        return info;
                    }
                   
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return null;
                }
            }
            return null;
        }


        public UserSessionEntity TryLoginX(string thridpart_username, string thirdpartuserid, int logintype,string url)
        {
            UserSessionEntity info = new UserSessionEntity();
            SqlParameter[] prms = { 
                                        new SqlParameter("@LoginID",thirdpartuserid),
                                        new SqlParameter("@LoginName",thridpart_username),
                                        new SqlParameter("@LoginType",logintype),
                                        new SqlParameter("@Url",url),
                                        new SqlParameter("@NickName",thridpart_username)
                                        
                                      };
            string targetname = "";
            string targetid = "";
            switch (logintype)
            {
                case 1:
                    targetname = "ThirdWxUserName";
                    targetid = "ThirdWxID";
                    break;
                case 2:
                    targetname = "ThirdWbUserName";
                    targetid = "ThirdWbID";
                    break;
                case 3:
                    targetname = "ThirdQqUserNameW";
                    targetid = "ThirdQqID";
                    break;
            }
            SqlTransaction trans = null;
            string sql = string.Format("select UserID,AreaID,Telphone,ThirdWxUserName,ThirdWbUserName,ThirdQqUserNameW,ThirdPictureUrl,p.PicturePath,p.Domain,NickName from M_User m left join M_V_Picture p on m.PictureID=p.PictureID where {0}=@LoginID  ", targetid);
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    DataTable dt = SQlHelper.ExecuteDataset(trans, CommandType.Text, sql, prms).Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        int userid = int.Parse(dt.Rows[0]["UserID"].ToString());
                       
                        info.UserID = userid;
                        info.ZoneID = dt.Rows[0]["AreaID"].ToString() == "" ? 0 : int.Parse(dt.Rows[0]["AreaID"].ToString());
                        info.Telphone = dt.Rows[0]["Telphone"].ToString();
                        info.ThirdWxUserName = dt.Rows[0]["ThirdWxUserName"].ToString();
                        info.ThirdWbUserName = dt.Rows[0]["ThirdWbUserName"].ToString();
                        info.ThirdQqUserNameW = dt.Rows[0]["ThirdQqUserNameW"].ToString();
                        info.NickName = dt.Rows[0]["NickName"].ToString();
                       
                        info.ThirdPictureUrl = dt.Rows[0]["ThirdPictureUrl"].ToString();
                        info.PicturePath = dt.Rows[0]["PicturePath"].ToString();
                        info.Domain = dt.Rows[0]["Domain"].ToString();
                        trans.Commit();
                        return info;
                    }
                    else
                    {
                        sql = string.Format("insert into M_User(LoginName,PassWord,{0},{1},LoginType,ThirdPictureUrl,NickName) output inserted.UserID values('{2}','{3}',@LoginID,@LoginName,@LoginType,@Url,@NickName)",
                                         targetid, targetname, Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
                        int userid = int.Parse(SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms).ToString());
                        trans.Commit();
                        info.UserID=userid;
                        info.NickName = thridpart_username;
                        info.ZoneID=0;
                        switch (logintype)
                        { 
                            case 1:
                                info.ThirdWxUserName = thridpart_username;
                                break;
                            case 2:
                                info.ThirdWbUserName = thridpart_username;
                                break;
                            case 3:
                                info.ThirdQqUserNameW = thridpart_username;
                                break;
                        }
                        info.Domain = string.Empty;
                        if (string.IsNullOrEmpty(url))
                        {
                            url = "http://";
                        }
                        info.ThirdPictureUrl = url;
                        return info;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogUtil.WriteLog(ex);
                    return null;
                }
            }

        }

        public bool AddSnsBinding(int userid, string thridpart_username, string thirdpartuserid, int logintype, string url)
        {
            SqlParameter[] prms = { 
                                        new SqlParameter("@LoginID",thirdpartuserid),
                                        new SqlParameter("@LoginName",thridpart_username),
                                        new SqlParameter("@LoginType",logintype),
                                        new SqlParameter("@Url",url),
                                        new SqlParameter("@UserID",userid)
                                      };
            string targetname = "";
            string targetid = "";
            switch (logintype)
            {
                case 1:
                    targetname = "ThirdWxUserName";
                    targetid = "ThirdWxID";
                    break;
                case 2:
                    targetname = "ThirdWbUserName";
                    targetid = "ThirdWbID";
                    break;
                case 3:
                    targetname = "ThirdQqUserNameW";
                    targetid = "ThirdQqID";
                    break;
            }
            string sql =string.Format( "update M_User set {0}=@LoginName,{1}=@LoginID,ThirdPictureUrl=@Url,LoginType=@LoginType where UserID=@UserID",targetname,targetid);
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

        public bool AddSmsCodeToDB(string phone, string code, int expriemin)
        {
            try
            {
                string sql = "insert into M_SmsCodes(Phone,VCode,SendTIme,ExpriedTime) values(@Phone,@VCode,getdate(),DATEADD(\"mi\",2,GETDATE()))";
                SqlParameter[] prms = { 
                                      new SqlParameter("@Phone",phone),
                                      new SqlParameter("@VCode",code)
                                      };
                SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, sql, prms);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public bool CheckSmsCode(string phone, string code, SqlTransaction trans)
        {
            try
            {
                SqlParameter[] prms = { 
                                      new SqlParameter("@Phone",phone),
                                      new SqlParameter("@VCode",code)
                                      };
                string sql = "SELECT count(1) FROM [dbo].[M_SmsCodes] WHERE Phone=@Phone AND VCode=@VCode AND ExpriedTime>=GETDATE()";
                if (trans != null)
                {
                    return int.Parse(SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms).ToString()) > 0;
                }
                else
                {
                    return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sql, prms).ToString()) > 0;
                }

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public int RegesiterNewUser(string phone, string pwd, string code, out string msg,out string nickName)
        {

            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                nickName = "手机用户" + NickNameMaxID();
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    if (CheckSmsCode(phone, code, trans))
                    {
                        string sqlcheck = "select count(1) from M_User where ( LoginName=@LoginName or Telphone=@LoginName )";
                        string sql = "insert into M_User(LoginName,PassWord,Telphone,NickName) output inserted.UserID values (@LoginName,@PassWord,@Telphone,@NickName) ";
                        SqlParameter[] prms = { 
                                                new SqlParameter("@LoginName",phone),
                                                new SqlParameter("@PassWord",pwd),
                                                new SqlParameter("@Telphone",phone),
                                                new SqlParameter("@NickName",nickName)
                                              };
                       
                        if (int.Parse(SQlHelper.ExecuteScalar(trans, CommandType.Text, sqlcheck, prms).ToString()) > 0)
                        {
                            msg =string.Format( "系统中已经存在登录名为:{0}的账户",phone);
                            trans.Rollback();
                            return 0;
                        }
                        else
                        {
                            int userid = int.Parse(SQlHelper.ExecuteScalar(trans, CommandType.Text, sql, prms).ToString());
                            //注册推荐码更新设置 -By Edward
                            StringBuilder strUserRecommend = new StringBuilder();
                            strUserRecommend.AppendFormat(@"INSERT INTO M_User_Recommend(UserID,RecommendID)
                              SELECT TOP 1 u.UserID,um.RecommendID
                              FROM dbo.M_User AS u
                              INNER JOIN dbo.M_User_Recommend_Mobile AS um ON u.LoginName=um.Mobile OR u.Telphone = um.Mobile
                              LEFT JOIN dbo.M_User_Recommend AS ur ON ur.UserID = u.UserID
                              WHERE ur.UserID IS NULL AND um.Mobile='{0}' ORDER BY um.RMID ASC"
                            , phone);
                            SQlHelper.ExecuteNonQuery(trans, CommandType.Text, strUserRecommend.ToString(), null).ToString();

                            trans.Commit();
                            msg = "注册成功";
                            return userid;
                        }
                      
                    }
                    else
                    {
                        msg = "验证码无效";
                        return 0;
                    }
                     

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogUtil.WriteLog(ex);
                    return 0;
                }
            }

        }

        public bool CheckUserExist(string loginname)
        {
            try
            {
                string sqlcheck = "select count(1) from M_User where LoginName=@LoginName";
                SqlParameter[] prms = { 
                                                new SqlParameter("@LoginName",loginname)
                                              };
                return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqlcheck, prms).ToString()) > 0;

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return true;
            }
        }

        


        public bool BindPhone(int userid, string phone,string code,string pwd, out string msg)
        {
            msg = string.Empty;
            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    if (CheckSmsCode(phone, code, trans))
                    {

                        string check = "select count(*) from M_User where Telphone=@Telphone";

                        string sql = "update M_User set LoginName=@LoginName,Telphone=@Telphone,PassWord=@Pwd where UserID=@UserID";
                        SqlParameter[] prms = { 
                                              new SqlParameter("@Telphone",phone),
                                              new SqlParameter("@LoginName",phone),
                                              new SqlParameter("@UserID",userid),
                                              new SqlParameter("@Pwd",pwd)
                                              };

                        int cnt = int.Parse(SQlHelper.ExecuteScalar(trans,CommandType.Text,check,prms).ToString());
                        if (cnt == 0)
                        {
                            SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prms);
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            trans.Rollback();
                            msg = "此手机号已经被其他账号绑定，请更换另一个手机号后重试！";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "验证码无效";
                        trans.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    msg = ex.Message;
                    LogUtil.WriteLog(ex);
                    return false;
                }
            }
           
        }

        public bool ResetPassword(string phone, string code, string pwd, out string msg)
        {
            msg = string.Empty;
            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    if (CheckSmsCode(phone, code, trans))
                    {
                        string sql = "update M_User set PassWord=@Pwd where Telphone=@Telphone";
                        SqlParameter[] prms = { 
                                              new SqlParameter("@Telphone",phone),
                                              new SqlParameter("@Pwd",pwd)
                                              };
                        SQlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prms);
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        msg = "验证码无效";
                        trans.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    msg = ex.Message;
                    LogUtil.WriteLog(ex);
                    return false;
                }
            }

        }

        /// <summary>
        ///   NickName 随机数
        /// </summary>
        /// <returns></returns>
        public int NickNameMaxID()
        {
            try
            {
                string sqlcheck = "  INSERT INTO  M_User_NickNameMaxID (IsUse)  VALUES(1);SELECT SCOPE_IDENTITY() ";
                return int.Parse(SQlHelper.ExecuteScalar(SQlHelper.MyConnectStr, CommandType.Text, sqlcheck).ToString());
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return 0;
            }
        }
    }
}
