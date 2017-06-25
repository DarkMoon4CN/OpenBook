using Mars.Server.DAO.Comments;
using Mars.Server.Entity.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL.Comments
{
    public class BCtrl_CommentSearch
    {
        CommentSearchDAO dao = new CommentSearchDAO();

        /// <summary>
        /// 使用缓存查询信息
        /// </summary>
        /// <param name="info">查询主体</param>
        /// <param name="totalcnt">返回条目数</param>
        /// <returns></returns>
        public DataTable QueryData(CommentSearchEntity info, out int totalcnt)
        {
            return dao.QueryData(info, out totalcnt);
        }

        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="viewStateID"></param>
        /// <returns></returns>
        public bool ChangeViewState(int commentID, int viewStateID)
        {
            return dao.ChangeViewState(commentID, viewStateID);
        }
    }
}
