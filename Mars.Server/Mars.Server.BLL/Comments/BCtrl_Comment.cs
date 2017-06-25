using Mars.Server.DAO.Comments;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace Mars.Server.BLL.Comments
{
    public class BCtrl_Comment
    {
        CommentDAO dao = new CommentDAO();
        public int ILikeThis(int id, int uid)
        {
            return dao.ILikeThis(id, uid);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public bool DeleteComment(int cid,int userid)
        {
            return dao.DeleteComment(cid,userid);
        }

        public SqlDataReader GetWillCheckCommentReader(bool isAll = false)
        {
            return dao.GetWillCheckCommentReader(isAll);
        }

        public bool WriteBack(DataTable returnTable)
        {
            bool returnValue = false;

            if (returnTable.Rows.Count > 0)
            {
                //SqlBulkCopy 写入数据库 然后进行批量改写
                if (dao.WriteBack(returnTable))
                {
                    //批量改写状态
                    returnValue = dao.UpdateCheckType();
                }
            }

            return returnValue; 
        }

        public void UpdateNewWordState(Dictionary<int, string> wordList)
        {
            dao.UpdateNewWordState(wordList);
        }
    }
}
