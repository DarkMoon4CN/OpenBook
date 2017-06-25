using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System.Data.SqlClient;
using Dapper;

namespace Mars.Server.DAO
{
    /// <summary>
    /// 公司部门
    /// </summary>
   public class DepartmentDAO
    {
       /// <summary>
       /// 查询部门列表
       /// </summary>
       /// <returns></returns>
        public List<DepartmentEntity> QueryDepartments()
       {
            try
            {
                string sql = "SELECT * FROM M_System_Department";

                using (SqlConnection con = new SqlConnection(SQlHelper.MyConnectStr))
                {
                    return con.Query<DepartmentEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
       }
    }
}
