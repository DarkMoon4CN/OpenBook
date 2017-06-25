using Mars.Server.DAO.Systems;
using Mars.Server.Entity.Systems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Systems
{
    public class BCtrl_StartPicSearch
    {
        StartPicSearchDAO dao = new StartPicSearchDAO();

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(StartPicSearchEntity info, out int totalcnt)
        {
            return dao.QueryData(info, out totalcnt);
        }

        public bool ChangeDefault(int id, int isdefault)
        {
            return dao.ChangeDefault(id, isdefault);
        }

        public bool DeleteStartPic(int id)
        {
            return dao.DeleteStartPic(id);
        }

        public bool Add(StartPicEntity item)
        {
            return dao.Add(item);
        }
    }
}
