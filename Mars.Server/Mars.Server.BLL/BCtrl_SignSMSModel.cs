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
     * 作用：提供发送短信时依照模板发送
     * 作者：
     * 时间：2015-11-28
     * 备注：BCtrl_SignSMSModel将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class BCtrl_SignSMSModel
    {
        private BCtrl_SignSMSModel() { }
        private static BCtrl_SignSMSModel _instance;
        public static BCtrl_SignSMSModel Instance { get { return _instance ?? (_instance = new BCtrl_SignSMSModel()); } }
        private static readonly SignSMSModelDAO dao = new SignSMSModelDAO();


        public OperationResult<IList<SMSModeEntity>> SignSMSModel_GetWhere(string strWhere = null)
        {
            return dao.SignSMSModel_GetWhere(strWhere);
        } 


    }
}
