using Mars.Server.DAO.Users;
using Mars.Server.Entity.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Users
{
    public class BCtrl_ReportSearch
    {
        ReportSearchDAO dao = new ReportSearchDAO();

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(ReportSearchEntity info, out int totalcnt)
        {
            return dao.QueryData(info, out totalcnt);
        }

        public bool Delete(int reportID)
        {
            return dao.Delete(reportID);
        }
    }
}
