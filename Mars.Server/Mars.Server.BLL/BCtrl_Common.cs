using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.BLL
{
    public class BCtrl_Common
    {
        CommonDAO commonobj = new CommonDAO();
        public bool AddFeedback(FeedbackEntity info)
        {
            return commonobj.AddFeedback(info);
        }
        public bool CheckAppUpdateState(int apptype,string clientversion, out AppUpdateEntity versioninfo)
        {   
            AppUpdateEntity info= commonobj.TryGetNewVer(apptype);
            if (clientversion == info.Version)
            {
                versioninfo = null;
                return false;
            }
            else
            {
                versioninfo = info;
                return true;
            }
        }

        public bool CheckAppUpdateState(int apptype, string clientversion,int versiontype, out AppUpdateEntity versioninfo)
        {
            AppUpdateEntity info = commonobj.TryGetNewVer(apptype, versiontype);
            if (info ==null || clientversion == info.Version)
            {
                versioninfo = null;
                return false;
            }
            else
            {
                versioninfo = info;
                return true;
            }
        }

        public DataTable GetAppUpdateList(searchAppUpdateEntity entity, out int totalcnt)
        {
            return commonobj.GetAppUpdateList(entity, out totalcnt);
        }
        public bool UpdateAppVersion(AppUpdateEntity entity)
        {
            return commonobj.UpdateAppVersion(entity);
        }
        public AppUpdateEntity TryGetVersion(int apptype)
        {
            AppUpdateEntity info = commonobj.TryGetVersion(apptype);
            return info;
        }
        public string deleteAppUpdate(int appid)
        {
            return commonobj.deleteAppUpdate(appid);
        }
        /// <summary>
        /// 根据版本号，判断是否进行审核
        /// </summary>
        /// <param name="versionCode"></param>
        /// <returns></returns>
        public bool IsVersionCheck(string versionCode)
        {
            return commonobj.IsVersionCheck(versionCode);
        }
        
    }
}
