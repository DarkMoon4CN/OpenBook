using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_SMS
    {
        private BCtrl_SMS() { }
        private static BCtrl_SMS _instance;
        public static BCtrl_SMS Instance { get { return _instance ?? (_instance = new BCtrl_SMS()); } }
        private static readonly SMSDAO dao = new SMSDAO();


        public IList<SMSEntity> SMS_GetALL(string strWhere = null)
        {
            return dao.SMS_GetALL(strWhere);
        }

        public DataTable SMS_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            return dao.SMS_GetList(pageIndex, pageSize, orderType, strWhere, out recordCount);
        }

        public bool SMS_Insert(SMSEntity entity)
        {
            return dao.SMS_Insert(entity);
        }

        public bool SMS_Update(string content, int isSend, string modelKey, int smsID)
        {
            return dao.SMS_Update(content, isSend,modelKey,smsID);
        }

        public bool SMS_Delete(int smsID)
        {
            return dao.SMS_Delete(smsID);
        }

        /// <summary>
        /// 按条件删除 谨慎使用
        /// </summary>
        /// <param name="isDelete">判定是否删除,设定为使调用者注意</param>
        /// <param name="sysUserId">系统操作员</param>
        /// <param name="IsSend">发送状态</param>
        /// <returns></returns>
        public bool SMS_Delete(int sysUserID, int isSend = 0, bool isDelete = false)
        {
            return dao.SMS_Delete(sysUserID, isSend, isDelete);
        }
    }
}
