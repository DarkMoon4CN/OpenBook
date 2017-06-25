using Mars.Server.DAO.Comments;
using Mars.Server.Entity.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Comments
{
    public class BCtrl_SensitiveWordSearch
    {
        SensitiveWordSearchDAO dao = new SensitiveWordSearchDAO();

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(SensitiveWordSearchEntity info, out int totalcnt)
        {
            return dao.QueryData(info, out totalcnt);
        }

        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="swID"></param>
        /// <param name="stateTypeID"></param>
        /// <returns></returns>
        public bool ChangeStateType(int swID,int stateTypeID)
        {
            return dao.ChangeStateType(swID, stateTypeID);
        }

        public bool UpdateSensitiveWord(SensitiveWordEntity item)
        {
            return dao.UpdateSensitiveWord(item);
        }

        public int AddSensitiveWord(SensitiveWordEntity item)
        {
            return dao.AddSensitiveWord(item);
        }
    }
}
