using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_SignBook
    {
        private BCtrl_SignBook() { }
        private static BCtrl_SignBook _instance;
        public static BCtrl_SignBook Instance { get { return _instance ?? (_instance = new BCtrl_SignBook()); } }
        private static readonly SignBookDAO dao = new SignBookDAO();

        public bool IsAllowCustomerKey(string customerKey) 
        {
            return dao.IsAllowCustomerKey(customerKey);
        }

        public IList<SignBookEntity> SignBook_Get(string customerKey)
        {
             return dao.SignBook_Get(customerKey);
        }

        public IList<SignBookEntity> SignBook_Get(int signID)
        {
           return dao.SignBook_Get(signID);
        }

        public bool SignBook_Update(string customerKey, int state)
        {
            return dao.SignBook_Update(customerKey, state);
        }

        public bool SignBook_Update(SignBookEntity entity)
        {
            return dao.SignBook_Update(entity);
        }
        public DataTable SignBook_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            return dao.SignBook_GetList(pageIndex, pageSize, orderType, strWhere, out recordCount);
        }

        public bool SignBook_Delete(int signID) 
        {
            return dao.SignBook_Delete(signID);
        }

        public bool IsExistLuckyNumber(string luckyNumber) 
        {
            return dao.IsExistLuckyNumber(luckyNumber);
        }

        public IList<SignBookEntity> SignBook_GetALL(string strWhere=null)
        {
            return dao.SignBook_GetALL(strWhere);
        }

        public bool SignBook_Insert(SignBookEntity entity)
        {
            return dao.SignBook_Insert(entity);
        }

        public int SignBook_Count(string strWhere = null) 
        {
            return dao.SignBook_Count(strWhere);
        }
    }
}
