using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    /*
     * 模块：签到短信模板 
     * 作用：提供展会表的Email发送操作
     * 作者：
     * 时间：2015-11-30
     * 备注：BCtrl_ExhibitionEmail将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class BCtrl_ExhibitionEmail
    {
        private BCtrl_ExhibitionEmail() { }
        private static BCtrl_ExhibitionEmail _instance;
        public static BCtrl_ExhibitionEmail Instance { get { return _instance ?? (_instance = new BCtrl_ExhibitionEmail()); } }
        private static readonly ExhibitionEmailDAO dao = new ExhibitionEmailDAO();

        public OperationResult<bool> ExhibitionEmail_Insert(ExhibitionEmailEntity entity)
        {
            return dao.ExhibitionEmail_Insert(entity);
        }

        public OperationResult<IList<ExhibitionEmailEntity>> ExhibitionEmail_GetWhere(string strWhere = null)
        {
            return dao.ExhibitionEmail_GetWhere(strWhere);
        }

        public OperationResult<bool> ExhibitionEmail_UpdateSendTypeID(int bookListCustomerID, int sendTypeID) 
        {
            return dao.ExhibitionEmail_UpdateSendTypeID(bookListCustomerID,sendTypeID);
        }
        public OperationResult<bool> ExhibitionEmail_UpdateSendTypeID(string customerEmail, int sendTypeID)
        {
            return dao.ExhibitionEmail_UpdateSendTypeID(customerEmail, sendTypeID);
        }
    }
}
