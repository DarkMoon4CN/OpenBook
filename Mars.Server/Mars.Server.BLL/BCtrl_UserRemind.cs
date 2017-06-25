using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public  class BCtrl_UserRemind
    {
        private BCtrl_UserRemind() { }
        private static BCtrl_UserRemind _instance;
        public static BCtrl_UserRemind Instance { get { return _instance ?? (_instance = new BCtrl_UserRemind()); } }
        private static readonly UserRemindDAO dao = new UserRemindDAO();

        public OperationResult<bool> UserRemind_InsertOrUpdate(IList<UserRemindEntity> entitys,int userID)
        {
            OperationResult<bool> result = new  OperationResult<bool>(OperationResultType.NoChanged,"操作没有引发任何变化，提交取消。",true);
            for (int i = 0; i < entitys.Count; i++)
            {
                var entity=entitys[i];
                string strWhere = " AND UserID={0} AND RemindTypeID={1} ";
                strWhere = string.Format(strWhere,userID,entity.RemindTypeID);
                var existEntitys = dao.UserRemind_GetWhere(strWhere);
                entity.UserID = userID;
                if (existEntitys.AppendData.Count != 0)//已存在的实体
                {
                    result= dao.UserRemind_Update(entity);
                }
                else
                {
                    result = dao.UserRemind_Insert(entity);
                }
                if (result.ResultType != OperationResultType.Success)
                {
                    break;
                }
            }
            return result;
        }

        public OperationResult<IList<UserRemindEntity>> UserRemind_GetWhere(string strWhere = null)
        {
            return dao.UserRemind_GetWhere(strWhere);
        }

        public OperationResult<IList<UserRemindEntity>> UserRemind_GetByUserID(int userID) 
        {
            string strWhere = string.Empty;
            strWhere += "  AND UserID ={0} ORDER BY  RemindTypeID  ASC ";
            strWhere = string.Format(strWhere, userID);
            return dao.UserRemind_GetWhere(strWhere);
        }

        public OperationResult<IList<UserRemindTypeEntity>> UserRemindType_GetWhere(string strWhere = null)
        {
            return dao.UserRemindType_GetWhere(strWhere);
        }

    }
}
