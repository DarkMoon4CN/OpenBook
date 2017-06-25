using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.DAO;
using Mars.Server.Entity;

namespace Mars.Server.BLL
{
   public class BCtrl_UserMail
    {
       UserMailDAO dao = new UserMailDAO();
        public bool Insert(UserMailEntity entity)
        {
            return dao.Insert(entity);
        }

        public bool Update(UserMailEntity entity)
        {
            return dao.Update(entity);
        }

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="enumMailStatus"></param>
        /// <returns></returns>
        public bool UpdateSendStatus(int mailID, string remark, UserMailStatus enumMailStatus)
        {
            return dao.UpdateSendStatus(mailID, remark, enumMailStatus);
        }

          /// <summary>
        /// 查询等待发送邮件列表
        /// </summary>
        /// <param name="enumMailStatus"></param>
        /// <returns></returns>
        public List<UserMailViewEntity> QueryViewWaitSendMailList()
        {
            return dao.QueryViewWaitSendMailList();
        }

         /// <summary>
        /// 获取公司邮件账号列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyEmailAccountEntity> QueryCoEmailAccountList()
        {
            return dao.QueryCoEmailAccountList();
        }
    }
}
