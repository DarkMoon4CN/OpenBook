using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.BLL
{
    public class BCtrl_Auth
    {
        AuthDAO authobj = new AuthDAO();
        public UserSessionEntity TryLogin(string loginname, string pwd, out string msg)
        {
             var info=authobj.TryLogin(loginname, pwd, out msg);
             if (info !=null && string.IsNullOrEmpty(info.NickName))
             {
                 BCtrl_Users userobj = new BCtrl_Users();
                 string nickName = "手机用户" + authobj.NickNameMaxID();
                 userobj.UpdateMyNickName(info.UserID, nickName);
                 info.NickName = nickName;
             }
             if (info !=null && string.IsNullOrEmpty(info.UserPictureUrl) && string.IsNullOrEmpty(info.ThirdPictureUrl))
             {
                 info.Domain = string.Empty;
                 
                 info.PicturePath = AppUtil.UserDefaultHeader + AppUtil.ConvertJpg;
             }
             return info;
        }
        public bool AddSnsBinding(int userid, string thridpart_username, string thirdpartuserid, int logintype, string url)
        {
            return authobj.AddSnsBinding(userid, thridpart_username, thirdpartuserid, logintype, url);
        }

        public UserSessionEntity TryLoginX(string thridpart_username, string thirdpartuserid, int logintype,string url)
        {
            var info=authobj.TryLoginX(thridpart_username, thirdpartuserid, logintype,url);
            if (string.IsNullOrEmpty(info.NickName))
            {
                BCtrl_Users userobj = new BCtrl_Users();
                string nickName = "手机用户" + authobj.NickNameMaxID();
                userobj.UpdateMyNickName(info.UserID, nickName);
                info.NickName = nickName;
            }
            if (string.IsNullOrEmpty(info.UserPictureUrl) && string.IsNullOrEmpty(info.ThirdPictureUrl))
            {
                info.Domain = string.Empty;
                info.PicturePath = AppUtil.UserDefaultHeader + AppUtil.ConvertJpg;
            }
            return info;
        }

        public bool AddSmsCodeToDB(string phone, string code, int expriemin)
        {
            return authobj.AddSmsCodeToDB(phone, code,expriemin);
        }

        public bool CheckSmsCode(string phone, string code, SqlTransaction trans)
        {
            return authobj.CheckSmsCode(phone, code, null);
        }

        public int RegesiterNewUser(string phone, string pwd, string code, out string msg, out string nickName)
        {

            return authobj.RegesiterNewUser(phone, pwd, code, out msg, out nickName);

        }

        public bool CheckUserExist(string loginname)
        {
            return authobj.CheckUserExist(loginname);
        }

        public bool BindPhone(int userid, string phone, string code, string pwd, out string msg)
        {
            return authobj.BindPhone(userid, phone, code, pwd, out msg);
        }

        public bool ResetPassword(string phone, string code, string pwd, out string msg)
        {
            return authobj.ResetPassword(phone, code, pwd, out msg);
        }

        public UserSessionEntity IsThereExistUser(string thridpart_username, string thirdpartuserid, int logintype, string url)
        {
            return authobj.IsThereExistUser(thridpart_username, thirdpartuserid, logintype, url);
        }
        
    }
}
