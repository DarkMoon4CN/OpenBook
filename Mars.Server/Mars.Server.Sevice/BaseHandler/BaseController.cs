using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using System.Web.SessionState;
using Mars.Server.Utils;
using System.Web;

namespace Mars.Server.Sevice.BaseHandler
{
    /// <summary>
    /// AJAX基类
    /// </summary>
    public class BaseController
    {
        public UserSessionEntity  CurrentUser
        {
            get
            {
                //if (Session[WebKeys.UserSessionKey] != null)
                //{
                //    return Session[WebKeys.UserSessionKey] as UserSessionEntity;
                //}
                //else
                //{
                //    return null;
                //}
                throw new NotImplementedException();
            }
        }

        public HttpSessionState Session
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }

        /// <summary>
        /// 当前管理员信息
        /// </summary>
        protected AdminSessionEntity CurrentAdmin
        {
            get
            {
                if (Session[WebKeys.AdminSessionKey] != null)
                {
                    return Session[WebKeys.AdminSessionKey] as AdminSessionEntity;
                }
                else
                {
                    LogUtil.WriteLog(" webapp admin session lost to root");
                    return null;
                }
            }
        }

        protected string De(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
    }
}
