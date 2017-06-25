using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.BLL
{
   public  class BCtrl_Users
    {
       UserDAO userobj = new UserDAO();
        public bool UpdateUser(UserEntity info)
        {
            return userobj.UpdateUser(info);
        }
        public UserEntity QueryUserInfo(int userid)
        {
            return userobj.QueryUserInfo(userid);
        }
        public UserEntity QueryUserInfo(string loginName)
        {
            return userobj.QueryUserInfo(loginName);
        }
        public UserEntity QueryUserInfo(string loginName, bool isPhone)
        {
            return userobj.QueryUserInfo(loginName, isPhone);
        }

        public bool UpdateMyNickName(int userid, string nickname)
        {
            return userobj.UpdateMyNickName(userid, nickname);
        }

        public bool ChangePwd(string oldpwd, string newpwd, int userid, out string msg)
        {
            return userobj.ChangePwd(oldpwd, newpwd, userid, out msg);
        }

        public bool SaveUserPicture(int pictureid, int userid)
        {
            return userobj.SaveUserPicture(pictureid, userid);
        }

        public bool ClearUser(int userid) 
        {
            return userobj.ClearUser(userid);
        }
    }
}
