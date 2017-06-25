using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Mars.Server.Entity;
using System.Data.SqlClient;
using Mars.Server.Utils;
namespace Mars.Server.DAO
{
    /*
     * 模块：签到短信模板
     * 作用：提供发送短信时依照模板发送
     * 作者：
     * 时间：2015-11-28
     * 备注：SignSMSModelDAO将引用OperationResult.cs 作为返回数据结果承载体
     */
    public class SignSMSModelDAO
    {
        public OperationResult<IList<SMSModeEntity>> SignSMSModel_GetWhere(string strWhere = null) 
        {
            IList<SMSModeEntity> items = new List<SMSModeEntity>();
            try
            {
                string sql = " SELECT * FROM M_SignSMS_Model  WHERE  1=1 " + strWhere;
                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                   items=con.Query<SMSModeEntity>(sql).ToList();
                   return new OperationResult<IList<SMSModeEntity>>(OperationResultType.Success, "数据完成查询", items);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new OperationResult<IList<SMSModeEntity>>(OperationResultType.Success, "异常结果："+ex.Message) ;
            }
        }
    }
}
