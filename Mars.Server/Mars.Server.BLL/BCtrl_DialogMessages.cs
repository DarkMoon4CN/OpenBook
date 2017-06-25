using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_DialogMessages
    {
        private BCtrl_DialogMessages() { }
        private static BCtrl_DialogMessages _instance;
        public static BCtrl_DialogMessages Instance { get { return _instance ?? (_instance = new BCtrl_DialogMessages()); } }
        private static readonly DialogMessagesDAO dao = new DialogMessagesDAO();


        public OperationResult<IList<DialogMessagesEntity>> DialogMessages_GetWhere(string strWhere = null)
        {
            return dao.DialogMessages_GetWhere(strWhere);
        }

        public OperationResult<bool> DialogMessages_Insert(DialogMessagesEntity entity)
        {
            return dao.DialogMessages_Insert(entity);
        }

        public OperationResult<DataTable> DialogMessages_GetList(int pageIndex, int pageSize, string orderType, string strWhere, out int recordCount)
        {
            return dao.DialogMessages_GetList(pageIndex, pageSize, orderType, strWhere, out recordCount);
        }

        public OperationResult<bool> DialogMessages_Update(DialogMessagesEntity entity) 
        {
            return dao.DialogMessages_Update(entity);
        }

        public OperationResult<bool> DialogMessages_Delete(int messageID)
        {
            return dao.DialogMessages_Delete(messageID);
        }
    }
}
